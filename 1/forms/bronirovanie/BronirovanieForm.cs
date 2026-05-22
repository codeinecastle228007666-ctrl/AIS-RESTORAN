// Форма управления бронированием столов
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;
using _1.data;

namespace _1.forms.bronirovanie
{
    // Форма бронирования столов: создание, удаление, изменение статуса, проверка занятости.
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

        // Загрузка списка клиентов в ComboBox.
        void LoadClients()
        {
            string sql = "SELECT client_id, fio FROM client";
            DataTable dt = Db.GetData(sql);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "fio";
            comboBox1.ValueMember = "client_id";
        }

        // Загрузка списка столов в ComboBox.
        void LoadTables()
        {
            string sql = "SELECT stol_id, nomer FROM stol";
            DataTable dt = Db.GetData(sql);
            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "nomer";
            comboBox2.ValueMember = "stol_id";
        }

        // Загрузка статусов бронирования.
        void LoadStatus()
        {
            string sql = "SELECT status_broni_id, nazvanie FROM status_broni";
            DataTable dt = Db.GetData(sql);
            comboBox3.DataSource = dt;
            comboBox3.DisplayMember = "nazvanie";
            comboBox3.ValueMember = "status_broni_id";
        }

        // Загрузка всех бронирований в DataGridView.
        void LoadBron()
        {
            string sql = @"
                SELECT
                    b.bronirovanie_id AS ""ID"",
                    b.status_broni_id AS ""StatusID"",
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
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["StatusID"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            UpdateStatusUI();
        }

        // Создание нового бронирования с проверками вместимости и занятости.
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null || comboBox2.SelectedValue == null || comboBox3.SelectedValue == null)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            int client = Convert.ToInt32(comboBox1.SelectedValue);
            int stol = Convert.ToInt32(comboBox2.SelectedValue);
            DateTime date = dateTimePicker1.Value;
            int guests = Convert.ToInt32(numericUpDown1.Value);
            int status = 1; // Новая бронь

            try
            {
                var conn = Db.GetSessionConnection();
                using var tr = conn.BeginTransaction();

                // Проверка: достаточно ли мест на выбранном столе
                int capacity = GetTableCapacity(stol, tr);
                if (guests > capacity)
                {
                    tr.Rollback();
                    MessageBox.Show($"Этот стол рассчитан максимум на {capacity} гостей.");
                    return;
                }

                // Проверка: не занят ли стол в это же время (брони и активные заказы)
                if (TableIsBusy(stol, date, tr))
                {
                    tr.Rollback();
                    MessageBox.Show("Этот стол уже забронирован на выбранное время.");
                    return;
                }

                string sql = @"
                    INSERT INTO bronirovanie
                    (client_id, stol_id, data_broni, kolvo_gostei, status_broni_id)
                    VALUES (@c, @s, @d, @g, @st)";

                using var cmd = new NpgsqlCommand(sql, conn, tr);
                cmd.Parameters.AddWithValue("@c", client);
                cmd.Parameters.AddWithValue("@s", stol);
                cmd.Parameters.AddWithValue("@d", date);
                cmd.Parameters.AddWithValue("@g", guests);
                cmd.Parameters.AddWithValue("@st", status);
                cmd.ExecuteNonQuery();

                tr.Commit();
                MessageBox.Show("Бронирование успешно создано");
                LoadBron();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка создания бронирования:\n" + ex.Message);
            }
        }

        // Удаление выбранного бронирования.
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

        // Проверяет, занят ли стол в указанное время (конфликт с бронями и активными заказами с окном 2 часа).
        bool TableIsBusy(int stol, DateTime date, NpgsqlTransaction tr)
        {
            // Окно проверки: 2 часа
            DateTime end = date.AddHours(2);

            // Проверка броней с активными статусами (не отменена/выполнена/просрочена)
            string sql = @"
                SELECT COUNT(*)
                FROM bronirovanie
                WHERE stol_id = @stol
                AND status_broni_id NOT IN (3, 4, 5)
                AND data_broni < @end
                AND data_broni + interval '2 hour' > @start";

            using var cmd = new NpgsqlCommand(sql, tr.Connection, tr);
            cmd.Parameters.AddWithValue("@stol", stol);
            cmd.Parameters.AddWithValue("@start", date);
            cmd.Parameters.AddWithValue("@end", end);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            if (count > 0) return true;

            // Проверка активных заказов на этот стол
            string sqlZakazi = @"
                SELECT COUNT(*)
                FROM zakazi z
                WHERE z.stol_id = @stol
                AND z.status_zakaza_id NOT IN (6, 7)
                AND z.data_zakaza < @end
                AND z.data_zakaza + interval '2 hour' > @start";

            using var cmd2 = new NpgsqlCommand(sqlZakazi, tr.Connection, tr);
            cmd2.Parameters.AddWithValue("@stol", stol);
            cmd2.Parameters.AddWithValue("@start", date);
            cmd2.Parameters.AddWithValue("@end", end);

            int countZakazi = Convert.ToInt32(cmd2.ExecuteScalar());

            return countZakazi > 0;
        }

        // Возвращает вместимость стола.
        int GetTableCapacity(int stol, NpgsqlTransaction tr)
        {
            string sql = "SELECT kolvo_mest FROM stol WHERE stol_id = @id";
            using var cmd = new NpgsqlCommand(sql, tr.Connection, tr);
            cmd.Parameters.AddWithValue("@id", stol);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private void BronirovanieForm_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd.MM.yyyy HH:mm";
            dateTimePicker1.ShowUpDown = true;
        }

        // Открыть визуальный выбор стола.
        private void buttonSelectTable_Click(object sender, EventArgs e)
        {
            ZalForm zal = new ZalForm(dateTimePicker1.Value);
            if (zal.ShowDialog() == DialogResult.OK)
            {
                comboBox2.SelectedValue = zal.SelectedTableId;
            }
        }

        // Применить новый статус к выбранному бронированию.
        private void buttonApplyStatus_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int bronId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            int currentStatusId = GetCurrentStatusId(bronId);
            int newStatusId = Convert.ToInt32(comboBox3.SelectedValue);

            // Проверка допустимости перехода статуса
            if (!IsTransitionAllowed(currentStatusId, newStatusId))
            {
                MessageBox.Show("Недопустимый переход статуса.");
                return;
            }

            string sql = @"
                UPDATE bronirovanie
                SET status_broni_id = @s
                WHERE bronirovanie_id = @id";

            Db.ekzekuttranzakcii(sql,
                new NpgsqlParameter("@s", newStatusId),
                new NpgsqlParameter("@id", bronId));

            LoadBron();
        }

        // Получает текущий статус бронирования.
        private int GetCurrentStatusId(int bronId)
        {
            string sql = "SELECT status_broni_id FROM bronirovanie WHERE bronirovanie_id = @id";
            var dt = Db.GetData(sql, new Npgsql.NpgsqlParameter("@id", bronId));
            return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 0;
        }

        // Проверяет допустимость перехода статуса.
        private bool IsTransitionAllowed(int oldStatus, int newStatus)
        {
            if (oldStatus == newStatus) return false;

            // Терминальные статусы: изменение невозможно
            if (oldStatus == 3 || oldStatus == 4 || oldStatus == 5)
                return false;

            // Нельзя вернуть в статус "Новая"
            if (newStatus == 1) return false;

            // Допустимые переходы: 1->2, 1->3, 2->3, 2->4
            if (oldStatus == 1 && (newStatus == 2 || newStatus == 3)) return true;
            if (oldStatus == 2 && (newStatus == 3 || newStatus == 4)) return true;

            return false;
        }

        // Обновляет состояние UI в зависимости от текущего статуса.
        private void UpdateStatusUI()
        {
            if (dataGridView1.CurrentRow == null)
            {
                buttonApplyStatus.Enabled = false;
                comboBox3.Enabled = true;
                return;
            }

            int currentStatusId = GetCurrentStatusId(bronId: Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value));

            if (currentStatusId == 3 || currentStatusId == 4 || currentStatusId == 5)
            {
                buttonApplyStatus.Enabled = false;
                comboBox3.Enabled = false;
                buttonApplyStatus.Text = "Статус изменён нельзя";
                return;
            }

            buttonApplyStatus.Enabled = true;
            comboBox3.Enabled = true;

            if (currentStatusId == 1)
                buttonApplyStatus.Text = "Подтвердить / Отменить";
            else
                buttonApplyStatus.Text = "Выполнить / Отменить";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            UpdateStatusUI();
        }

        // Цветовая маркировка статусов бронирования.
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Статус")
            {
                string status = e.Value?.ToString();

                if (status == "Подтверждена")
                    e.CellStyle.BackColor = Color.LightGreen;
                else if (status == "Ожидает подтверждения")
                    e.CellStyle.BackColor = Color.Khaki;
                else if (status == "Отменена")
                    e.CellStyle.BackColor = Color.LightCoral;
                else if (status == "Выполнена")
                    e.CellStyle.BackColor = Color.LightBlue;
                else if (status == "Просрочена")
                    e.CellStyle.BackColor = Color.Gray;
            }
        }
    }
}
