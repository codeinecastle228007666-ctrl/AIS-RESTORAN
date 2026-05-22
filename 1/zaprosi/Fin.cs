// Форма со всеми отчётами: 16 требуемых запросов (8 простых + 8 сложных) + 5 финансовых
using _1.data;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace _1.zaprosi
{
    // Форма со всеми запросами (21 шт: 16 из требований КП + 5 финансовых дополнений).
    public partial class Fin : Form
    {
        public Fin()
        {
            InitializeComponent();
            InitQuerySelector();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            ExcelPackage.License.SetNonCommercialPersonal("Student Project");
        }

        private void InitQuerySelector()
        {
            comboBox1.Items.Clear();
            // Простые (8)
            comboBox1.Items.Add("Список всех заказов с клиентами и статусами");
            comboBox1.Items.Add("Заказы конкретного официанта");
            comboBox1.Items.Add("Состав конкретного заказа");
            comboBox1.Items.Add("Все блюда меню с категориями");
            comboBox1.Items.Add("Клиенты и количество их заказов");
            comboBox1.Items.Add("Столы и количество заказов");
            comboBox1.Items.Add("Способы оплаты и количество использований");
            comboBox1.Items.Add("Сотрудники и их должности");
            // Сложные (8)
            comboBox1.Items.Add("Блюда с выручкой выше средней");
            comboBox1.Items.Add("Официанты со средним чеком выше среднего по ресторану");
            comboBox1.Items.Add("Клиенты, заказавшие блюда дороже среднего по меню");
            comboBox1.Items.Add("Категории блюд с количеством заказов выше среднего");
            comboBox1.Items.Add("Клиенты с количеством заказов больше среднего");
            comboBox1.Items.Add("Заказы с суммой выше среднего чека");
            comboBox1.Items.Add("Официанты, не оформившие ни одного заказа");
            comboBox1.Items.Add("Продукты, чаще всего используемые в заказах");
            // Финансовые (5)
            comboBox1.Items.Add("Итоги за период");
            comboBox1.Items.Add("Выручка по дням");
            comboBox1.Items.Add("Прибыль по дням");
            comboBox1.Items.Add("Рентабельность по дням");
            comboBox1.Items.Add("Средний чек по дням");
            comboBox1.SelectedIndex = 0;
        }

        // Popup-диалог выбора сотрудника из списка
        private int? SelectEmployee()
        {
            DataTable dt = Db.GetData("SELECT sotrudnik_id, fio FROM sotrudniki ORDER BY fio");
            if (dt.Rows.Count == 0) { MessageBox.Show("Нет сотрудников в базе.", "Ошибка"); return null; }
            using (var form = new Form { Text = "Выбор официанта", Size = new Size(350, 130), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false })
            {
                var combo = new ComboBox { Location = new Point(12, 12), Size = new Size(310, 24), DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "fio", ValueMember = "sotrudnik_id", DataSource = dt };
                var okBtn = new Button { Text = "OK", Location = new Point(12, 50), Size = new Size(80, 30), DialogResult = DialogResult.OK };
                var cancelBtn = new Button { Text = "Отмена", Location = new Point(100, 50), Size = new Size(80, 30), DialogResult = DialogResult.Cancel };
                form.Controls.Add(combo); form.Controls.Add(okBtn); form.Controls.Add(cancelBtn);
                form.AcceptButton = okBtn; form.CancelButton = cancelBtn;

                if (form.ShowDialog() == DialogResult.OK && combo.SelectedValue != null)
                    return Convert.ToInt32(combo.SelectedValue);
            }
            return null;
        }

        // Popup-диалог выбора заказа из таблицы (№, Дата, Клиент, Статус)
        private int? SelectOrder()
        {
            DataTable dt = Db.GetData(@"
                SELECT z.zakaz_id AS ""№"", z.data_zakaza AS ""Дата"", c.fio AS ""Клиент"", s.nazvanie AS ""Статус""
                FROM zakazi z
                JOIN client c ON c.client_id = z.client_id
                JOIN status_zakaza s ON s.status_zakaza_id = z.status_zakaza_id
                ORDER BY z.data_zakaza DESC");
            if (dt.Rows.Count == 0) { MessageBox.Show("Нет заказов в базе.", "Ошибка"); return null; }
            using (var form = new Form { Text = "Выбор заказа", Size = new Size(650, 400), StartPosition = FormStartPosition.CenterParent })
            {
                var dgv = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    MultiSelect = false,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    DataSource = dt
                };
                var okBtn = new Button { Text = "Выбрать", DialogResult = DialogResult.OK, Dock = DockStyle.Bottom, Size = new Size(0, 35) };
                form.Controls.Add(dgv); form.Controls.Add(okBtn);
                form.AcceptButton = okBtn;

                if (form.ShowDialog() == DialogResult.OK && dgv.SelectedRows.Count > 0)
                    return Convert.ToInt32(dgv.SelectedRows[0].Cells["№"].Value);
            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0: Q_AllOrdersWithClients(); break;
                case 1: Q_OrdersByWaiter(); break;
                case 2: Q_OrderComposition(); break;
                case 3: Q_AllDishesWithCategories(); break;
                case 4: Q_ClientsOrderCount(); break;
                case 5: Q_TablesOrderCount(); break;
                case 6: Q_PaymentMethodsUsage(); break;
                case 7: Q_EmployeesWithPositions(); break;
                case 8: Q_DishesAboveAvgRevenue(); break;
                case 9: Q_WaitersAboveAvgCheck(); break;
                case 10: Q_ClientsAboveAvgPrice(); break;
                case 11: Q_CategoriesAboveAvgOrders(); break;
                case 12: Q_ClientsAboveAvgOrderCount(); break;
                case 13: Q_OrdersAboveAvgCheck(); break;
                case 14: Q_WaitersWithoutOrders(); break;
                case 15: Q_MostUsedProducts(); break;
                case 16: Totals(); break;
                case 17: RevenueByDay(); break;
                case 18: ProfitByDay(); break;
                case 19: ProfitabilityByDay(); break;
                case 20: AverageCheckByDay(); break;
            }
            tabControl1.SelectedTab = tabPage1;
        }

        // ================ ПРОСТЫЕ ЗАПРОСЫ ================

        // 1. Список всех заказов с клиентами и статусами
        private void Q_AllOrdersWithClients()
        {
            dataGridView1.DataSource = Db.GetData(@"
                SELECT z.zakaz_id AS ""№"",
                       z.data_zakaza AS ""Дата"",
                       c.fio AS ""Клиент"",
                       s.nazvanie AS ""Статус""
                FROM zakazi z
                JOIN client c ON c.client_id = z.client_id
                JOIN status_zakaza s ON s.status_zakaza_id = z.status_zakaza_id
                WHERE z.data_zakaza BETWEEN @d1 AND @d2
                ORDER BY z.data_zakaza",
                new NpgsqlParameter("@d1", dateTimePicker1.Value),
                new NpgsqlParameter("@d2", dateTimePicker2.Value));
        }

        // 2. Заказы, оформленные конкретным официантом
        private void Q_OrdersByWaiter()
        {
            int? empId = SelectEmployee();
            if (empId == null) return;
            dataGridView1.DataSource = Db.GetData(@"
                SELECT z.zakaz_id AS ""№"",
                       z.data_zakaza AS ""Дата"",
                       c.fio AS ""Клиент""
                FROM zakazi z
                JOIN client c ON c.client_id = z.client_id
                WHERE z.sotrudnik_id = @emp
                  AND z.data_zakaza BETWEEN @d1 AND @d2
                ORDER BY z.data_zakaza",
                new NpgsqlParameter("@emp", empId.Value),
                new NpgsqlParameter("@d1", dateTimePicker1.Value),
                new NpgsqlParameter("@d2", dateTimePicker2.Value));
        }

        // 3. Состав конкретного заказа
        private void Q_OrderComposition()
        {
            int? orderId = SelectOrder();
            if (orderId == null) return;
            DataTable dt = Db.GetData(@"
                SELECT b.nazvanie AS ""Блюдо"",
                       sz.kolichestvo AS ""Кол-во"",
                       sz.cena AS ""Цена""
                FROM sostav_zakaza sz
                JOIN bludo b ON b.bludo_id = sz.bludo_id
                WHERE sz.zakaz_id = @zid",
                new NpgsqlParameter("@zid", orderId.Value));
            if (dt.Rows.Count == 0)
                MessageBox.Show($"Заказ №{orderId.Value} пуст.", "Инфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dataGridView1.DataSource = dt;
        }

        // 4. Все блюда меню с категориями
        private void Q_AllDishesWithCategories()
        {
            dataGridView1.DataSource = Db.GetData(@"
                SELECT b.nazvanie AS ""Блюдо"",
                       k.nazvanie AS ""Категория"",
                       b.cena AS ""Цена""
                FROM bludo b
                JOIN kategoriya_menu k ON k.kategoriya_id = b.kategoriya_id
                ORDER BY k.nazvanie, b.nazvanie");
        }

        // 5. Клиенты и количество их заказов
        private void Q_ClientsOrderCount()
        {
            dataGridView1.DataSource = Db.GetData(@"
                SELECT c.fio AS ""Клиент"",
                       COUNT(z.zakaz_id) AS ""Кол-во заказов""
                FROM client c
                LEFT JOIN zakazi z ON z.client_id = c.client_id
                GROUP BY c.fio
                ORDER BY 2 DESC");
        }

        // 6. Столы и количество заказов за ними
        private void Q_TablesOrderCount()
        {
            dataGridView1.DataSource = Db.GetData(@"
                SELECT s.nomer AS ""Стол"",
                       COUNT(z.zakaz_id) AS ""Кол-во заказов""
                FROM stol s
                LEFT JOIN zakazi z ON z.stol_id = s.stol_id
                GROUP BY s.nomer
                ORDER BY s.nomer");
        }

        // 7. Способы оплаты и количество использований
        private void Q_PaymentMethodsUsage()
        {
            dataGridView1.DataSource = Db.GetData(@"
                SELECT so.nazvanie AS ""Способ оплаты"",
                       COUNT(o.oplata_id) AS ""Кол-во использований""
                FROM sposob_oplati so
                LEFT JOIN oplata o ON o.sposob_oplati_id = so.sposob_oplati_id
                GROUP BY so.nazvanie
                ORDER BY 2 DESC");
        }

        // 8. Сотрудники и их должности
        private void Q_EmployeesWithPositions()
        {
            dataGridView1.DataSource = Db.GetData(@"
                SELECT s.fio AS ""Сотрудник"",
                       d.nazvanie AS ""Должность"",
                       d.oklad AS ""Оклад""
                FROM sotrudniki s
                JOIN dolzhnost d ON d.dolzhnost_id = s.dolzhnost_id
                ORDER BY d.oklad DESC");
        }

        // ================ СЛОЖНЫЕ ЗАПРОСЫ ================

        // 1. Блюда, выручка по которым выше средней по ресторану
        private void Q_DishesAboveAvgRevenue()
        {
            dataGridView1.DataSource = Db.GetData(@"
                SELECT b.nazvanie AS ""Блюдо"",
                       SUM(sz.kolichestvo * sz.cena) AS ""Выручка""
                FROM sostav_zakaza sz
                JOIN bludo b ON b.bludo_id = sz.bludo_id
                GROUP BY b.nazvanie
                HAVING SUM(sz.kolichestvo * sz.cena) > (
                    SELECT AVG(bludo_sum)
                    FROM (
                        SELECT SUM(kolichestvo * cena) AS bludo_sum
                        FROM sostav_zakaza
                        GROUP BY bludo_id
                    ) t
                )
                ORDER BY 2 DESC");
        }

        // 2. Официанты со средним чеком выше среднего по ресторану
        private void Q_WaitersAboveAvgCheck()
        {
            dataGridView1.DataSource = Db.GetData(@"
                SELECT s.fio AS ""Официант"",
                       ROUND(AVG(o.summa), 2) AS ""Средний чек""
                FROM oplata o
                JOIN zakazi z ON z.zakaz_id = o.zakaz_id
                JOIN sotrudniki s ON s.sotrudnik_id = z.sotrudnik_id
                GROUP BY s.fio
                HAVING AVG(o.summa) > (SELECT AVG(summa) FROM oplata)
                ORDER BY 2 DESC");
        }

        // 3. Клиенты, заказавшие блюда дороже среднего по меню
        private void Q_ClientsAboveAvgPrice()
        {
            dataGridView1.DataSource = Db.GetData(@"
                SELECT DISTINCT c.fio AS ""Клиент""
                FROM client c
                JOIN zakazi z ON z.client_id = c.client_id
                JOIN sostav_zakaza sz ON sz.zakaz_id = z.zakaz_id
                WHERE sz.cena > (SELECT AVG(cena) FROM bludo)
                ORDER BY c.fio");
        }

        // 4. Категории блюд с количеством заказов выше среднего
        private void Q_CategoriesAboveAvgOrders()
        {
            dataGridView1.DataSource = Db.GetData(@"
                SELECT k.nazvanie AS ""Категория"",
                       SUM(sz.kolichestvo) AS ""Продано шт.""
                FROM sostav_zakaza sz
                JOIN bludo b ON b.bludo_id = sz.bludo_id
                JOIN kategoriya_menu k ON k.kategoriya_id = b.kategoriya_id
                GROUP BY k.nazvanie
                HAVING SUM(sz.kolichestvo) > (
                    SELECT AVG(cat_sum)
                    FROM (
                        SELECT SUM(sz2.kolichestvo) AS cat_sum
                        FROM sostav_zakaza sz2
                        JOIN bludo b2 ON b2.bludo_id = sz2.bludo_id
                        GROUP BY b2.kategoriya_id
                    ) t
                )
                ORDER BY 2 DESC");
        }

        // 5. Клиенты, которые сделали больше заказов, чем средний клиент
        private void Q_ClientsAboveAvgOrderCount()
        {
            dataGridView1.DataSource = Db.GetData(@"
                SELECT c.fio AS ""Клиент"",
                       COUNT(z.zakaz_id) AS ""Заказов""
                FROM client c
                LEFT JOIN zakazi z ON z.client_id = c.client_id
                GROUP BY c.client_id, c.fio
                HAVING COUNT(z.zakaz_id) > (
                    SELECT AVG(cnt)
                    FROM (
                        SELECT COUNT(z2.zakaz_id) AS cnt
                        FROM client c2
                        LEFT JOIN zakazi z2 ON z2.client_id = c2.client_id
                        GROUP BY c2.client_id
                    ) t
                )
                ORDER BY 2 DESC");
        }

        // 6. Заказы с суммой выше среднего чека
        private void Q_OrdersAboveAvgCheck()
        {
            dataGridView1.DataSource = Db.GetData(@"
                SELECT z.zakaz_id AS ""№ заказа"",
                       o.summa AS ""Сумма""
                FROM oplata o
                JOIN zakazi z ON z.zakaz_id = o.zakaz_id
                WHERE o.summa > (SELECT AVG(summa) FROM oplata)
                  AND z.data_zakaza BETWEEN @d1 AND @d2
                ORDER BY o.summa DESC",
                new NpgsqlParameter("@d1", dateTimePicker1.Value),
                new NpgsqlParameter("@d2", dateTimePicker2.Value));
        }

        // 7. Официанты, которые не оформили ни одного заказа
        private void Q_WaitersWithoutOrders()
        {
            dataGridView1.DataSource = Db.GetData(@"
                SELECT s.fio AS ""Официант""
                FROM sotrudniki s
                LEFT JOIN zakazi z ON z.sotrudnik_id = s.sotrudnik_id
                WHERE z.zakaz_id IS NULL
                ORDER BY s.fio");
        }

        // 8. Продукты, которые чаще всего используются в заказах
        private void Q_MostUsedProducts()
        {
            dataGridView1.DataSource = Db.GetData(@"
                SELECT p.nazvanie AS ""Продукт"",
                       SUM(sb.kolichestvo * sz.kolichestvo) AS ""Общий расход""
                FROM product p
                JOIN sostav_bluda sb ON sb.product_id = p.product_id
                JOIN sostav_zakaza sz ON sz.bludo_id = sb.bludo_id
                GROUP BY p.product_id, p.nazvanie
                ORDER BY 2 DESC");
        }

        // ================ ФИНАНСОВЫЕ (из оригинального Fin.cs) ================

        private void Totals()
        {
            string sql = @"
                SELECT 
                SUM(o.summa) AS ""Выручка"",
                SUM(sb.kolichestvo * sz.kolichestvo * p.cena) AS ""Себестоимость"",
                SUM(o.summa) - SUM(sb.kolichestvo * sz.kolichestvo * p.cena) AS ""Прибыль"",
                AVG(o.summa) AS ""Средний чек""
                FROM zakazi z
                JOIN oplata o ON o.zakaz_id = z.zakaz_id
                JOIN sostav_zakaza sz ON sz.zakaz_id = z.zakaz_id
                JOIN sostav_bluda sb ON sb.bludo_id = sz.bludo_id
                JOIN product p ON p.product_id = sb.product_id
                WHERE z.data_zakaza BETWEEN @d1 AND @d2";
            dataGridView1.DataSource = Db.GetData(sql,
                new NpgsqlParameter("@d1", dateTimePicker1.Value),
                new NpgsqlParameter("@d2", dateTimePicker2.Value));
        }

        private void RevenueByDay()
        {
            string sql = @"
                SELECT DATE(z.data_zakaza) AS ""День"",
                SUM(o.summa) AS ""Выручка""
                FROM zakazi z
                JOIN oplata o ON o.zakaz_id = z.zakaz_id
                WHERE z.data_zakaza BETWEEN @d1 AND @d2
                GROUP BY DATE(z.data_zakaza)
                ORDER BY DATE(z.data_zakaza)";
            var dt = Db.GetData(sql,
                new NpgsqlParameter("@d1", dateTimePicker1.Value),
                new NpgsqlParameter("@d2", dateTimePicker2.Value));
            dataGridView1.DataSource = dt;
            BuildChart(dt, "День", "Выручка", "Выручка по дням");
        }

        private void ProfitByDay()
        {
            string sql = @"
                SELECT DATE(z.data_zakaza) AS ""День"",
                SUM(o.summa) - SUM(sb.kolichestvo * sz.kolichestvo * p.cena) AS ""Прибыль""
                FROM zakazi z
                JOIN oplata o ON o.zakaz_id = z.zakaz_id
                JOIN sostav_zakaza sz ON sz.zakaz_id = z.zakaz_id
                JOIN sostav_bluda sb ON sb.bludo_id = sz.bludo_id
                JOIN product p ON p.product_id = sb.product_id
                WHERE z.data_zakaza BETWEEN @d1 AND @d2
                GROUP BY DATE(z.data_zakaza)
                ORDER BY DATE(z.data_zakaza)";
            var dt = Db.GetData(sql,
                new NpgsqlParameter("@d1", dateTimePicker1.Value),
                new NpgsqlParameter("@d2", dateTimePicker2.Value));
            dataGridView1.DataSource = dt;
            BuildChart(dt, "День", "Прибыль", "Прибыль по дням");
        }

        private void ProfitabilityByDay()
        {
            string sql = @"
                SELECT DATE(z.data_zakaza) AS ""День"",
                       ROUND(
                            CASE 
                            WHEN SUM(o.summa) = 0 THEN 0
                            ELSE (SUM(o.summa) - SUM(sb.kolichestvo * sz.kolichestvo * p.cena))
                             / SUM(o.summa) * 100
                             END, 2
                            ) AS ""Рентабельность""
                FROM zakazi z
                JOIN oplata o ON o.zakaz_id = z.zakaz_id
                JOIN sostav_zakaza sz ON sz.zakaz_id = z.zakaz_id
                JOIN sostav_bluda sb ON sb.bludo_id = sz.bludo_id
                JOIN product p ON p.product_id = sb.product_id
                WHERE z.data_zakaza BETWEEN @d1 AND @d2
                GROUP BY DATE(z.data_zakaza)
                ORDER BY DATE(z.data_zakaza)";
            var dt = Db.GetData(sql,
                new NpgsqlParameter("@d1", dateTimePicker1.Value),
                new NpgsqlParameter("@d2", dateTimePicker2.Value));
            dataGridView1.DataSource = dt;
            BuildChart(dt, "День", "Рентабельность", "Рентабельность (%)");
        }

        private void AverageCheckByDay()
        {
            string sql = @"
                SELECT DATE(z.data_zakaza) AS ""День"",
                       AVG(o.summa) AS ""Средний чек""
                FROM zakazi z
                JOIN oplata o ON o.zakaz_id = z.zakaz_id
                WHERE z.data_zakaza BETWEEN @d1 AND @d2
                GROUP BY DATE(z.data_zakaza)
                ORDER BY DATE(z.data_zakaza)";
            var dt = Db.GetData(sql,
                new NpgsqlParameter("@d1", dateTimePicker1.Value),
                new NpgsqlParameter("@d2", dateTimePicker2.Value));
            dataGridView1.DataSource = dt;
            BuildChart(dt, "День", "Средний чек", "Средний чек");
        }

        private void BuildChart(DataTable dt, string xColumn, string yColumn, string seriesName)
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            chart1.ChartAreas.Add("MainArea");
            var area = chart1.ChartAreas[0];

            area.AxisX.LabelStyle.Format = "dd.MM";
            area.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.LabelStyle.Angle = -45;

            area.AxisY.LabelStyle.Format = "N0";

            var series = chart1.Series.Add(seriesName);
            series.ChartType = SeriesChartType.Column;
            series.XValueType = ChartValueType.Date;

            series.IsValueShownAsLabel = true;
            series.LabelFormat = "N0";

            foreach (DataRow row in dt.Rows)
            {
                DateTime dateValue;
                if (row[xColumn] is DateOnly d)
                    dateValue = d.ToDateTime(TimeOnly.MinValue);
                else if (row[xColumn] is DateTime dtVal)
                    dateValue = dtVal;
                else
                    continue;
                series.Points.AddXY(dateValue, Convert.ToDecimal(row[yColumn]));
            }

            chart1.ChartAreas[0].RecalculateAxesScale();
            chart1.Legends[0].Docking = Docking.Top;
            chart1.BackColor = Color.WhiteSmoke;
            tabControl1.SelectedTab = tabPage2;
        }

        private void ExportToExcel()
        {
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("Нет данных для экспорта.");
                return;
            }
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel files (*.xlsx)|*.xlsx";
                sfd.FileName = $"Отчёт_{DateTime.Now:yyyyMMdd}.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("Отчёт");
                        int currentRow = 1;
                        ws.Cells[currentRow, 1].Value = comboBox1.Text;
                        ws.Cells[currentRow, 1, currentRow, dataGridView1.Columns.Count].Merge = true;
                        ws.Cells[currentRow, 1].Style.Font.Size = 16;
                        ws.Cells[currentRow, 1].Style.Font.Bold = true;
                        ws.Cells[currentRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        currentRow += 2;
                        ws.Cells[currentRow, 1].Value = $"Дата отчёта: {DateTime.Now:dd.MM.yyyy HH:mm}";
                        currentRow += 2;
                        for (int col = 0; col < dataGridView1.Columns.Count; col++)
                        {
                            ws.Cells[currentRow, col + 1].Value = dataGridView1.Columns[col].HeaderText;
                            ws.Cells[currentRow, col + 1].Style.Font.Bold = true;
                            ws.Cells[currentRow, col + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[currentRow, col + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        int headerRow = currentRow;
                        currentRow++;
                        for (int row = 0; row < dataGridView1.Rows.Count; row++)
                        {
                            for (int col = 0; col < dataGridView1.Columns.Count; col++)
                            {
                                var value = dataGridView1.Rows[row].Cells[col].Value;
                                ws.Cells[currentRow, col + 1].Value = value;
                                if (value is DateTime dt)
                                    ws.Cells[currentRow, col + 1].Style.Numberformat.Format = "dd.MM.yyyy";
                                else if (value is decimal || value is double || value is int)
                                    ws.Cells[currentRow, col + 1].Style.Numberformat.Format = "#,##0.00";
                                else if (value is DateOnly d)
                                {
                                    ws.Cells[currentRow, col + 1].Value = d.ToDateTime(TimeOnly.MinValue);
                                    ws.Cells[currentRow, col + 1].Style.Numberformat.Format = "dd.MM.yyyy";
                                }
                            }
                            currentRow++;
                        }
                        int dataEndRow = currentRow - 1;
                        ws.Cells[currentRow, 1].Value = "Итого:";
                        ws.Cells[currentRow, 1].Style.Font.Bold = true;
                        for (int col = 0; col < dataGridView1.Columns.Count; col++)
                        {
                            if (decimal.TryParse(dataGridView1.Rows[0].Cells[col].Value?.ToString(), out _))
                            {
                                string colLetter = ExcelCellAddress.GetColumnLetter(col + 1);
                                ws.Cells[currentRow, col + 1].Formula = $"SUM({colLetter}{headerRow + 1}:{colLetter}{dataEndRow})";
                                ws.Cells[currentRow, col + 1].Style.Font.Bold = true;
                            }
                        }
                        ws.Cells.AutoFitColumns();
                        File.WriteAllBytes(sfd.FileName, package.GetAsByteArray());
                    }
                    MessageBox.Show("Файл сохранён успешно!");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        public void FormatGrid()
        {
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                string name = col.HeaderText.ToLower();
                if (name.Contains("выручка") || name.Contains("прибыль") || name.Contains("себестоимость") || name.Contains("сумма") || name.Contains("чек") || name.Contains("цена"))
                {
                    col.DefaultCellStyle.Format = "N2";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (name.Contains("рентабельность") || name.Contains("%"))
                {
                    col.DefaultCellStyle.Format = "N2";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (name.Contains("количество") || name.Contains("заказов") || name.Contains("расход") || name.Contains("кол-во"))
                {
                    col.DefaultCellStyle.Format = "N0";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            FormatGrid();
        }
    }
}
