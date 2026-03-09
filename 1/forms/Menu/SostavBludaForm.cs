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
            this.Text = "Состав блюда: " + bludoId;

        }

        private void LoadProducts()
        {
            string sql = "SELECT product_id, nazvanie FROM product ORDER BY nazvanie";
            comboBoxProduct.DataSource = Db.GetData(sql);
            comboBoxProduct.DisplayMember = "nazvanie";
            comboBoxProduct.ValueMember = "product_id";
        }

        private void LoadSostav()
        {
            string sql = $@"
                SELECT sb.sostav_bluda_id AS ""ID"",
                p.nazvanie AS ""Продукт"",
                sb.kolichestvo AS ""Количество""
                FROM sostav_bluda sb
                JOIN product p ON sb.product_id = p.product_id
                WHERE sb.bludo_id = {bludoId}
                ORDER BY p.nazvanie
            ";
            dataGridView1.DataSource = Db.GetData(sql);
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

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

            Db.ekzekuttranzakcii(sql,
                new NpgsqlParameter("@b", bludoId),
                new NpgsqlParameter("@p", productId),
                new NpgsqlParameter("@k", kolvo)
            );

            LoadSostav();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            string sql = "DELETE FROM sostav_bluda WHERE sostav_bluda_id=@id";

            Db.ekzekuttranzakcii(sql,
               new NpgsqlParameter("@id", id)
               );

            LoadSostav();
        }
    }
}
