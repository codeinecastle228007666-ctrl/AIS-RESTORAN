// Визуальный план расположения столов (схема зала)
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
    // Схема зала: визуальный выбор, проверка занятости столов, бронь и заказы.
    public partial class ZalForm : Form
    {
        // ID выбранного стола.
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

        // Отрисовка кнопок-столов на панели.
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

                // Красный — занят, зелёный — свободен
                if (TableIsBooked(stolId))
                    tableButton.BackColor = Color.Red;
                else
                    tableButton.BackColor = Color.LightGreen;

                tableButton.Tag = stolId;
                tableButton.Click += TableButton_Click;
                panel1.Controls.Add(tableButton);
                x += 100;

                // Перенос строки после 5 кнопок
                if (x > 400)
                {
                    x = 20;
                    y += 80;
                }
            }
        }

        // Выбор стола по клику на кнопку.
        private void TableButton_Click(object sender, EventArgs e)
        {
            if (sender is not Button clickedButton) return;
            SelectedTableId = Convert.ToInt32(clickedButton.Tag);
            DialogResult = DialogResult.OK;
            Close();
        }

        // Проверка, забронирован ли стол на выбранное время (окно 2 часа, учитывает брони и заказы).
        bool TableIsBooked(int stolId)
        {
            DateTime end = selectedDate.AddHours(2);

            using (var con = Db.GetConnection())
            {
                con.Open();

                // Проверка броней
                string sqlBron = @"
                    SELECT COUNT(*)
                    FROM bronirovanie
                    WHERE stol_id = @stolId
                    AND status_broni_id NOT IN (3, 4, 5)
                    AND (
                          data_broni < @end
                          AND data_broni + interval '2 hour' > @start
                        )
                ";
                using (var cmd = new NpgsqlCommand(sqlBron, con))
                {
                    cmd.Parameters.AddWithValue("@stolId", stolId);
                    cmd.Parameters.AddWithValue("@start", selectedDate);
                    cmd.Parameters.AddWithValue("@end", end);
                    try
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count > 0) return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка проверки бронирования:\n" + ex.Message);
                        return false;
                    }
                }

                // Проверка активных заказов на тот же стол
                string sqlZakaz = @"
                    SELECT COUNT(*)
                    FROM zakazi
                    WHERE stol_id = @stolId
                    AND status_zakaza_id NOT IN (6, 7)
                    AND data_zakaza < @end
                    AND data_zakaza + interval '2 hour' > @start
                ";
                using (var cmd2 = new NpgsqlCommand(sqlZakaz, con))
                {
                    cmd2.Parameters.AddWithValue("@stolId", stolId);
                    cmd2.Parameters.AddWithValue("@start", selectedDate);
                    cmd2.Parameters.AddWithValue("@end", end);
                    try
                    {
                        int count = Convert.ToInt32(cmd2.ExecuteScalar());
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
}
