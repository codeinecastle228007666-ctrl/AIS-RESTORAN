using _1.data;
using _1.forms;
using _1.forms.zakazo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace _1
{
    public partial class Zakazi : Form
    {
        public Zakazi()
        {
            InitializeComponent();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            redzakazaform redForm = new redzakazaform();
            if (redForm.ShowDialog() == DialogResult.OK)
            {
                LoadZakazi(textBoxSearch.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e) { }

        private void Zakazi_Load(object sender, EventArgs e)
        {
            LoadZakazi("");
        }

        private void LoadZakazi(string search)
        {
            string sql = @"
                SELECT 
                    z.zakaz_id AS ""ID"",
                    z.status_zakaza_id AS ""StatusID"",
                    c.fio AS ""Клиент"",
                    s.fio AS ""Сотрудник"",
                    z.data_zakaza AS ""Дата заказа"",
                    sz.nazvanie AS ""Статус""
                    FROM zakazi z
                    JOIN client c ON c.client_id = z.client_id
                    JOIN sotrudniki s ON s.sotrudnik_id = z.sotrudnik_id
                    JOIN status_zakaza sz ON sz.status_zakaza_id = z.status_zakaza_id
            ";

            if (!string.IsNullOrWhiteSpace(search))
            {
                sql += @"
                    WHERE LOWER(c.fio) LIKE @search
                    OR LOWER(s.fio) LIKE @search
                    OR z.zakaz_id::text LIKE @search
                ";
            }

            sql += " ORDER BY z.data_zakaza DESC";

            if (!string.IsNullOrWhiteSpace(search))
            {
                dataGridView1.DataSource = Db.GetData(sql, new Npgsql.NpgsqlParameter("@search", $"%{search.ToLower()}%"));
            }
            else
            {
                dataGridView1.DataSource = Db.GetData(sql);
            }
            dataGridView1.Columns["StatusID"].Visible = false;
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Selected = true;
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            LoadZakazi(textBoxSearch.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            int zakazId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            oplata oplataForm = new oplata(zakazId);

            if (oplataForm.ShowDialog() == DialogResult.OK)
            {
                LoadZakazi(textBoxSearch.Text);
            }
        }

        private void button5_click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            int zakazId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            zakazi_itemsForm sostavForm = new zakazi_itemsForm(zakazId);
            sostavForm.ShowDialog();
        }

        private bool _loadingstatuses = false;
        private int _currentStatusId;

        private void LoadAvailableStatuses(int currentStatusId)
        {
            _loadingstatuses = true;
            _currentStatusId = currentStatusId;

            string sql = "SELECT status_zakaza_id, nazvanie FROM status_zakaza";
            var table = Db.GetData(sql);

            var allowed = table.AsEnumerable()
                .Where(row => IsTransitionAllowed(currentStatusId,
                    row.Field<int>("status_zakaza_id")));

            if (allowed.Any())
            {
                comboBox1.DataSource = allowed.CopyToDataTable();
                comboBox1.DisplayMember = "nazvanie";
                comboBox1.ValueMember = "status_zakaza_id";
                comboBox1.Enabled = true;
            }
            else
            {
                comboBox1.DataSource = null;
                comboBox1.Enabled = false;
            }

            button6.Enabled = false;
            _loadingstatuses = false;
        }

        private bool IsTransitionAllowed(int oldStatus, int newStatus)
        {
            if (oldStatus == newStatus) return false;

            return
                (oldStatus == 1 && (newStatus == 2 || newStatus == 7)) ||
                (oldStatus == 2 && (newStatus == 3 || newStatus == 7)) ||
                (oldStatus == 3 && newStatus == 4) ||
                (oldStatus == 4 && newStatus == 5) ||
                (oldStatus == 5 && newStatus == 6);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            int status = Convert.ToInt32(dataGridView1.CurrentRow.Cells["StatusID"].Value);
            LoadAvailableStatuses(status);
            UpdateUIByStatus(status);
        }

        private void combobox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingstatuses) return;
            int selectedStatusId = Convert.ToInt32(comboBox1.SelectedValue);
            button6.Enabled = selectedStatusId != _currentStatusId;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int zakazId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            int newStatus = Convert.ToInt32(comboBox1.SelectedValue);

            string sql = "UPDATE zakazi SET status_zakaza_id = @status WHERE zakaz_id = @id";

            try
            {
                Db.ekzekuttranzakcii(sql,
                    new Npgsql.NpgsqlParameter("@status", newStatus),
                    new Npgsql.NpgsqlParameter("@id", zakazId)
                );

                LoadZakazi(textBoxSearch.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name != "Статус") return;

            var cellStatus = dataGridView1.Rows[e.RowIndex].Cells["StatusID"].Value;
            if (cellStatus == null || !int.TryParse(cellStatus.ToString(), out int status)) return;

            Color back = status switch
            {
                1 => Color.LightGray,
                2 => Color.LightBlue,
                3 => Color.Orange,
                4 => Color.LightGreen,
                5 => Color.Green,
                6 => Color.DarkGreen,
                7 => Color.Red,
                _ => dataGridView1.DefaultCellStyle.BackColor
            };

            e.CellStyle.BackColor = back;
            e.CellStyle.SelectionBackColor = back;
        }

        private void UpdateUIByStatus(int statusId)
        {
            bool isFinal = statusId == 6 || statusId == 7;
            comboBox1.Enabled = !isFinal;
            button6.Enabled = false;
            button4.Enabled = statusId == 4;
            button5.Enabled = !isFinal;
        }
    }
}
