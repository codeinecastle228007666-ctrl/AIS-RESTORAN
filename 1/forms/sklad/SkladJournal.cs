using _1.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1.forms.sklad
{
    public partial class SkladJournal : Form
    {
        public SkladJournal()
        {
            InitializeComponent();
        }

        private void SkladJournal_Load(object sender, EventArgs e)
        {
            string sql = @"
        SELECT
            d.dvizhenie_id AS ""ID"",
            p.nazvanie AS ""Продукт"",
            d.tip AS ""Тип"",
            d.kolichestvo AS ""Количество"",
            d.zakaz_id AS ""Заказ"",
            d.data_dvizheniya AS ""Дата""
        FROM sklad_dvizhenie d
        JOIN product p ON p.product_id = d.product_id
        ORDER BY d.data_dvizheniya DESC
    ";

            dataGridView1.DataSource = Db.GetData(sql);
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
