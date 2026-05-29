// Форма выбора блюда для добавления в заказ
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

namespace _1.forms.zakazo
{
    // Форма выбора блюда с проверкой остатков для добавления в заказ.
    public partial class dobavlenie_bludaForm : Form
    {
        public dobavlenie_bludaForm()
        {
            InitializeComponent();
        }

        // Загрузка списка блюд при открытии.
        private void dobavlenie_bludaForm_Load(object sender, EventArgs e)
        {
            LoadBluda("");
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 50;
            numericUpDown1.Value = 1;
        }

        // Загрузка блюд с фильтрацией по поиску.
        private void LoadBluda(string search)
        {
            string sql = @"
                SELECT
                bludo_id AS ""ID"",
                nazvanie AS ""Название"", 
                cena AS ""Цена""
                FROM bludo
                WHERE LOWER (nazvanie) LIKE LOWER (@search)
                ORDER BY nazvanie
            ";
            dataGridView1.DataSource = Db.GetData(sql, new Npgsql.NpgsqlParameter("@search", $"%{search}%"));
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Поиск при вводе текста.
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadBluda(textBox1.Text);
        }

        // Проверка наличия ингредиентов на складе для выбранного блюда.
        private bool CheckStock(int bludoId, int quantity)
        {
            string sql = @"
                SELECT p.nazvanie AS product_name,
                       (s.kolichestvo / NULLIF(sb.kolichestvo * @kolvo, 0)) AS porciy_mozhno,
                       s.kolichestvo AS ostatok,
                       sb.kolichestvo AS trebuetsya_na_porciyu
                FROM sostav_bluda sb
                JOIN product p ON p.product_id = sb.product_id
                JOIN sklad s ON s.product_id = sb.product_id
                WHERE sb.bludo_id = @bludoId
                  AND s.kolichestvo < sb.kolichestvo * @kolvo
            ";
            var dt = Db.GetData(sql,
                new Npgsql.NpgsqlParameter("@bludoId", bludoId),
                new Npgsql.NpgsqlParameter("@kolvo", quantity));

            if (dt.Rows.Count > 0)
            {
                StringBuilder msg = new StringBuilder("Недостаточно ингредиентов для приготовления блюда!\n\n");
                foreach (DataRow row in dt.Rows)
                {
                    decimal ostatok = Convert.ToDecimal(row["ostatok"]);
                    decimal trebuetsya = Convert.ToDecimal(row["trebuetsya_na_porciyu"]) * quantity;
                    msg.AppendLine($"- {row["product_name"]}: остаток {ostatok}, требуется {trebuetsya}");
                }
                MessageBox.Show(msg.ToString(), "Недостаточно продуктов", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // Подтверждение выбора блюда.
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int bludoId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            int kolvo = (int)numericUpDown1.Value;

            if (!CheckStock(bludoId, kolvo)) return;

            SelectedBludoId = bludoId;
            cena = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Цена"].Value);
            kolichestvo = kolvo;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public int SelectedBludoId { get; private set; }
        public decimal cena { get; private set; }
        public int kolichestvo { get; private set; }
    }
}
