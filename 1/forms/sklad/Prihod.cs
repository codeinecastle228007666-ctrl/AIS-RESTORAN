using _1.data;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1.forms.sklad
{
    public partial class Prihod : Form
    {
        public Prihod()
        {
            InitializeComponent();
        }



        private void LoadPostavshiki()
        {
            string sql = "SELECT postavschik_id, nazvanie FROM postavschik ORDER BY nazvanie";
            comboBox2.DataSource = Db.GetData(sql);
            comboBox2.DisplayMember = "nazvanie";
            comboBox2.ValueMember = "postavschik_id";

        }


        private void LoadProducts()
        {
            string sql = "SELECT product_id, nazvanie FROM product ORDER BY nazvanie";
            comboBox1.DataSource = Db.GetData(sql);
            comboBox1.DisplayMember = "nazvanie";
            comboBox1.ValueMember = "product_id";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // ========== 1. ПРОВЕРКА ВЫБОРА ==========
                if (comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите продукт");
                    return;
                }

                if (comboBox2.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите поставщика");
                    return;
                }

                // ========== 2. ПОЛУЧЕНИЕ ID ==========
                if (comboBox1.SelectedValue == null || comboBox1.SelectedValue == DBNull.Value)
                {
                    MessageBox.Show("Ошибка: не удалось получить ID продукта");
                    return;
                }

                if (comboBox2.SelectedValue == null || comboBox2.SelectedValue == DBNull.Value)
                {
                    MessageBox.Show("Ошибка: не удалось получить ID поставщика");
                    return;
                }

                int productId = Convert.ToInt32(comboBox1.SelectedValue);
                int postavId = Convert.ToInt32(comboBox2.SelectedValue);

                // ========== 3. ПАРСИНГ ==========
                System.Globalization.CultureInfo ruCulture = new System.Globalization.CultureInfo("ru-RU");

                if (!decimal.TryParse(textBox1.Text, System.Globalization.NumberStyles.Any, ruCulture, out decimal kolvo) || kolvo <= 0)
                {
                    MessageBox.Show("Количество должно быть больше 0");
                    return;
                }

                if (!decimal.TryParse(textBox2.Text, System.Globalization.NumberStyles.Any, ruCulture, out decimal cena) || cena <= 0)
                {
                    MessageBox.Show("Цена должна быть больше 0");
                    return;
                }

                kolvo = Math.Round(kolvo, 3);
                cena = Math.Round(cena, 2);

                using var conn = Db.GetConnection();
                conn.Open();
                using var tr = conn.BeginTransaction();

                // ========== ГЛАВНОЕ: Устанавливаем ID пользователя для триггера ==========
                // Используем 1 как ID пользователя по умолчанию (можно заменить на реальный ID текущего пользователя)
                string setUserId = "SET LOCAL app.user_id = '1'";
                using (var setCmd = new NpgsqlCommand(setUserId, conn, tr))
                {
                    setCmd.ExecuteNonQuery();
                }

                // ========== 4. РАБОТА С sklad ==========
                string checkSql = "SELECT COUNT(*) FROM sklad WHERE product_id = @productId";
                using (var checkCmd = new NpgsqlCommand(checkSql, conn, tr))
                {
                    checkCmd.Parameters.Add("@productId", NpgsqlTypes.NpgsqlDbType.Integer).Value = productId;
                    long count = (long)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        string insertSql = @"INSERT INTO sklad (product_id, kolichestvo) VALUES (@productId, @kolvo)";
                        using (var insertCmd = new NpgsqlCommand(insertSql, conn, tr))
                        {
                            insertCmd.Parameters.Add("@productId", NpgsqlTypes.NpgsqlDbType.Integer).Value = productId;
                            insertCmd.Parameters.Add("@kolvo", NpgsqlTypes.NpgsqlDbType.Numeric).Value = kolvo;
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string updateSql = @"UPDATE sklad SET kolichestvo = kolichestvo + @kolvo WHERE product_id = @productId";
                        using (var updateCmd = new NpgsqlCommand(updateSql, conn, tr))
                        {
                            updateCmd.Parameters.Add("@kolvo", NpgsqlTypes.NpgsqlDbType.Numeric).Value = kolvo;
                            updateCmd.Parameters.Add("@productId", NpgsqlTypes.NpgsqlDbType.Integer).Value = productId;
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }

                // ========== 5. ВСТАВКА В sklad_dvizhenie ==========
                string kolvoStr = kolvo.ToString(System.Globalization.CultureInfo.InvariantCulture);
                string cenaStr = cena.ToString(System.Globalization.CultureInfo.InvariantCulture);

                string sql2 = $@"
            INSERT INTO sklad_dvizhenie
            (product_id, zakaz_id, tip, kolichestvo, postavschik_id, cena, data_dvizheniya)
            VALUES
            ({productId}, NULL, 'IN', {kolvoStr}, {postavId}, {cenaStr}, now())
        ";

                using (var cmd2 = new NpgsqlCommand(sql2, conn, tr))
                {
                    cmd2.ExecuteNonQuery();
                }

                tr.Commit();
                MessageBox.Show($"Приход сохранён!\n\nТовар: {comboBox1.Text}\nКоличество: {kolvo:0.000}\nЦена: {cena:0.00}", "Успех");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($" ОШИБКА: {ex.Message}\n\n{ex.StackTrace}", "Ошибка");
            }
        }

        private void Prihod_Load(object sender, EventArgs e)
        {
            LoadPostavshiki();
            LoadProducts();
            textBox1.KeyPress += numeric_KeyPress;
            textBox2.KeyPress += numeric_KeyPress;
        }

        private void numeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox box = sender as TextBox;

            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != ',' &&
                e.KeyChar != '\b')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && box.Text.Contains(","))
            {
                e.Handled = true;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBox1.Text, out decimal value))
                textBox1.Text = value.ToString("0.000");
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBox2.Text, out decimal value))
                textBox2.Text = value.ToString("0.00");
        }
    }
}