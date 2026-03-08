using _1.data;
using Npgsql;
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
    public partial class Prihod : Form
    {
        public Prihod()
        {
            InitializeComponent();
        }



        private void LoadPostavshiki()
        {
            string sql = "SELECT postavschik_id, nazvanie FROM postavschik ORDER BY nazvanie";
            comboBox2.DataSource = Db.GetData(sql);
            comboBox2.DisplayMember = "nazvanie";
            comboBox2.ValueMember = "postavschik_id";

        }


        private void LoadProducts()
        {
            string sql = "SELECT product_id, nazvanie FROM product ORDER BY nazvanie";
            comboBox1.DataSource = Db.GetData(sql);
            comboBox1.DisplayMember = "nazvanie";
            comboBox1.ValueMember = "product_id";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int productId = Convert.ToInt32(comboBox1.SelectedValue);
            int postavId = Convert.ToInt32(comboBox2.SelectedValue);

            if (!decimal.TryParse(textBox1.Text, out decimal kolvo) || kolvo <= 0)
            {
                MessageBox.Show("Неверное количество");
                return;
            }

            if (!decimal.TryParse(textBox2.Text, out decimal cena) || cena <= 0)
            {
                MessageBox.Show("Неверная цена");
                return;
            }

            string sql1 = @"
        UPDATE sklad
        SET kolichestvo = kolichestvo + @kolvo
        WHERE product_id = @productId
    ";

            string sql2 = @"
        INSERT INTO sklad_dvizhenie
        (product_id, tip, kolichestvo, postavschik_id, cena)
        VALUES
        (@productId, 'IN', @kolvo, @postavId, @cena)
    ";

            try
            {
                Db.ekzekuttranzakcii(sql1,
                    new NpgsqlParameter("@kolvo", kolvo),
                    new NpgsqlParameter("@productId", productId)
                );

                Db.ekzekuttranzakcii(sql2,
                    new NpgsqlParameter("@productId", productId),
                    new NpgsqlParameter("@kolvo", kolvo),
                    new NpgsqlParameter("@postavId", postavId),
                    new NpgsqlParameter("@cena", cena)
                );

                MessageBox.Show("Приход сохранён");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Prihod_Load(object sender, EventArgs e)
        {
            LoadPostavshiki();
            LoadProducts();
        }
    }
}