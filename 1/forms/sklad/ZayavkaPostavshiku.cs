using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using _1.data;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace _1.forms.sklad
{
    // Форма создания заявки поставщику. Генерирует Excel-файл с таблицей продуктов.
    public partial class ZayavkaPostavshiku : Form
    {
        // Папка на рабочем столе, куда сохраняются все заявки
        private static readonly string OutputDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "заявки");
        // Таблица с продуктами из БД (ID, название, цена, единица измерения)
        private DataTable _productsTable;

        public ZayavkaPostavshiku()
        {
            InitializeComponent();
        }

        // При загрузке формы загружаем все справочники и настраиваем таблицу
        private void ZayavkaPostavshiku_Load(object sender, EventArgs e)
        {
            LoadPostavshiki();          // список поставщиков
            LoadProducts();             // список продуктов
            SetupItemsGrid();           // настройка колонок таблицы товаров
            LoadCompanyInfo();          // информация о ресторане (от кого)
            LoadZayavkiList();          // список уже созданных заявок
            textBoxDate.Text = DateTime.Now.ToString("dd.MM.yyyy");
            textBoxNomer.Text = DateTime.Now.ToString("yyyyMMddHHmmss"); // номер = дата+время
        }

        // Загружаем название, адрес и телефон ресторана из таблицы restoran
        private void LoadCompanyInfo()
        {
            string sql = "SELECT nazvanie, adress, nomer_telefona FROM restoran ORDER BY restoran_id LIMIT 1";
            DataTable dt = Db.GetData(sql);
            if (dt.Rows.Count > 0)
            {
                string name = dt.Rows[0]["nazvanie"].ToString();
                string addr = dt.Rows[0]["adress"].ToString();
                string phone = dt.Rows[0]["nomer_telefona"]?.ToString() ?? "";
                textBoxOtKogo.Text = $"{name}, {addr}, т. {phone}";
            }
        }

        private DataTable _postavshikiTable;

        // Загрузка списка поставщиков в выпадающий список
        private void LoadPostavshiki()
        {
            string sql = "SELECT postavschik_id, nazvanie, adres FROM postavschik ORDER BY nazvanie";
            _postavshikiTable = Db.GetData(sql);
            comboBoxPostavshik.DataSource = _postavshikiTable;
            comboBoxPostavshik.DisplayMember = "nazvanie";
            comboBoxPostavshik.ValueMember = "postavschik_id";
            comboBoxPostavshik.SelectedIndex = -1;
        }

        // Когда выбрали поставщика — показываем его адрес в текстовом поле
        private void comboBoxPostavshik_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPostavshik.SelectedIndex >= 0)
            {
                DataRowView row = comboBoxPostavshik.SelectedItem as DataRowView;
                textBoxAdres.Text = row?["adres"]?.ToString() ?? "";
            }
            else
            {
                textBoxAdres.Text = "";
            }
        }

        // Загрузка всех продуктов с их единицами измерения
        private void LoadProducts()
        {
            string sql = @"
                SELECT p.product_id, p.nazvanie, p.cena, e.nazvanie AS edinica
                FROM product p
                JOIN edinica_izmereniya e ON p.edinica_izmereniya_id = e.edinica_izmereniya_id
                ORDER BY p.nazvanie";
            _productsTable = Db.GetData(sql);
        }

        // Настройка колонок таблицы для ввода продуктов в заявку
        private void SetupItemsGrid()
        {
            dataGridViewItems.Columns.Clear();

            // Колонка с выпадающим списком продуктов (ComboBox)
            DataGridViewComboBoxColumn colProduct = new DataGridViewComboBoxColumn();
            colProduct.HeaderText = "Продукт";
            colProduct.Name = "product";
            colProduct.DataSource = _productsTable;
            colProduct.DisplayMember = "nazvanie";
            colProduct.ValueMember = "product_id";
            colProduct.Width = 180;
            dataGridViewItems.Columns.Add(colProduct);

            // Единица измерения (заполняется автоматически)
            DataGridViewTextBoxColumn colUnit = new DataGridViewTextBoxColumn();
            colUnit.HeaderText = "Ед. изм.";
            colUnit.Name = "unit";
            colUnit.ReadOnly = true;
            colUnit.Width = 60;
            dataGridViewItems.Columns.Add(colUnit);

            // Количество товара (вводит пользователь)
            DataGridViewTextBoxColumn colQty = new DataGridViewTextBoxColumn();
            colQty.HeaderText = "Количество";
            colQty.Name = "qty";
            colQty.Width = 80;
            dataGridViewItems.Columns.Add(colQty);

            // Цена за единицу (берётся из БД, не редактируется)
            DataGridViewTextBoxColumn colPrice = new DataGridViewTextBoxColumn();
            colPrice.HeaderText = "Цена";
            colPrice.Name = "price";
            colPrice.ReadOnly = true;
            colPrice.Width = 70;
            dataGridViewItems.Columns.Add(colPrice);

            // Сумма = количество * цена, вычисляется автоматически
            DataGridViewTextBoxColumn colTotal = new DataGridViewTextBoxColumn();
            colTotal.HeaderText = "Сумма";
            colTotal.Name = "total";
            colTotal.ReadOnly = true;
            colTotal.Width = 80;
            dataGridViewItems.Columns.Add(colTotal);

            // Подписываемся на события изменения ячеек
            dataGridViewItems.CellValueChanged += DataGridViewItems_CellValueChanged;
            dataGridViewItems.EditingControlShowing += DataGridViewItems_EditingControlShowing;
        }

        // Когда пользователь меняет продукт или количество — пересчитываем цену и сумму
        private void DataGridViewItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewItems.Rows[e.RowIndex];
            if (row.IsNewRow) return;

            // Если изменили продукт — подставляем его цену и единицу измерения
            if (e.ColumnIndex == dataGridViewItems.Columns["product"].Index)
            {
                if (row.Cells["product"].Value != null &&
                    int.TryParse(row.Cells["product"].Value.ToString(), out int prodId))
                {
                    DataRow[] found = _productsTable.Select($"product_id = {prodId}");
                    if (found.Length > 0)
                    {
                        row.Cells["price"].Value = found[0]["cena"];
                        row.Cells["unit"].Value = found[0]["edinica"];
                    }
                }
                RecalcRow(row);
            }
            // Если изменили количество — просто пересчитываем сумму
            else if (e.ColumnIndex == dataGridViewItems.Columns["qty"].Index)
            {
                RecalcRow(row);
            }
        }

        // Перехватываем ввод в ячейку количества, чтобы разрешить только цифры и запятую
        private void DataGridViewItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridViewItems.CurrentCell.ColumnIndex == dataGridViewItems.Columns["qty"].Index
                && e.Control is DataGridViewTextBoxEditingControl tb)
            {
                tb.KeyPress -= QtyCell_KeyPress;
                tb.KeyPress += QtyCell_KeyPress;
            }
        }

        private void QtyCell_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
                && e.KeyChar != ',' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        // Пересчёт суммы в строке: цена * количество
        private void RecalcRow(DataGridViewRow row)
        {
            if (row.Cells["price"].Value != null && row.Cells["qty"].Value != null)
            {
                // Заменяем точку на запятую, чтобы корректно распарсить русскую культуру
                string qtyText = row.Cells["qty"].Value.ToString().Replace('.', ',');
                decimal price = Convert.ToDecimal(row.Cells["price"].Value);
                if (decimal.TryParse(qtyText, out decimal qty))
                {
                    row.Cells["total"].Value = price * qty;
                }
            }
        }

        // Добавить пустую строку в таблицу товаров
        private void buttonAddRow_Click(object sender, EventArgs e)
        {
            dataGridViewItems.Rows.Add();
        }

        // Удалить выбранную строку из таблицы (кроме новой пустой)
        private void buttonRemoveRow_Click(object sender, EventArgs e)
        {
            if (dataGridViewItems.CurrentRow != null && !dataGridViewItems.CurrentRow.IsNewRow)
                dataGridViewItems.Rows.RemoveAt(dataGridViewItems.CurrentRow.Index);
        }

        // Создание заявки: сбор данных из формы и вызов GenerateExcel
        private void buttonCreate_Click(object sender, EventArgs e)
        {
            // Проверяем, что поставщик выбран
            if (comboBoxPostavshik.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите поставщика");
                return;
            }

            // Проверяем, есть ли хотя бы одна строка с товаром
            bool hasData = false;
            foreach (DataGridViewRow row in dataGridViewItems.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["product"].Value != null)
                {
                    hasData = true;
                    break;
                }
            }

            if (!hasData)
            {
                MessageBox.Show("Добавьте хотя бы одну позицию в таблицу");
                return;
            }

            // Собираем все данные из формы
            string supplier = comboBoxPostavshik.Text;
            string nomer = textBoxNomer.Text;
            string date = textBoxDate.Text;
            string otKogo = textBoxOtKogo.Text;
            string bodyText = textBoxBody.Text;

            // Создаём таблицу с колонками для Excel
            DataTable itemsDt = new DataTable();
            itemsDt.Columns.Add("№", typeof(int));
            itemsDt.Columns.Add("Наименование");
            itemsDt.Columns.Add("Ед.изм");
            itemsDt.Columns.Add("Количество", typeof(decimal));
            itemsDt.Columns.Add("Цена", typeof(decimal));
            itemsDt.Columns.Add("Сумма", typeof(decimal));

            decimal totalQty = 0;
            decimal totalSum = 0;
            int num = 1;

            // Проходим по всем строкам таблицы и собираем данные
            foreach (DataGridViewRow row in dataGridViewItems.Rows)
            {
                if (row.IsNewRow || row.Cells["product"].Value == null) continue;

                int prodId = Convert.ToInt32(row.Cells["product"].Value);
                DataRow[] found = _productsTable.Select($"product_id = {prodId}");
                if (found.Length == 0) continue;

                string name = found[0]["nazvanie"].ToString();
                string unit = found[0]["edinica"].ToString();
                string qtyText = row.Cells["qty"].Value?.ToString().Replace('.', ',') ?? "0";
                decimal.TryParse(qtyText, out decimal qty);
                decimal price = Convert.ToDecimal(row.Cells["price"].Value ?? 0);
                decimal total = price * qty;

                itemsDt.Rows.Add(num++, name, unit, qty, price, total);
                totalQty += qty;
                totalSum += total;
            }

            if (itemsDt.Rows.Count == 0)
            {
                MessageBox.Show("Нет корректных строк для формирования заявки");
                return;
            }

            // Генерируем Excel-файл
            GenerateExcel(supplier, nomer, date, otKogo, bodyText, itemsDt, totalQty, totalSum);
        }

        // Формирование Excel-файла заявки через библиотеку EPPlus
        private void GenerateExcel(string supplier, string nomer, string date,
            string otKogo, string bodyText, DataTable items,
            decimal totalQty, decimal totalSum)
        {
            // Лицензия для некоммерческого использования (EPPlus требует)
            ExcelPackage.License.SetNonCommercialPersonal("Student Project");
            using (var package = new ExcelPackage())
            {
                // Создаём лист с названием "Заявка"
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("Заявка");

                // Шапка документа: кому, от кого, дата
                ws.Cells[1, 1].Value = $"Кому: {supplier}";
                ws.Cells[1, 1].Style.Font.Size = 12;
                ws.Cells[2, 1].Value = "Директору _________________________________";
                ws.Cells[4, 1].Value = $"От: {otKogo}";
                ws.Cells[6, 1].Value = date;
                ws.Cells[7, 1].Value = $"ЗАЯВКА на поставку товара № {nomer}";
                ws.Cells[7, 1].Style.Font.Bold = true;
                ws.Cells[7, 1].Style.Font.Size = 14;
                ws.Cells[7, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Текст заявки (тело) — многострочное поле, которое пользователь редактирует
                ws.Cells[9, 1].Value = bodyText;
                ws.Cells[9, 1, 9, 6].Merge = true;

                // Таблица с товарами
                int headerRow = 11;
                ws.Cells[headerRow, 1].Value = "№";
                ws.Cells[headerRow, 2].Value = "Наименование";
                ws.Cells[headerRow, 3].Value = "Ед. изм.";
                ws.Cells[headerRow, 4].Value = "Количество";
                ws.Cells[headerRow, 5].Value = "Цена за 1 ед., руб.";
                ws.Cells[headerRow, 6].Value = "Общая стоимость, руб.";

                // Оформляем шапку таблицы: жирный шрифт, центрирование, рамки
                using (var rng = ws.Cells[headerRow, 1, headerRow, 6])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    rng.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                }

                int row = headerRow + 1;

                // Заполняем строки товарами из таблицы
                foreach (DataRow dr in items.Rows)
                {
                    ws.Cells[row, 1].Value = dr["№"];
                    ws.Cells[row, 2].Value = dr["Наименование"];
                    ws.Cells[row, 3].Value = dr["Ед.изм"];
                    ws.Cells[row, 4].Value = dr["Количество"];
                    ws.Cells[row, 4].Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[row, 5].Value = dr["Цена"];
                    ws.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[row, 6].Value = dr["Сумма"];
                    ws.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";

                    using (var rng = ws.Cells[row, 1, row, 6])
                        rng.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    row++;
                }

                // Строка с итогами: общее количество и общая сумма
                int totalRow = row;
                ws.Cells[totalRow, 1].Value = "ИТОГО:";
                ws.Cells[totalRow, 1, totalRow, 3].Merge = true;
                ws.Cells[totalRow, 4].Value = totalQty;
                ws.Cells[totalRow, 4].Style.Numberformat.Format = "#,##0.000";
                ws.Cells[totalRow, 5].Value = "";
                ws.Cells[totalRow, 6].Value = totalSum;
                ws.Cells[totalRow, 6].Style.Numberformat.Format = "#,##0.00";

                using (var rng = ws.Cells[totalRow, 1, totalRow, 6])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                // Подпись директора
                row += 2;
                ws.Cells[row, 1].Value = $"Директор _________________________________";
                row++;
                ws.Cells[row, 1].Value = $"М.П.                                   (подпись)";

                // Настройка шрифта и ширины колонок
                ws.Cells[1, 1, totalRow, 6].Style.Font.Name = "Times New Roman";
                ws.Cells.AutoFitColumns();
                ws.Column(2).Width = 40;
                ws.Column(5).Width = 18;
                ws.Column(6).Width = 22;

                // Сохраняем файл в папку "заявки" на рабочем столе
                Directory.CreateDirectory(OutputDir);
                string fileName = $"Заявка_№{nomer}.xlsx";
                string filePath = Path.Combine(OutputDir, fileName);
                File.WriteAllBytes(filePath, package.GetAsByteArray());

                MessageBox.Show($"Заявка создана:\n{fileName}", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Открываем созданный файл в Excel
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });

                // Очищаем форму для следующей заявки
                dataGridViewItems.Rows.Clear();
                comboBoxPostavshik.SelectedIndex = -1;
                textBoxNomer.Text = DateTime.Now.ToString("yyyyMMddHHmmss");
                LoadZayavkiList();
            }
        }

        // Загрузка списка уже созданных заявок из папки
        private void LoadZayavkiList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("№", typeof(int));
            dt.Columns.Add("Дата");
            dt.Columns.Add("Файл");

            // Если папки нет — показываем пустой список
            if (!Directory.Exists(OutputDir))
            {
                dataGridViewZayavki.DataSource = dt;
                dataGridViewZayavki.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                return;
            }

            // Сканируем все файлы в папке, сортируем от новых к старым
            int num = 1;
            foreach (string file in Directory.GetFiles(OutputDir, "Заявка_№*.xlsx")
                .OrderByDescending(f => f))
            {
                FileInfo fi = new FileInfo(file);
                DataRow dr = dt.NewRow();
                dr["№"] = num++;
                dr["Дата"] = fi.CreationTime.ToString("dd.MM.yyyy HH:mm");
                dr["Файл"] = fi.Name;
                dt.Rows.Add(dr);
            }

            dataGridViewZayavki.DataSource = dt;
            dataGridViewZayavki.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Открыть выбранную заявку в Excel
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (dataGridViewZayavki.CurrentRow == null)
            {
                MessageBox.Show("Выберите строку");
                return;
            }

            string fileName = dataGridViewZayavki.CurrentRow.Cells["Файл"].Value?.ToString();
            if (string.IsNullOrEmpty(fileName)) return;

            string filePath = Path.Combine(OutputDir, fileName);
            if (File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("Файл не найден");
            }
        }

        // Удалить выбранную заявку (файл с диска)
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewZayavki.CurrentRow == null)
            {
                MessageBox.Show("Выберите строку");
                return;
            }

            string fileName = dataGridViewZayavki.CurrentRow.Cells["Файл"].Value?.ToString();
            if (string.IsNullOrEmpty(fileName)) return;

            if (MessageBox.Show($"Удалить {fileName}?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            string filePath = Path.Combine(OutputDir, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

            LoadZayavkiList();
        }
    }
}
