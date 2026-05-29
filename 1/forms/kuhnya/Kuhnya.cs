// Форма мониторинга заказов для кухни (с автообновлением и звуковым оповещением)
using _1.data;
using System;
using System.Data;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace _1.forms
{
    // Форма кухни: отображает активные заказы в реальном времени. Автообновление каждые 5 секунд, звуковой сигнал при новых заказах.
    // Статусы: 2-Принят, 3-Готовится, 4-Готов
    public partial class Kuhnya : Form
    {
        private System.Windows.Forms.Timer _timer; // Таймер автообновления
        private int _lastOrderCount;

        public Kuhnya()
        {
            InitializeComponent();

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 5000; // 5 секунд
            _timer.Tick += Timer_Tick;
        }

        private void Kuhnya_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            LoadOrders();
            _timer.Start();
        }

        private void Kuhnya_FormClosing(object sender, FormClosingEventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
        }

        // Срабатывает каждые 5 секунд. Если появились новые заказы — звуковой сигнал и мигание.
        private void Timer_Tick(object sender, EventArgs e)
        {
            int currentCount = GetCurrentOrderCount();

            if (currentCount > _lastOrderCount && _lastOrderCount >= 0)
            {
                SystemSounds.Beep.Play();
                FlashForm();
            }

            _lastOrderCount = currentCount;
            LoadOrders();
        }

        // Возвращает количество активных заказов (статусы 2, 3, 4).
        private int GetCurrentOrderCount()
        {
            string sql = @"
                SELECT COUNT(*)
                FROM zakazi
                WHERE status_zakaza_id IN (2, 3, 4)
            ";
            var dt = Db.GetData(sql);
            if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                return Convert.ToInt32(dt.Rows[0][0]);
            return 0;
        }

        // Мигание формы жёлтым цветом при появлении нового заказа.
        private void FlashForm()
        {
            this.BackColor = Color.Yellow;
            System.Windows.Forms.Timer flashTimer = new System.Windows.Forms.Timer();
            flashTimer.Interval = 300;
            int flashes = 0;

            flashTimer.Tick += (s, e) =>
            {
                flashes++;
                this.BackColor = flashes % 2 == 0 ? Color.LightYellow : Color.Yellow;
                if (flashes >= 6)
                {
                    flashTimer.Stop();
                    flashTimer.Dispose();
                    this.BackColor = SystemColors.Control;
                }
            };
            flashTimer.Start();
        }

        // Загрузка активных заказов (статусы: Новый, Готовится, Готов).
        private void LoadOrders()
        {
            string sql = @"
                SELECT
                    z.zakaz_id AS ""ID"",
                    z.data_zakaza AS ""Время"",
                    c.fio AS ""Клиент"",
                    st.nomer AS ""Стол"",
                    sz.nazvanie AS ""Статус"",
                    z.status_zakaza_id AS ""StatusID""
                FROM zakazi z
                JOIN client c ON c.client_id = z.client_id
                JOIN stol st ON st.stol_id = z.stol_id
                JOIN status_zakaza sz ON sz.status_zakaza_id = z.status_zakaza_id
                WHERE z.status_zakaza_id IN (2, 3, 4)
                ORDER BY z.data_zakaza ASC
            ";

            dataGridView1.DataSource = Db.GetData(sql);

            if (dataGridView1.Columns["StatusID"] != null)
                dataGridView1.Columns["StatusID"].Visible = false;
            if (dataGridView1.Columns["ID"] != null)
                dataGridView1.Columns["ID"].Visible = false;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.CellFormatting -= dataGridView1_CellFormatting;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;

            UpdateOrderCount();
            UpdateButtonState();
        }

        private void UpdateOrderCount()
        {
            _lastOrderCount = dataGridView1.Rows.Count;
            labelOrderCount.Text = $"Активных заказов: {_lastOrderCount}";
        }

        // Обновление текста и состояния кнопки в зависимости от статуса заказа.
        private void UpdateButtonState()
        {
            if (dataGridView1.CurrentRow == null)
            {
                buttonMarkReady.Enabled = false;
                return;
            }

            int statusId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["StatusID"].Value);

            if (statusId == 4)
            {
                buttonMarkReady.Enabled = false;
                buttonMarkReady.Text = "Заказ уже готов";
            }
            else if (statusId == 2)
            {
                buttonMarkReady.Enabled = true;
                buttonMarkReady.Text = "Начать готовить";
            }
            else if (statusId == 3)
            {
                buttonMarkReady.Enabled = true;
                buttonMarkReady.Text = "Готов к выдаче";
            }
            else
            {
                buttonMarkReady.Enabled = false;
            }
        }

        // Цветовая маркировка заказов: жёлтый — новый, голубой — готовится, зелёный — готов.
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name != "Статус") return;

            var cellStatus = dataGridView1.Rows[e.RowIndex].Cells["StatusID"]?.Value;
            if (cellStatus == null || !int.TryParse(cellStatus.ToString(), out int status)) return;

            Color back = status switch
            {
                2 => Color.LightYellow,
                3 => Color.LightBlue,
                4 => Color.LightGreen,
                _ => dataGridView1.DefaultCellStyle.BackColor
            };

            e.CellStyle.BackColor = back;
            e.CellStyle.SelectionBackColor = back;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonState();
        }

        // Смена статуса заказа: Новый → Готовится → Готов.
        // SQL UPDATE через NpgsqlParameter — защита от инъекций.
        private void buttonMarkReady_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите заказ");
                return;
            }

            int zakazId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            int statusId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["StatusID"].Value);

            int newStatus;
            string actionText;

            if (statusId == 2)
            {
                newStatus = 3;
                actionText = "Готовится";
            }
            else if (statusId == 3)
            {
                newStatus = 4;
                actionText = "Готов к выдаче (заказ скроется из списка)";
            }
            else
            {
                MessageBox.Show("Этот заказ уже готов к выдаче");
                return;
            }

            // Проверка допустимости перехода по общим правилам
            if (!IsKitchenTransitionAllowed(statusId, newStatus))
            {
                MessageBox.Show("Недопустимый переход статуса");
                return;
            }

            if (MessageBox.Show($"Изменить статус заказа #{zakazId} на \"{actionText}\"?",
                "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            string sql = "UPDATE zakazi SET status_zakaza_id = @status WHERE zakaz_id = @id";

            try
            {
                Db.ekzekuttranzakcii(sql,
                    new Npgsql.NpgsqlParameter("@status", newStatus),
                    new Npgsql.NpgsqlParameter("@id", zakazId)
                );

                MessageBox.Show($"Заказ #{zakazId} — {actionText}");
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка изменения статуса:\n" + ex.Message);
            }
        }

        // Общие правила переходов статусов (синхронизировано с Zakazi.IsTransitionAllowed).
        private bool IsKitchenTransitionAllowed(int oldStatus, int newStatus)
        {
            if (oldStatus == newStatus) return false;
            return
                (oldStatus == 2 && (newStatus == 3 || newStatus == 7)) ||
                (oldStatus == 3 && newStatus == 4);
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadOrders();
        }
    }
}
