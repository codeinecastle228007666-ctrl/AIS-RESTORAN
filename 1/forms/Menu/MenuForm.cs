// Форма просмотра и редактирования меню
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
    // Управление меню: просмотр блюд, поиск, добавление, редактирование, удаление.
    // Кнопки: "Добавить" (buttonAdd), "Изменить" (buttonRed), "Удалить" (buttonDelete), поиск (textBoxSearch)
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

        // Загрузка меню с фильтрацией по поиску.
        // SQL: параметр @search передаётся через NpgsqlParameter, а не конкатенация строк —
        // защита от SQL-инъекций. LIKE работает на уровне сервера (LOWER b.nazvanie).
        void LoadMenu(string search)
        {
            string sql = @"
                SELECT b.bludo_id AS ""ID"",
                b.nazvanie AS ""Название"",
                km.nazvanie AS ""Категория"",
                b.cena AS ""Цена"",
                b.opisanie AS ""Описание""
                FROM bludo b
                JOIN kategoriya_menu km ON b.kategoriya_id = km.kategoriya_id
                WHERE LOWER(b.nazvanie) LIKE LOWER(@search)
                ORDER BY b.nazvanie
            ";

            // Параметр @search: значение подставляется СУБД, а не встраивается в SQL-строку.
            dataGridView1.DataSource = Db.GetData(sql, new Npgsql.NpgsqlParameter("@search", $"%{search}%"));
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
                LoadMenu("");
        }

        private void buttonRed_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            BludoEditForm editForm = new BludoEditForm(id);
            if (editForm.ShowDialog() == DialogResult.OK)
                LoadMenu("");
        }

        // Удаление выбранного блюда и его состава.
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            if (MessageBox.Show("Вы уверены, что хотите удалить это блюдо?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.No) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            string sql1 = "DELETE FROM sostav_bluda WHERE bludo_id=@id";
            string sql2 = "DELETE FROM bludo WHERE bludo_id=@id";

            try
            {
                // Транзакция: каскадное удаление вручную (сначала состав, потом блюдо).
                // Используется сессионное подключение — все команды в одном соединении.
                var conn = Db.GetSessionConnection();
                using var tr = conn.BeginTransaction();

                using (var cmd1 = new Npgsql.NpgsqlCommand(sql1, conn, tr))
                { cmd1.Parameters.AddWithValue("@id", id); cmd1.ExecuteNonQuery(); }

                using (var cmd2 = new Npgsql.NpgsqlCommand(sql2, conn, tr))
                { cmd2.Parameters.AddWithValue("@id", id); cmd2.ExecuteNonQuery(); }

                tr.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления блюда:\n" + ex.Message);
                return;
            }

            LoadMenu("");
        }
    }
}
