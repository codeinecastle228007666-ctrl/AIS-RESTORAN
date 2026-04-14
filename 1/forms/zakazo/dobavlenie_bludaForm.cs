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

namespace _1.forms.zakazo
{
    public partial class dobavlenie_bludaForm : Form
    {
        public dobavlenie_bludaForm()
        {
            InitializeComponent();
        }

        private void dobavlenie_bludaForm_Load(object sender, EventArgs e) // Загрузка блюд при открытии формы
        {
            LoadBluda("");
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 50;
            numericUpDown1.Value = 1;
        }

        private void LoadBluda(string search) // Загрузка блюд в DataGridView с учетом поискового запроса
        {
            string sql = $@"
            SELECT
            bludo_id AS ""ID"",
            nazvanie AS ""Название"", 
            cena AS ""Цена""
            FROM bludo
            WHERE LOWER (nazvanie) LIKE LOWER ('%{search}%')
            ORDER BY nazvanie
            ";
            dataGridView1.DataSource = Db.GetData(sql);
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) // Поиск блюд при вводе текста
        {
            LoadBluda(textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e) // Подтверждение выбора блюда и его количества
        {
            if (dataGridView1.CurrentCell == null) return; // Проверка на наличие выбранной ячейки
            
            SelectedBludoId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            cena = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Цена"].Value);
            kolichestvo = (int)numericUpDown1.Value;

            this.DialogResult = DialogResult.OK; 
            this.Close(); 

        }

        public int SelectedBludoId { get; private set; }
        public decimal cena { get; private set; }
        public int kolichestvo { get; private set; }
    }
}
