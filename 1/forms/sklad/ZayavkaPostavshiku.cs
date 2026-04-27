using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using _1.data;

namespace _1.forms.sklad
{
    public partial class ZayavkaPostavshiku : Form
    {
        public ZayavkaPostavshiku()
        {
            InitializeComponent();
        }

        private void ZayavkaPostavshiku_Load(object sender, EventArgs e)
        {
            LoadPostavshiki();
            LoadProducts();
            LoadZayavki();
        }

        private void LoadPostavshiki()
        {
            string sql = "SELECT postavschik_id, nazvanie FROM postavschik ORDER BY nazvanie";
            comboBoxPostavshik.DataSource = Db.GetData(sql);
            comboBoxPostavshik.DisplayMember = "nazvanie";
            comboBoxPostavshik.ValueMember = "postavschik_id";
            comboBoxPostavshik.SelectedIndex = -1;
        }

        private void LoadProducts()
        {
            string sql = "SELECT product_id, nazvanie FROM product ORDER BY nazvanie";
            comboBoxProduct.DataSource = Db.GetData(sql);
            comboBoxProduct.DisplayMember = "nazvanie";
            comboBoxProduct.ValueMember = "product_id";
            comboBoxProduct.SelectedIndex = -1;
        }

        private void LoadZayavki()
        {
            string path = Application.StartupPath + "\\zayavki\\";
            if (System.IO.Directory.Exists(path))
            {
                var files = System.IO.Directory.GetFiles(path, "*.txt");
                DataTable dt = new DataTable();
                dt.Columns.Add("Дата");
                dt.Columns.Add("Поставщик");
                dt.Columns.Add("Товар");
                dt.Columns.Add("Количество");
                dt.Columns.Add("Статус");

                foreach (var file in files)
                {
                    string[] lines = System.IO.File.ReadAllLines(file);
                    if (lines.Length >= 5)
                    {
                        DataRow row = dt.NewRow();
                        row["Дата"] = lines[0].Replace("Дата: ", "");
                        row["Поставщик"] = lines[2].Replace("Поставщик: ", "");
                        row["Товар"] = lines[3].Replace("Товар: ", "");
                        row["Количество"] = lines[4].Replace("Количество: ", "");
                        row["Статус"] = lines[5].Replace("Статус: ", "");
                        dt.Rows.Add(row);
                    }
                }
                dataGridViewZayavki.DataSource = dt;
                dataGridViewZayavki.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxPostavshik.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите поставщика");
                    return;
                }

                if (comboBoxProduct.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите продукт");
                    return;
                }

                if (!decimal.TryParse(textBoxKolichestvo.Text, out decimal kolvo) || kolvo <= 0)
                {
                    MessageBox.Show("Введите корректное количество");
                    return;
                }

                string postavshik = comboBoxPostavshik.Text;
                string product = comboBoxProduct.Text;
                string data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string status = "Новая";

                string dir = Application.StartupPath + "\\zayavki\\";
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                string fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{postavshik}_{product}.txt";
                string filePath = dir + fileName;

                string content = $@"Дата: {data}
Номер: {DateTime.Now:yyyyMMddHHmmss}
Поставщик: {postavshik}
Товар: {product}
Количество: {kolvo:0.000}
Статус: {status}
========================================
Примечание: Заявка сформирована автоматически
Дата создания: {DateTime.Now:dd.MM.yyyy HH:mm:ss}";

                System.IO.File.WriteAllText(filePath, content, Encoding.UTF8);

                MessageBox.Show($"Заявка создана!\n\nПоставщик: {postavshik}\nТовар: {product}\nКоличество: {kolvo:0.000}",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                comboBoxPostavshik.SelectedIndex = -1;
                comboBoxProduct.SelectedIndex = -1;
                textBoxKolichestvo.Clear();
                LoadZayavki();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (dataGridViewZayavki.CurrentRow == null)
            {
                MessageBox.Show("Выберите заявку");
                return;
            }

            string date = dataGridViewZayavki.CurrentRow.Cells["Дата"].Value.ToString();
            string postavshik = dataGridViewZayavki.CurrentRow.Cells["Поставщик"].Value.ToString();
            string product = dataGridViewZayavki.CurrentRow.Cells["Товар"].Value.ToString();

            string dir = Application.StartupPath + "\\zayavki\\";
            if (System.IO.Directory.Exists(dir))
            {
                var files = System.IO.Directory.GetFiles(dir, "*.txt");
                foreach (var file in files)
                {
                    string content = System.IO.File.ReadAllText(file);
                    if (content.Contains(date) && content.Contains(postavshik) && content.Contains(product))
                    {
                        System.Diagnostics.Process.Start("notepad.exe", file);
                        return;
                    }
                }
            }
            MessageBox.Show("Файл заявки не найден");
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewZayavki.CurrentRow == null)
            {
                MessageBox.Show("Выберите заявку");
                return;
            }

            string date = dataGridViewZayavki.CurrentRow.Cells["Дата"].Value.ToString();
            string postavshik = dataGridViewZayavki.CurrentRow.Cells["Поставщик"].Value.ToString();
            string product = dataGridViewZayavki.CurrentRow.Cells["Товар"].Value.ToString();

            if (MessageBox.Show($"Удалить заявку от {date}?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string dir = Application.StartupPath + "\\zayavki\\";
                if (System.IO.Directory.Exists(dir))
                {
                    var files = System.IO.Directory.GetFiles(dir, "*.txt");
                    foreach (var file in files)
                    {
                        string content = System.IO.File.ReadAllText(file);
                        if (content.Contains(date) && content.Contains(postavshik) && content.Contains(product))
                        {
                            System.IO.File.Delete(file);
                            MessageBox.Show("Заявка удалена");
                            LoadZayavki();
                            return;
                        }
                    }
                }
            }
        }
    }
}