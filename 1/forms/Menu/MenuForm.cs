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

namespace _1.forms.Menu
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            LoadMenu("");
        }

        void LoadMenu(string search)
        {
            string sql = $@"
                SELECT b.bludo_id AS ""ID"",
                b.nazvanie AS ""Название"",
                km.nazvanie AS ""Категория"",
                b.cena AS ""Цена"",
                b.opisanie AS ""Описание""
                FROM bludo b
                JOIN kategoriya_menu km ON b.kategoriya_id = km.kategoriya_id
                WHERE LOWER(b.nazvanie) LIKE LOWER('%{search}%')
                ORDER BY b.nazvanie
            ";

            dataGridView1.DataSource = Db.GetData(sql);
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            LoadMenu(textBoxSearch.Text);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            BludoEditForm editForm = new BludoEditForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadMenu("");
            }
        }

        private void buttonRed_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            BludoEditForm editForm = new BludoEditForm(id);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadMenu("");
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            if (MessageBox.Show("Вы уверены, что хотите удалить это блюдо?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.No) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            string sql1 = "DELETE FROM sostav_bluda WHERE bludo_id=@id";
            string sql2 = "DELETE FROM bludo WHERE bludo_id=@id";

            Db.ekzekuttranzakcii(sql1, new Npgsql.NpgsqlParameter("@id", id));
            Db.ekzekuttranzakcii(sql2, new Npgsql.NpgsqlParameter("@id", id));

            LoadMenu("");

        }
    }
}
