using _1.data;
using _1.forms.sklad;
using _1.zaprosi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1.forms
{
    public partial class Sklad : Form
    {
        public Sklad()
        {
            InitializeComponent();
        }

        private void LoadSklad(string search = "")
        {
            string sql = @"
                SELECT 
                    s.product_id AS ""ID"",
                    p.nazvanie AS ""Продукт"",
                    e.nazvanie AS ""Ед. изм"",
                    s.kolichestvo AS ""Остаток""
                FROM sklad s
                JOIN product p ON p.product_id = s.product_id
                JOIN edinica_izmereniya e ON e.edinica_izmereniya_id = p.edinica_izmereniya_id
                WHERE 1=1
            ";

            if (!string.IsNullOrWhiteSpace(search))
            {
                sql += " AND LOWER(p.nazvanie) LIKE @search";
            }

            sql += " ORDER BY p.nazvanie";

            if (!string.IsNullOrWhiteSpace(search))
            {
                dataGridView1.DataSource = Db.GetData(sql, new Npgsql.NpgsqlParameter("@search", $"%{search.ToLower()}%"));
            }
            else
            {
                dataGridView1.DataSource = Db.GetData(sql);
            }

            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CheckLowStock()
        {
            string sql = @"
                SELECT p.nazvanie, s.kolichestvo, e.nazvanie
                FROM sklad s
                JOIN product p ON p.product_id = s.product_id
                JOIN edinica_izmereniya e ON e.edinica_izmereniya_id = p.edinica_izmereniya_id
                WHERE s.kolichestvo < 1
                ORDER BY s.kolichestvo ASC
            ";

            DataTable dt = Db.GetData(sql);
            if (dt.Rows.Count > 0)
            {
                StringBuilder msg = new StringBuilder("Внимание! Заканчиваются следующие продукты:\n\n");
                foreach (DataRow row in dt.Rows)
                {
                    msg.AppendLine($"• {row["nazvanie"]} — {row["kolichestvo"]} {row["edinica_izmereniya_id"]}");
                }
                MessageBox.Show(msg.ToString(), "Низкие остатки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Sklad_Load(object sender, EventArgs e)
        {
            LoadSklad("");
            CheckLowStock();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name != "Остаток") return;
            if (e.Value == null) return;

            if (decimal.TryParse(e.Value.ToString(), out decimal ostatok))
            {
                if (ostatok <= 5)
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            Prihod form = new Prihod();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadSklad(textBoxSearch.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SkladJournal form = new SkladJournal();
            form.ShowDialog();
        }

        private void buttonZayavka_Click(object sender, EventArgs e)
        {
            ZayavkaPostavshiku form = new ZayavkaPostavshiku();
            form.ShowDialog();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            LoadSklad(textBoxSearch.Text);
        }
    }
}
