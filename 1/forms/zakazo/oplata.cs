using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using _1.data;
using Npgsql;
using QRCoder;

namespace _1.forms
{
    // Форма оплаты заказа. Показывает сумму, способ оплаты, генерирует чек и QR-код.
    public partial class oplata : Form
    {
        private int _zakazId;

        public oplata(int zakazId)
        {
            InitializeComponent();
            _zakazId = zakazId; // запоминаем ID заказа, который будем оплачивать
        }

        // При загрузке проверяем, не оплачен ли уже заказ, и показываем данные
        private void oplata_Load(object sender, EventArgs e)
        {
            // Проверяем, есть ли уже оплата в таблице oplata для этого заказа
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

                // Если оплата уже была — закрываем форму
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

            // Загружаем способы оплаты из БД (наличные, карта, QR и т.д.)
            string sql = "SELECT sposob_oplati_id, nazvanie FROM sposob_oplati";
            comboBox1.DataSource = Db.GetData(sql);
            comboBox1.DisplayMember = "nazvanie";
            comboBox1.ValueMember = "sposob_oplati_id";

            LoadSumma();          // показываем общую сумму заказа
            ShowQrIfNeeded();     // если выбран QR-способ — генерируем код
        }

        // Загружаем сумму: складываем количество * цену по всем позициям заказа
        private void LoadSumma()
        {
            string sql = "SELECT SUM(kolichestvo * cena) FROM sostav_zakaza WHERE zakaz_id = @zakaz";
            var table = Db.GetData(sql, new NpgsqlParameter("@zakaz", _zakazId));

            if (table.Rows.Count > 0 && table.Rows[0][0] != DBNull.Value)
                textBox1.Text = table.Rows[0][0].ToString();
            else
                textBox1.Text = "0";
        }

        // Когда меняем способ оплаты — проверяем, надо ли показать QR-код
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowQrIfNeeded();
        }

        // Если выбран способ оплаты с QR (СБП, QR-код), то генерируем и показываем QR-код
        private void ShowQrIfNeeded()
        {
            if (comboBox1.SelectedItem is DataRowView drv &&
                comboBox1.SelectedValue != null &&
                int.TryParse(comboBox1.SelectedValue.ToString(), out int _))
            {
                string name = drv["nazvanie"].ToString();
                // Проверяем, содержит ли название способа оплаты слова QR, код или СБП
                bool isQr = name.IndexOf("QR", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            name.IndexOf("код", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            name.IndexOf("СБП", StringComparison.OrdinalIgnoreCase) >= 0;

                if (isQr && decimal.TryParse(textBox1.Text, out decimal summa) && summa > 0)
                {
                    // Формируем ссылку для QR-кода (в реальном проекте тут был бы URL платёжного шлюза)
                    string qrData = $"RestaurantPayment|Order:{_zakazId}|Amount:{summa:F2}|http://pay.example.com/order/{_zakazId}";
                    GenerateQrCode(qrData);
                    pictureBoxQR.Visible = true;
                    return;
                }
            }
            pictureBoxQR.Visible = false;
        }

        // Генерация QR-кода через библиотеку QRCoder
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
                    old?.Dispose(); // освобождаем старую картинку, чтобы не было утечки памяти
                }
            }
        }

        // Генерация чека в txt-файл, сохранение на рабочий стол в папку cheki.
        private void GenerateReceipt(decimal summa, string sposobOplati)
        {
            try
            {
                // Папка на рабочем столе: cheki
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string dir = Path.Combine(desktop, "cheki");
                Directory.CreateDirectory(dir); // создаём, если нет
                string filePath = Path.Combine(dir, $"check_{_zakazId}_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

                // Загружаем информацию о заказе: кто клиент, какой официант, за каким столом, когда был создан
                DataTable orderInfo = Db.GetData(@"
                    SELECT c.fio AS client, s.fio AS sotrudnik, st.nomer AS stol, z.data_zakaza AS data
                    FROM zakazi z
                    JOIN client c ON c.client_id = z.client_id
                    JOIN sotrudniki s ON s.sotrudnik_id = z.sotrudnik_id
                    JOIN stol st ON st.stol_id = z.stol_id
                    WHERE z.zakaz_id = @id",
                    new NpgsqlParameter("@id", _zakazId));

                // Загружаем список блюд в заказе: название, количество, цена, сумма по позиции
                DataTable items = Db.GetData(@"
                    SELECT b.nazvanie, sz.kolichestvo, sz.cena, (sz.kolichestvo * sz.cena) AS summa
                    FROM sostav_zakaza sz
                    JOIN bludo b ON b.bludo_id = sz.bludo_id
                    WHERE sz.zakaz_id = @id",
                    new NpgsqlParameter("@id", _zakazId));

                // Формируем текстовый чек с рамками из символов псевдографики
                var sb = new StringBuilder();
                sb.AppendLine("═══════════════════════════════════════════");
                sb.AppendLine("            АИС \"РЕСТОРАН\"");
                sb.AppendLine("           КАССОВЫЙ ЧЕК");
                sb.AppendLine("═══════════════════════════════════════════");

                if (orderInfo.Rows.Count > 0)
                {
                    var row = orderInfo.Rows[0];
                    sb.AppendLine($"Заказ №: {_zakazId}");
                    sb.AppendLine($"Дата: {Convert.ToDateTime(row["data"]):dd.MM.yyyy HH:mm}");
                    sb.AppendLine($"Клиент: {row["client"]}");
                    sb.AppendLine($"Официант: {row["sotrudnik"]}");
                    sb.AppendLine($"Стол: {row["stol"]}");
                }

                sb.AppendLine("───────────────────────────────────────────");
                sb.AppendLine($"{"Блюдо",-30} {"Кол",5} {"Цена",8} {"Сумма",8}");
                sb.AppendLine("───────────────────────────────────────────");

                // Перебираем все блюда и добавляем их в чек (обрезаем длинные названия)
                foreach (DataRow item in items.Rows)
                {
                    string name = item["nazvanie"].ToString();
                    int kol = Convert.ToInt32(item["kolichestvo"]);
                    decimal price = Convert.ToDecimal(item["cena"]);
                    decimal total = Convert.ToDecimal(item["summa"]);
                    sb.AppendLine($"{Truncate(name, 28),-30} {kol,5} {price,8:F2} {total,8:F2}");
                }

                sb.AppendLine("───────────────────────────────────────────");
                sb.AppendLine($"{"ИТОГО:",-43} {summa,8:F2}");
                sb.AppendLine($"Способ оплаты: {sposobOplati}");
                sb.AppendLine("═══════════════════════════════════════════");
                sb.AppendLine($"             {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                sb.AppendLine("         Спасибо за посещение!");

                // Сохраняем файл в кодировке UTF-8 и открываем в блокноте
                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                System.Diagnostics.Process.Start("notepad.exe", filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения чека:\n" + ex.Message);
            }
        }

        // Обрезает строку до max символов, вместо последнего ставит многоточие
        private static string Truncate(string s, int max)
        {
            return s.Length <= max ? s : s[..(max - 1)] + "…";
        }

        // Кнопка «Оплатить»: проверяет данные, повторно проверяет оплату, вызывает хранимую процедуру
        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, выбран ли способ оплаты
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Выберите способ оплаты");
                return;
            }

            int sposobOplatiId = Convert.ToInt32(comboBox1.SelectedValue);
            decimal summa = Convert.ToDecimal(textBox1.Text);

            // Сумма должна быть положительной
            if (summa <= 0)
            {
                MessageBox.Show("Сумма не позволяет провести оплату");
                return;
            }

            // Спрашиваем подтверждение у пользователя
            if (MessageBox.Show($"Провести оплату заказа #{_zakazId} на сумму {summa:F2}?",
                "Подтверждение оплаты", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            // Повторная проверка — вдруг заказ уже успели оплатить в другой вкладке
            string recheckSql = "SELECT COUNT(*) FROM oplata WHERE zakaz_id = @zakaz";
            var recheckTable = Db.GetData(recheckSql, new NpgsqlParameter("@zakaz", _zakazId));
            if (recheckTable.Rows.Count > 0 && Convert.ToInt32(recheckTable.Rows[0][0]) > 0)
            {
                MessageBox.Show("Заказ уже оплачен!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            // Вызываем хранимую процедуру оплаты и обновляем статус заказа на 5 (оплачен)
            string sql = @"
                CALL sp_make_oplata(@p_zakaz_id, @p_sposob_oplati_id);
                UPDATE zakazi SET status_zakaza_id = 5 WHERE zakaz_id = @zakaz;
            ";

            try
            {
                // Запускаем всё в одной транзакции через вспомогательный метод
                Db.ekzekuttranzakcii(sql,
                    new NpgsqlParameter("@p_zakaz_id", _zakazId),
                    new NpgsqlParameter("@p_sposob_oplati_id", sposobOplatiId),
                    new NpgsqlParameter("@zakaz", _zakazId)
                );

                // После успешной оплаты генерируем чек и открываем его
                string sposobName = comboBox1.Text;
                GenerateReceipt(summa, sposobName);

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
