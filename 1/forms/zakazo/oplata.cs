// Форма оплаты заказа
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using _1.data;
using Npgsql;
using QRCoder;

namespace _1.forms
{
    // Форма проведения оплаты по заказу. Проверяет, не оплачен ли уже заказ, отображает сумму, вызывает процедуру sp_make_oplata.
    // Двойная проверка оплаты (при загрузке + перед списанием) — защита от двойного списания (TOCTOU).
    // Все SQL-запросы с параметрами (NpgsqlParameter) — защита от инъекций.
    public partial class oplata : Form
    {
        private int _zakazId;
        public oplata(int zakazId)
        {
            InitializeComponent();
            _zakazId = zakazId;
        }

        private void oplata_Load(object sender, EventArgs e)
        {
            // Проверка: не оплачен ли уже заказ (@zakaz — параметр, не строка в SQL)
            string checksql = "SELECT COUNT(*) FROM oplata WHERE zakaz_id = @zakaz";
            try
            {
                var checkTable = Db.GetData(checksql,
                    new NpgsqlParameter("@zakaz", _zakazId));

                if (checkTable.Rows.Count == 0)
                {
                    MessageBox.Show("Ошибка проверки данных");
                    return;
                }

                if (Convert.ToInt32(checkTable.Rows[0][0]) > 0)
                {
                    MessageBox.Show("Заказ уже оплачен!");
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка проверки данных:\n" + ex.Message);
            }

            string sql = "SELECT sposob_oplati_id, nazvanie FROM sposob_oplati";
            comboBox1.DataSource = Db.GetData(sql);
            comboBox1.DisplayMember = "nazvanie";
            comboBox1.ValueMember = "sposob_oplati_id";

            LoadSumma();
            ShowQrIfNeeded();
        }

        // Расчёт суммы заказа (суммирование позиций в заказе).
        private void LoadSumma()
        {
            string sql = "SELECT SUM(kolichestvo * cena) FROM sostav_zakaza WHERE zakaz_id = @zakaz";
            var table = Db.GetData(sql, new NpgsqlParameter("@zakaz", _zakazId));

            if (table.Rows.Count > 0 && table.Rows[0][0] != DBNull.Value)
                textBox1.Text = table.Rows[0][0].ToString();
            else
                textBox1.Text = "0";
        }

        // Обработчик смены способа оплаты — перерисовка QR.
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowQrIfNeeded();
        }

        // Определяет, нужно ли показать QR-код (по названию способа оплаты), и генерирует/прячет его.
        private void ShowQrIfNeeded()
        {
            if (comboBox1.SelectedItem is DataRowView drv &&
                comboBox1.SelectedValue != null &&
                int.TryParse(comboBox1.SelectedValue.ToString(), out int _))
            {
                string name = drv["nazvanie"].ToString();
                bool isQr = name.IndexOf("QR", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            name.IndexOf("код", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            name.IndexOf("СБП", StringComparison.OrdinalIgnoreCase) >= 0;

                if (isQr && decimal.TryParse(textBox1.Text, out decimal summa) && summa > 0)
                {
                    string qrData = $"RestaurantPayment|Order:{_zakazId}|Amount:{summa:F2}|http://pay.example.com/order/{_zakazId}";
                    GenerateQrCode(qrData);
                    pictureBoxQR.Visible = true;
                    return;
                }
            }
            pictureBoxQR.Visible = false;
        }

        // Генерация QR-кода из переданной строки.
        private void GenerateQrCode(string data)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new QRCode(qrData))
                {
                    var bitmap = qrCode.GetGraphic(20);
                    var old = pictureBoxQR.Image;
                    pictureBoxQR.Image = bitmap;
                    old?.Dispose();
                }
            }
        }

        // Проведение оплаты: вызов процедуры и смена статуса.
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Выберите способ оплаты");
                return;
            }

            int sposobOplatiId = Convert.ToInt32(comboBox1.SelectedValue);
            decimal summa = Convert.ToDecimal(textBox1.Text);

            if (summa <= 0)
            {
                MessageBox.Show("Сумма не позволяет провести оплату");
                return;
            }

            if (MessageBox.Show($"Провести оплату заказа #{_zakazId} на сумму {summa:F2}?",
                "Подтверждение оплаты", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            // Перепроверка: не оплачен ли уже заказ (TOCTOU защита)
            string recheckSql = "SELECT COUNT(*) FROM oplata WHERE zakaz_id = @zakaz";
            var recheckTable = Db.GetData(recheckSql, new NpgsqlParameter("@zakaz", _zakazId));
            if (recheckTable.Rows.Count > 0 && Convert.ToInt32(recheckTable.Rows[0][0]) > 0)
            {
                MessageBox.Show("Заказ уже оплачен!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            // Вызов хранимой процедуры sp_make_oplata + смена статуса заказа на "Оплачен" (5).
            // Все три параметра — NpgsqlParameter (защита от инъекций, серверный парсинг типов).
            string sql = @"
                CALL sp_make_oplata(@p_zakaz_id, @p_sposob_oplati_id);
                UPDATE zakazi SET status_zakaza_id = 5 WHERE zakaz_id = @zakaz;
            ";

            try
            {
                Db.ekzekuttranzakcii(sql,
                    new NpgsqlParameter("@p_zakaz_id", _zakazId),
                    new NpgsqlParameter("@p_sposob_oplati_id", sposobOplatiId),
                    new NpgsqlParameter("@zakaz", _zakazId)
                );

                MessageBox.Show("Оплата успешно проведена!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка оплаты:\n" + ex.Message);
            }
        }
    }
}
