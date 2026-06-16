// Форма управления составом блюда (ингредиенты и их количество)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _1.data;
using Npgsql;

namespace _1.forms.Menu
{
    // Форма для добавления и удаления ингредиентов в составе блюда.
    public partial class SostavBludaForm : Form
    {
        int bludoId;
        public SostavBludaForm(int id)
        {
            InitializeComponent();
            bludoId = id;
        }

        private void SostavBludaForm_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadSostav();
            string sql = "SELECT nazvanie FROM bludo WHERE bludo_id = @id";
            var dt = Db.GetData(sql, new NpgsqlParameter("@id", bludoId));
            string name = dt.Rows.Count > 0 ? dt.Rows[0]["nazvanie"].ToString() : bludoId.ToString();
            this.Text = "Состав блюда: " + name;
        }

        // Загрузка списка продуктов для выбора.
        private void LoadProducts()
        {
            string sql = "SELECT product_id, nazvanie FROM product ORDER BY nazvanie";
            comboBoxProduct.DataSource = Db.GetData(sql);
            comboBoxProduct.DisplayMember = "nazvanie";
            comboBoxProduct.ValueMember = "product_id";
        }

        // Загрузка текущего состава блюда.
        private void LoadSostav()
        {
            string sql = @"
                SELECT sb.sostav_bluda_id AS ""ID"",
                p.nazvanie AS ""Продукт"",
                e.nazvanie AS ""Ед. изм."",
                sb.kolichestvo AS ""Количество""
                FROM sostav_bluda sb
                JOIN product p ON sb.product_id = p.product_id
                JOIN edinica_izmereniya e ON p.edinica_izmereniya_id = e.edinica_izmereniya_id
                WHERE sb.bludo_id = @id
                ORDER BY p.nazvanie
            ";
            dataGridView1.DataSource = Db.GetData(sql, new NpgsqlParameter("@id", bludoId));
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Добавление ингредиента в состав блюда.
        private void button1_Click(object sender, EventArgs e)
        {
            int productId = Convert.ToInt32(comboBoxProduct.SelectedValue);

            if (!decimal.TryParse(textBoxKolvo.Text, out decimal kolvo) || kolvo <= 0)
            {
                MessageBox.Show("Введите количество");
                return;
            }

            string sql = @"
                INSERT INTO sostav_bluda
                (bludo_id, product_id, kolichestvo)
                VALUES
                (@b, @p, @k)
            ";

            try
            {
                Db.ekzekuttranzakcii(sql,
                    new NpgsqlParameter("@b", bludoId),
                    new NpgsqlParameter("@p", productId),
                    new NpgsqlParameter("@k", kolvo)
                );
                LoadSostav();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления ингредиента:\n" + ex.Message);
            }
        }

        // Удаление ингредиента из состава.
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            string productName = dataGridView1.CurrentRow.Cells["Продукт"].Value?.ToString() ?? "";
            if (MessageBox.Show($"Удалить ингредиент \"{productName}\" из состава блюда?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            string sql = "DELETE FROM sostav_bluda WHERE sostav_bluda_id=@id";

            try
            {
                Db.ekzekuttranzakcii(sql,
                   new NpgsqlParameter("@id", id)
                );
                LoadSostav();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления ингредиента:\n" + ex.Message);
            }
        }
    }
}
