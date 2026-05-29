// Форма списка клиентов (CRUD + поиск)
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
using _1.forms.clients;

namespace _1
{
    // Список клиентов: просмотр, добавление, редактирование, удаление, поиск.
    public partial class Clienti : Form
    {
        public Clienti()
        {
            InitializeComponent();
            LoadClients();
        }

        // Загрузка списка клиентов из БД в DataGridView.
        void LoadClients()
        {
            string sql = @"
            SELECT 
                client_id AS ""ID"",
                fio AS ""ФИО"",
                nomer_telefona AS ""Телефон""
            FROM client
            ORDER BY fio
                ";
            dataTable = Db.GetData(sql);
            bindingSource.DataSource = dataTable;
            dataGridView1.DataSource = bindingSource;
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Кнопка для добавления клиента.
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            clientsRedForm redForm = new clientsRedForm();
            if (redForm.ShowDialog() == DialogResult.OK)
                LoadClients();
        }

        // Кнопка для редактирования выбранного клиента.
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            clientsRedForm redForm = new clientsRedForm(id);

            if (redForm.ShowDialog() == DialogResult.OK)
                LoadClients();
        }

        // Удаление выбранного клиента с подтверждением.
        // SQL: параметр @id — защита от SQL-инъекций. Значение не встраивается в строку.
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            if (MessageBox.Show("Вы уверены, что хотите удалить этого клиента?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            string sql = "DELETE FROM client WHERE client_id = @id";

            Db.ekzekuttranzakcii(sql, new Npgsql.NpgsqlParameter("@id", id));
            LoadClients();
        }

        BindingSource bindingSource = new BindingSource();
        DataTable dataTable = new DataTable();

        // Поиск клиента по ФИО или телефону (через BindingSource.Filter, НЕ SQL).
        // ВНИМАНИЕ: BindingSource.Filter — это НЕ SQL-запрос, а фильтр DataView над локальным DataTable.
        // Спецсимволы LIKE ([, %, _) экранируются для корректного поиска в DataView.
        // SQL-инъекция здесь невозможна — фильтр не уходит на сервер.
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
                bindingSource.RemoveFilter();
                return;
            }

            string filter = textBoxSearch.Text
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("_", "[_]");

            bindingSource.Filter =
                $"[ФИО] LIKE '%{filter}%' OR [Телефон] LIKE '%{filter}%'";
        }
    }
}
