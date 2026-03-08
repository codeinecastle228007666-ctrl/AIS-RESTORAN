using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using _1.data;

namespace _1.forms.bronirovanie
{
    public partial class BronirovanieForm : Form
    {
        public BronirovanieForm()
        {
            InitializeComponent();
            LoadClients();
            LoadTables();
            LoadStatus();
            LoadBron();
        }


        void LoadClients()
        {
            string sql = "SELECT client_id, fio FROM client";
            DataTable dt = Db.GetData(sql);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "fio";
            comboBox1.ValueMember = "client_id";
        }

        void LoadTables()
        {
            string sql = "SELECT stol_id, nomer FROM stol";
            DataTable dt = Db.GetData(sql);
            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "nomer";
            comboBox2.ValueMember = "stol_id";
        }

        void LoadStatus()
        {
            string sql = "SELECT status_broni_id, nazvanie FROM status_broni";
            DataTable dt = Db.GetData(sql);
            comboBox3.DataSource = dt;
            comboBox3.DisplayMember = "nazvanie";
            comboBox3.ValueMember = "status_broni_id";
        }

        void LoadBron()
        {
            string sql = @"
    SELECT 
        b.bronirovanie_id AS ""ID"",
        c.fio AS ""Клиент"",
        s.nomer AS ""Номер стола"",
        b.data_broni AS ""Дата брони"",
        b.kolvo_gostei AS ""Кол-во гостей"",
        st.nazvanie AS ""Статус""
    FROM bronirovanie b
    JOIN client c ON b.client_id = c.client_id
    JOIN stol s ON b.stol_id = s.stol_id
    JOIN status_broni st ON b.status_broni_id = st.status_broni_id
    ORDER BY b.data_broni";
            dataGridView1.DataSource = Db.GetData(sql);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            int client = Convert.ToInt32(comboBox1.SelectedValue);
            int stol = Convert.ToInt32(comboBox2.SelectedValue);
            DateTime date = dateTimePicker1.Value;
            int guests = Convert.ToInt32(numericUpDown1.Value);
            int status = Convert.ToInt32(comboBox3.SelectedValue);

            int capacity = gettablecapacity(stol);

            if (guests > capacity)
            {
                MessageBox.Show($"Этот стол рассчитан максимум на {capacity} гостей.");
                return;
            }

            if (TableIsBusy(stol, date))
            {
                MessageBox.Show("Этот стол уже забронирован на эту дату.");
                return;
            }

            string sql = @"
        INSERT INTO bronirovanie
        (client_id, stol_id, data_broni, kolvo_gostei, status_broni_id)
        VALUES
        (@c, @s, @d, @g, @st)";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@c", client),
                new NpgsqlParameter("@s", stol),
                new NpgsqlParameter("@d", date),
                new NpgsqlParameter("@g", guests),
                new NpgsqlParameter("@st", status)
            };

            Db.ekzekuttranzakcii(sql, parameters);

            LoadBron();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);

            if (MessageBox.Show("Удалить бронь?", "Подтверждение",
             MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            string sql = "DELETE FROM bronirovanie WHERE bronirovanie_id = @id";

            Db.ekzekuttranzakcii(sql, new NpgsqlParameter("@id", id));

            LoadBron();
        }


        bool TableIsBusy(int stol, DateTime date)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM bronirovanie
        WHERE stol_id = @stol AND data_broni = @date
        AND status_broni_id<>3";

            using (var con = Db.GetConnection())
            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@stol", stol);
                cmd.Parameters.AddWithValue("@date", date);

                con.Open();

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count > 0;
            }
        }


        int gettablecapacity(int stol)
        {
            string sql = "SELECT kolvo_mest FROM stol WHERE stol_id = @id";
            using (var con = Db.GetConnection())
            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", stol);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void BronirovanieForm_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd.MM.yyyy HH:mm";
            dateTimePicker1.ShowUpDown = true;
        }

        private void buttonSelectTable_Click(object sender, EventArgs e)
        {
            ZalForm zal = new ZalForm(dateTimePicker1.Value);
            if (zal.ShowDialog() == DialogResult.OK)
            {
                comboBox2.SelectedValue = zal.SelectedTableId;
            }
        }
    }
}
