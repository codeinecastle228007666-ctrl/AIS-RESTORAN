using System;
using System.Data;
using System.Windows.Forms;
using _1.data;
using Npgsql;

namespace _1.forms
{
    public partial class oplata : Form
    {
        private int _zakazId;
        public oplata(int zakazId)
        {
            InitializeComponent();
            _zakazId = zakazId;
        }

        private void oplata_Load(object sender, EventArgs e)
        {
            string checksql = "SELECT COUNT(*) FROM oplata WHERE zakaz_id = @zakaz";
            try
            {
                var checkTable = Db.GetData(checksql,
                    new NpgsqlParameter("@zakaz", _zakazId));

                if (checkTable.Rows.Count == 0)
                {
                    MessageBox.Show("Ошибка получения данных");
                    return;
                }

                if (Convert.ToInt32(checkTable.Rows[0][0]) > 0)
                {
                    MessageBox.Show("Этот заказ уже оплачен!");
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка проверки оплаты:\n" + ex.Message);
            }

            string sql = "SELECT sposob_oplati_id, nazvanie FROM sposob_oplati";
            comboBox1.DataSource = Db.GetData(sql);
            comboBox1.DisplayMember = "nazvanie";
            comboBox1.ValueMember = "sposob_oplati_id";

            LoadSumma();
        }

        private void LoadSumma()
        {
            string sql = "SELECT SUM(kolichestvo * cena) FROM sostav_zakaza WHERE zakaz_id = @zakaz";
            var table = Db.GetData(sql, new NpgsqlParameter("@zakaz", _zakazId));

            if (table.Rows.Count > 0 && table.Rows[0][0] != DBNull.Value)
            {
                textBox1.Text = table.Rows[0][0].ToString();
            }
            else
            {
                textBox1.Text = "0";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Выберите способ оплаты");
                return;
            }

            int sposobOplatiId = Convert.ToInt32(comboBox1.SelectedValue);
            decimal summa = Convert.ToDecimal(textBox1.Text);

            if (summa <= 0)
            {
                MessageBox.Show("Заказ не содержит позиций для оплаты");
                return;
            }

            string sql = @"
                CALL sp_make_oplata(@p_zakaz_id, @p_sposob_oplati_id);
                UPDATE zakazi SET status_zakaza_id = 5 WHERE zakaz_id = @zakaz;
            ";

            try
            {
                Db.ekzekuttranzakcii(sql,
                    new NpgsqlParameter("@p_zakaz_id", _zakazId),
                    new NpgsqlParameter("@p_sposob_oplati_id", sposobOplatiId),
                    new NpgsqlParameter("@zakaz", _zakazId)
                );

                MessageBox.Show("Оплата успешно проведена!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка оплаты:\n" + ex.Message);
            }
        }
    }
}
