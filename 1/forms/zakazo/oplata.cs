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
        
        private void oplata_Load(object sender, EventArgs e) //загрузка данных при открытии формы
        {
            string checksql = $@"
                SELECT COUNT(*) 
                FROM oplata 
                WHERE zakaz_id = {_zakazId}
            ";
            var checkTable = Db.GetData(checksql);
            if (Convert.ToInt32(checkTable.Rows[0][0]) > 0)
            {
                MessageBox.Show("Этот заказ уже оплачен!");
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            string sql = "SELECT sposob_oplati_id, nazvanie FROM sposob_oplati";

            comboBox1.DataSource = Db.GetData(sql);
            comboBox1.DisplayMember = "nazvanie";
            comboBox1.ValueMember = "sposob_oplati_id";

            LoadSumma();
        }

        private void LoadSumma() //подгрузка суммы заказа для отображения в текстовом поле
        {
            string sql = $@"
            SELECT SUM (kolichestvo * cena)
            FROM sostav_zakaza
            WHERE zakaz_id = {_zakazId}
            ";

            var table = Db.GetData(sql);

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
            int sposobOplatiId = Convert.ToInt32(comboBox1.SelectedValue);
            decimal summa = Convert.ToDecimal(textBox1.Text);

            string sql = @"
        INSERT INTO oplata (zakaz_id, data_oplati, sposob_oplati_id, summa)
        VALUES (@zakaz, NOW(), @sposob, @summa);

        UPDATE zakazi
        SET status_zakaza_id = 2
        WHERE zakaz_id = @zakaz;
    ";

            Db.ekzekuttranzakcii(sql,
                new Npgsql.NpgsqlParameter("@zakaz", _zakazId),
                new Npgsql.NpgsqlParameter("@sposob", sposobOplatiId),
                new Npgsql.NpgsqlParameter("@summa", summa)
            );

            MessageBox.Show("Оплата успешно проведена!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}