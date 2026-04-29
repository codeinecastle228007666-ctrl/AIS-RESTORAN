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

namespace _1.forms.bronirovanie
{
    public partial class ZalForm : Form
    {
        public int SelectedTableId = -1;
        public ZalForm()
        {
            InitializeComponent();
            LoadTables();
        }

        DateTime selectedDate;
        public ZalForm(DateTime date)
        {
            InitializeComponent();
            selectedDate = date;
            LoadTables();
        }
        void LoadTables()
        {
            string sql = "SELECT stol_id, nomer FROM stol ORDER BY nomer";
            DataTable tables = Db.GetData(sql);

            panel1.Controls.Clear();
            int x = 20;
            int y = 20;

            foreach (DataRow row in tables.Rows)
            {
                Button tableButton = new Button();
                tableButton.Width = 80;
                tableButton.Height = 60;
                tableButton.Left = x;
                tableButton.Top = y;
                tableButton.Text = "Стол " + row["nomer"].ToString();
                tableButton.Tag = row["stol_id"];
                int stolId = Convert.ToInt32(row["stol_id"]);

                if (TableIsBooked(stolId))
                {
                    tableButton.BackColor = Color.Red;
                }
                else
                {
                    tableButton.BackColor = Color.LightGreen;
                }
                tableButton.Tag = stolId;
                tableButton.Click += TableButton_Click;
                panel1.Controls.Add(tableButton);
                x += 100;

                if (x > 400)
                {
                    x = 20;
                    y += 80;
                }

            }

        }

        private void TableButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            SelectedTableId = Convert.ToInt32(clickedButton.Tag);
            DialogResult = DialogResult.OK;
            Close();
        }


        bool TableIsBooked(int stolId)
        {
            DateTime end = selectedDate.AddHours(2);

            string sql = @"
    SELECT COUNT(*)
    FROM bronirovanie
    WHERE stol_id = @stolId
    AND status_broni_id NOT IN (3, 4, 5)
    AND (
          data_broni < @end
          AND data_broni + interval '2 hour' > @start
        )
    ";

            using (var con = Db.GetConnection())
            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@stolId", stolId);
                cmd.Parameters.AddWithValue("@start", selectedDate);
                cmd.Parameters.AddWithValue("@end", end);

                try
                {
                    con.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка проверки бронирования:\n" + ex.Message);
                    return false;
                }
            }
        }
    }

}
