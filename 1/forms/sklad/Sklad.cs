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

        private void LoadSklad()
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
        ORDER BY p.nazvanie
    ";

            dataGridView1.DataSource = Db.GetData(sql);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }


        private void Sklad_Load(object sender, EventArgs e)
        {
            LoadSklad();
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

        private void button1_Click(object sender, EventArgs e)
        {
            Prihod form = new Prihod();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadSklad();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SkladJournal form = new SkladJournal();
            form.ShowDialog();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
           
        }

      
    }

}
