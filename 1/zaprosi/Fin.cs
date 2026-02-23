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

namespace _1.zaprosi
{
    public partial class Fin : Form
    {
        public Fin()
        {
            InitializeComponent();
            comboBox1.Items.Add("Итоги за период"); // Добавляем пункты в ComboBox
            comboBox1.Items.Add("Выручка по дням");
            comboBox1.Items.Add("Прибыль по дням");
            comboBox1.Items.Add("Заказы выше среднего чека");
            comboBox1.Items.Add("Блюда с выручкой выше средней");
            comboBox1.Items.Add("Рентабельность по дням");
            comboBox1.Items.Add("Средний чек по дням");
            comboBox1.SelectedIndex = 0; // Устанавливаем первый пункт по умолчанию
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    Totals(); break;
                case 1:
                    RevenueByDay(); break;
                case 2:
                    ProfitByDay(); break;
                case 3:
                    OrdersAboveAverageCheck(); break;
                case 4:
                    DishesAboveAverageRevenue(); break;
                case 5:
                    ProfitabilityByDay(); break;
                case 6:
                    AverageCheckByDay(); break;
   
                
            }

        }


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
            WHERE z.data_zakaza BETWEEN @d1 AND @d2;
            ";

            dataGridView1.DataSource = Db.GetData(sql,
                new NpgsqlParameter("@d1", dateTimePicker1.Value),
                new NpgsqlParameter("@d2", dateTimePicker2.Value));

            tabControl1.SelectedTab = tabPage1; // вкладка с таблицей
        }

        private void RevenueByDay()
        {
            string sql = @"
                SELECT DATE (z.data_zakaza) AS ""Дата"",
                SUM (o.summa) AS ""Выручка""
                FROM zakazi z
                JOIN oplata o ON o.zakaz_id = z.zakaz_id
                WHERE z.data_zakaza BETWEEN @d1 AND @d2
                GROUP BY DATE (z.data_zakaza)
                ORDER BY DATE (z.data_zakaza);
                ";
            var dt = Db.GetData(sql,
                new NpgsqlParameter("@d1", dateTimePicker1.Value),
                new NpgsqlParameter("@d2", dateTimePicker2.Value));

            dataGridView1.DataSource = dt;
            BuildChart(dt, "Дата", "Выручка", "Выручка по дням");
        }

        private void ProfitByDay()
        {
            string sql = @"
                SELECT DATE (z.data_zakaza) AS ""Дата"",
                SUM (o.summa) - SUM (sb.kolichestvo * sz.kolichestvo * p.cena) AS ""Прибыль""
                FROM zakazi z
                JOIN oplata o ON o.zakaz_id = z.zakaz_id
                JOIN sostav_zakaza sz ON sz.zakaz_id = z.zakaz_id
                JOIN sostav_bluda sb ON sb.bludo_id = sz.bludo_id
                JOIN product p ON p.product_id = sb.product_id
                WHERE z.data_zakaza BETWEEN @d1 AND @d2
                GROUP BY DATE (z.data_zakaza)
                ORDER BY DATE (z.data_zakaza);
                ";
            var dt = Db.GetData(sql,
                new NpgsqlParameter("@d1", dateTimePicker1.Value),
                new NpgsqlParameter("@d2", dateTimePicker2.Value));
            dataGridView1.DataSource = dt;
            BuildChart(dt, "Дата", "Прибыль", "Прибыль по дням");
        }

        private void BuildChart(DataTable dt, string xColumn, string yColumn, string seriesName)
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            chart1.ChartAreas.Add("MainArea");
            var area = chart1.ChartAreas[0];

            area.AxisX.LabelStyle.Format = "dd.MM";
            area.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.LabelStyle.Angle = -45;

            area.AxisY.LabelStyle.Format = "N0";

            var series = chart1.Series.Add(seriesName);
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            series.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;

            series.IsValueShownAsLabel = true;   // <-- подписи
            series.LabelFormat = "N0";           // формат без копеек

            foreach (DataRow row in dt.Rows)
            {
                DateTime dateValue;

                if (row[xColumn] is DateOnly d)
                    dateValue = d.ToDateTime(TimeOnly.MinValue);
                else if (row[xColumn] is DateTime dtValue)
                    dateValue = dtValue;
                else
                    continue;

                series.Points.AddXY(
                    dateValue,
                    Convert.ToDecimal(row[yColumn]));
            }

            chart1.ChartAreas[0].RecalculateAxesScale();
            chart1.Legends[0].Docking = Docking.Top;
            chart1.BackColor = Color.WhiteSmoke;
            tabControl1.SelectedTab = tabPage2;
        }

        private void OrdersAboveAverageCheck()
        {
            string sql = @"
        SELECT z.zakaz_id AS ""Номер заказа"",
               z.data_zakaza AS ""Дата"",
               o.summa AS ""Сумма"",
               s.fio AS ""Официант""
        FROM zakazi z
        JOIN oplata o ON o.zakaz_id = z.zakaz_id
        JOIN sotrudniki s ON s.sotrudnik_id = z.sotrudnik_id
        WHERE o.summa > (
            SELECT AVG(summa)
            FROM oplata
        )
        ORDER BY o.summa DESC;
    ";

            dataGridView1.DataSource = Db.GetData(sql);
        }

        private void DishesAboveAverageRevenue()
        {
            string sql = @"
        SELECT b.nazvanie AS ""Название"",
               SUM(sz.kolichestvo * b.cena) AS ""Выручка""
        FROM bludo b
        JOIN sostav_zakaza sz ON sz.bludo_id = b.bludo_id
        GROUP BY b.nazvanie
        HAVING SUM(sz.kolichestvo * b.cena) >
               (
                 SELECT AVG(total)
                 FROM (
                      SELECT SUM(sz2.kolichestvo * b2.cena) AS total
                      FROM bludo b2
                      JOIN sostav_zakaza sz2 ON sz2.bludo_id = b2.bludo_id
                      GROUP BY b2.bludo_id
                 ) sub
               );
    ";

            dataGridView1.DataSource = Db.GetData(sql);
        }

        private void ProfitabilityByDay()
        {
            string sql = @"
        SELECT DATE(z.data_zakaza) AS ""Дата"",
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
        ORDER BY DATE(z.data_zakaza);
    ";

            var dt = Db.GetData(sql,
                new NpgsqlParameter("@d1", dateTimePicker1.Value),
                new NpgsqlParameter("@d2", dateTimePicker2.Value));

            dataGridView1.DataSource = dt;
            BuildChart(dt, "Дата", "Рентабельность", "Рентабельность (%)");
        }

        private void AverageCheckByDay()
        {
            string sql = @"
        SELECT DATE(z.data_zakaza) AS ""Дата"",
               AVG(o.summa) AS ""Средний чек""
        FROM zakazi z
        JOIN oplata o ON o.zakaz_id = z.zakaz_id
        WHERE z.data_zakaza BETWEEN @d1 AND @d2
        GROUP BY DATE(z.data_zakaza)
        ORDER BY DATE(z.data_zakaza);
    ";

            var dt = Db.GetData(sql,
                new NpgsqlParameter("@d1", dateTimePicker1.Value),
                new NpgsqlParameter("@d2", dateTimePicker2.Value));

            dataGridView1.DataSource = dt;
            BuildChart(dt, "Дата", "Средний чек", "Средний чек");
        }

        

       
    }
}
