using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _1.data;
using _1.forms;

namespace _1
{
    public partial class Zakazi : Form
    {
        public Zakazi()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Zakazi_Load(object sender, EventArgs e)
        {
            LoadZakazi();
        }

        private void LoadZakazi() //дефолт загрузка всех нужных полей в таблицу
        {
            string sql = @"
                SELECT 
                    z.zakaz_id AS ""ID"",
                    c.fio AS ""Клиент"",
                    s.fio AS ""Сотрудник"",
                    z.data_zakaza AS ""Дата заказа"",
                    sz.nazvanie AS ""Статус""
                    FROM zakazi z
                    JOIN client c ON c.client_id = z.client_id
                    JOIN sotrudniki s ON s.sotrudnik_id = z.sotrudnik_id
                    JOIN status_zakaza sz ON sz.status_zakaza_id = z.status_zakaza_id
                    ORDER BY z.data_zakaza DESC
                ";
            dataGridView1.DataSource = Db.GetData(sql);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void button4_Click(object sender, EventArgs e) //кнопка оплаты заказа
        {
            if (dataGridView1.CurrentRow == null) return;
            int zakazId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            oplata oplataForm = new oplata(zakazId);

            if (oplataForm.ShowDialog() == DialogResult.OK)
            {
                LoadZakazi(); //обновляем данные в таблице после оплаты
            }

        }
    }
}
