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
using _1.forms.zakazo;

namespace _1.forms.zakazo
{
    public partial class zakazi_itemsForm : Form
    {
        private int _zakaziId;
        public zakazi_itemsForm(int zakaziId)
        {
            InitializeComponent();
            _zakaziId = zakaziId;
        }

        private bool _oplacheno = false;
        private void zakazi_itemsForm_Load(object sender, EventArgs e)
        {
            string checksql = $@"
                SELECT COUNT(*) 
                FROM oplata 
                WHERE zakaz_id = {_zakaziId}
            ";
            var checkTable = Db.GetData(checksql);

            _oplacheno = Convert.ToInt32(checkTable.Rows[0][0]) > 0;

            if (_oplacheno)
            {
                MessageBox.Show("Этот заказ уже оплачен! Редактирование состава заказа невозможно.");
                button1.Enabled = false; // отключаем кнопку добавления новых позиций
                button2.Enabled = false; // отключаем кнопку удаления позиций
            }
            LoadItems();

        }

        private void LoadItems() //дефолт загрузка всех нужных полей в таблицу
        {
            string sql = $@"
                SELECT 
                    sz.sostav_id AS ""ID"",
                    b.nazvanie AS ""Блюдо"",
                    sz.kolichestvo AS ""Количество"",
                    sz.cena AS ""Цена"",
                    (sz.kolichestvo * sz.cena) AS ""Сумма"" 
                FROM sostav_zakaza sz
                JOIN bludo b ON b.bludo_id = sz.bludo_id
                WHERE sz.zakaz_id = {_zakaziId}
            ";
            dataGridView1.DataSource = Db.GetData(sql);
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_oplacheno) return;

            dobavlenie_bludaForm form = new dobavlenie_bludaForm();

            if (form.ShowDialog() == DialogResult.OK)
            {
                string sql = @"
            INSERT INTO sostav_zakaza 
            (zakaz_id, bludo_id, kolichestvo, cena)
            VALUES
            (@zakaz, @bludo, @kolvo, @cena);
        ";

                Db.ekzekuttranzakcii(sql,
                    new Npgsql.NpgsqlParameter("@zakaz", _zakaziId),
                    new Npgsql.NpgsqlParameter("@bludo", form.SelectedBludoId),
                    new Npgsql.NpgsqlParameter("@kolvo", form.kolichestvo),
                    new Npgsql.NpgsqlParameter("@cena", form.cena)
                );

                LoadItems();
            }
        }


        private void button2_Click(object sender, EventArgs e) //удаление блюда
        {
            if (_oplacheno) return;
            if (dataGridView1.CurrentRow == null) return;
            int sostavZakazaId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            string sql = $@"
                DELETE FROM sostav_zakaza
                WHERE sostav_id = {sostavZakazaId}
            ";
            Db.ekzekuttranzakcii(sql);
            LoadItems();
        }

       
    }
}
