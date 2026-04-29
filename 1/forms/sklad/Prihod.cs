using _1.data;
using Npgsql;
using System;
using System.Globalization;
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

                CultureInfo ruCulture = new CultureInfo("ru-RU");

                if (!decimal.TryParse(textBox1.Text, NumberStyles.Any, ruCulture, out decimal kolvo) || kolvo <= 0)
                {
                    MessageBox.Show("Количество должно быть больше 0");
                    return;
                }

                if (!decimal.TryParse(textBox2.Text, NumberStyles.Any, ruCulture, out decimal cena) || cena <= 0)
                {
                    MessageBox.Show("Цена должна быть больше 0");
                    return;
                }

                kolvo = Math.Round(kolvo, 3);
                cena = Math.Round(cena, 2);

                string sql = @"
                    CALL prihod_producta(@p_product_id, @p_kolichestvo);
                    INSERT INTO sklad_dvizhenie (product_id, zakaz_id, tip, kolichestvo, postavschik_id, cena, data_dvizheniya)
                    VALUES (@productId, NULL, 'IN', @kolvo, @postavId, @cena, now());
                ";

                Db.ekzekuttranzakcii(sql,
                    new NpgsqlParameter("@p_product_id", productId),
                    new NpgsqlParameter("@p_kolichestvo", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = kolvo },
                    new NpgsqlParameter("@productId", productId),
                    new NpgsqlParameter("@kolvo", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = kolvo },
                    new NpgsqlParameter("@postavId", postavId),
                    new NpgsqlParameter("@cena", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = cena }
                );

                MessageBox.Show($"Приход сохранён!\n\nТовар: {comboBox1.Text}\nКоличество: {kolvo:0.000}\nЦена: {cena:0.00}", "Успех");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
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
