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
    public partial class Clienti : Form
    {
        public Clienti()
        {
            InitializeComponent();
            LoadClients();
        }


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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            clientsRedForm redForm = new clientsRedForm();
            if (redForm.ShowDialog() == DialogResult.OK)
            {
                LoadClients(); //обновляем данные в таблице после добавления нового клиента
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            clientsRedForm redForm = new clientsRedForm(id);

            if (redForm.ShowDialog() == DialogResult.OK)
            {
                LoadClients(); //обновляем данные в таблице после редактирования клиента
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            if (MessageBox.Show("Вы уверены, что хотите удалить этого клиента?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            string sql = "DELETE FROM client WHERE client_id = @id";

            Db.ekzekuttranzakcii(sql, new Npgsql.NpgsqlParameter("@id", id));
            LoadClients(); //обновляем данные в таблице после удаления клиента
        }

        BindingSource bindingSource = new BindingSource();
        DataTable dataTable = new DataTable();

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
                bindingSource.RemoveFilter();
                return;
            }

            string filter = textBoxSearch.Text.Replace("'", "''");

            bindingSource.Filter =
                $"[ФИО] LIKE '%{filter}%' OR [Телефон] LIKE '%{filter}%'";

        }
    }
}
