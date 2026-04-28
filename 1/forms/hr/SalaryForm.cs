using _1.data;
using Npgsql;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace _1.forms
{
    public partial class SalaryForm : Form
    {
        private int _sotrudnikId;
        private decimal _oklad = 0;
        private string _fio;
        private string _dolzhnost;
        private decimal _total = 0;
        private int _daysWorked = 0;
        private decimal _premium = 0;
        private decimal _deductions = 0;
        private DateTime _periodStart;
        private DateTime _periodEnd;

        public SalaryForm(int sotrudnikId, string fio, string dolzhnost)
        {
            InitializeComponent();
            _sotrudnikId = sotrudnikId;
            _fio = fio;
            _dolzhnost = dolzhnost;
            labelEmployee.Text = $"Сотрудник: {fio}\nДолжность: {dolzhnost}";

            // По умолчанию период – текущий месяц
            DateTime now = DateTime.Now;
            _periodStart = new DateTime(now.Year, now.Month, 1);
            _periodEnd = _periodStart.AddMonths(1).AddDays(-1);
            dateTimePickerStart.Value = _periodStart;
            dateTimePickerEnd.Value = _periodEnd;

            LoadOklad();
        }

        private void LoadOklad()
        {
            string sql = @"
                SELECT d.oklad
                FROM sotrudniki s
                JOIN dolzhnost d ON s.dolzhnost_id = d.dolzhnost_id
                WHERE s.sotrudnik_id = @id
            ";
            var dt = Db.GetData(sql, new NpgsqlParameter("@id", _sotrudnikId));
            if (dt.Rows.Count > 0)
            {
                _oklad = Convert.ToDecimal(dt.Rows[0]["oklad"]);
                labelOklad.Text = $"Оклад: {_oklad:N2} руб.";
            }
            else
            {
                _oklad = 0;
                labelOklad.Text = "Оклад не найден";
            }
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxDays.Text, out int days) || days < 0 || days > 31)
            {
                MessageBox.Show("Введите корректное количество рабочих дней (0–31)");
                return;
            }
            decimal premium = 0;
            decimal deductions = 0;
            decimal.TryParse(textBoxPremium.Text, out premium);
            decimal.TryParse(textBoxDeductions.Text, out deductions);

            int normDays = 21; // норма рабочих дней в месяце
            decimal baseSalary = _oklad / normDays * days;
            decimal total = baseSalary + premium - deductions;
            if (total < 0) total = 0;

            // Сохраняем для экспорта
            _daysWorked = days;
            _premium = premium;
            _deductions = deductions;
            _total = total;
            _periodStart = dateTimePickerStart.Value.Date;
            _periodEnd = dateTimePickerEnd.Value.Date;

            labelResult.Text = $"Сумма на руки: {total:N2} руб.";
            labelResult.Visible = true;
            buttonExport.Enabled = true;

            // Автоматический экспорт после расчёта (можно убрать, если не нужно)
            // ExportToExcel();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void ExportToExcel()
        {
            if (_daysWorked == 0 && _total == 0)
            {
                MessageBox.Show("Сначала выполните расчёт.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel files (*.xlsx)|*.xlsx";
                sfd.FileName = $"Зарплата_{_fio.Replace(" ", "_")}_{_periodStart:yyyyMM}.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExcelPackage.License.SetNonCommercialPersonal("Student Project");
                        using (var package = new ExcelPackage())
                        {
                            var ws = package.Workbook.Worksheets.Add("Расчёт зарплаты");

                            // Заголовок
                            ws.Cells[1, 1, 1, 3].Merge = true;
                            ws.Cells[1, 1].Value = "РАСЧЁТ ЗАРАБОТНОЙ ПЛАТЫ";
                            ws.Cells[1, 1].Style.Font.Bold = true;
                            ws.Cells[1, 1].Style.Font.Size = 14;
                            ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            // Информация о сотруднике
                            ws.Cells[3, 1].Value = "Сотрудник:";
                            ws.Cells[3, 2].Value = _fio;
                            ws.Cells[4, 1].Value = "Должность:";
                            ws.Cells[4, 2].Value = _dolzhnost;
                            ws.Cells[5, 1].Value = "Период:";
                            ws.Cells[5, 2].Value = $"{_periodStart:dd.MM.yyyy} – {_periodEnd:dd.MM.yyyy}";
                            ws.Cells[5, 2].Style.Numberformat.Format = "dd.MM.yyyy";

                            // Расчётные данные
                            ws.Cells[7, 1].Value = "Показатель";
                            ws.Cells[7, 2].Value = "Значение";
                            ws.Cells[7, 1, 7, 2].Style.Font.Bold = true;

                            ws.Cells[8, 1].Value = "Оклад (руб.)";
                            ws.Cells[8, 2].Value = _oklad;
                            ws.Cells[9, 1].Value = "Норма дней";
                            ws.Cells[9, 2].Value = 21;
                            ws.Cells[10, 1].Value = "Отработано дней";
                            ws.Cells[10, 2].Value = _daysWorked;
                            ws.Cells[11, 1].Value = "Базовая ЗП";
                            ws.Cells[11, 2].Value = _oklad / 21 * _daysWorked;
                            ws.Cells[12, 1].Value = "Премия";
                            ws.Cells[12, 2].Value = _premium;
                            ws.Cells[13, 1].Value = "Удержания";
                            ws.Cells[13, 2].Value = _deductions;
                            ws.Cells[14, 1].Value = "ИТОГО К ВЫДАЧЕ";
                            ws.Cells[14, 2].Value = _total;
                            ws.Cells[14, 1, 14, 2].Style.Font.Bold = true;

                            // Форматирование денежных ячеек
                            for (int row = 8; row <= 14; row++)
                            {
                                ws.Cells[row, 2].Style.Numberformat.Format = "#,##0.00";
                            }

                            ws.Cells.AutoFitColumns();
                            ws.Column(1).Width = 25;
                            ws.Column(2).Width = 20;

                            File.WriteAllBytes(sfd.FileName, package.GetAsByteArray());
                        }
                        MessageBox.Show("Экспорт успешно завершён!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при экспорте: " + ex.Message);
                    }
                }
            }
        }
    }
}