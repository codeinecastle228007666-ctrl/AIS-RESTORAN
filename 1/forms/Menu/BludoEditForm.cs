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

namespace _1.forms.Menu
{
    public partial class BludoEditForm : Form
    {
        int bludoId = -1;
        public BludoEditForm()
        {
            InitializeComponent();
        }

        public BludoEditForm(int id)
        {
            InitializeComponent();
            bludoId = id;
        }

        private void BludoEditForm_Load(object sender, EventArgs e)
        {
            LoadKategoriyaMenu();
            if (bludoId != -1)
                LoadBludo();
        }

        void LoadKategoriyaMenu()
        {
            string sql = "SELECT kategoriya_id, nazvanie FROM kategoriya_menu ORDER BY nazvanie";
            DataTable dt = Db.GetData(sql);
            comboBoxKategoriya.DataSource = dt;
            comboBoxKategoriya.DisplayMember = "nazvanie";
            comboBoxKategoriya.ValueMember = "kategoriya_id";
        }

        void LoadBludo()
        {
            string sql = $@"
            SELECT *
            FROM bludo
            WHERE bludo_id = {bludoId}";

            var table = Db.GetData(sql);
            if (table.Rows.Count == 0) return;
            var row = table.Rows[0];

            textBoxName.Text = row["nazvanie"].ToString();
            textBoxCena.Text = row["cena"].ToString();
            textBoxOpisanie.Text = row["opisanie"].ToString();
            comboBoxKategoriya.SelectedValue = row["kategoriya_id"];
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            decimal cena = Convert.ToDecimal(textBoxCena.Text);
            int kategoriya = Convert.ToInt32(comboBoxKategoriya.SelectedValue);
            string opisanie = textBoxOpisanie.Text;

            var parameters = new List<Npgsql.NpgsqlParameter>()
    {
        new Npgsql.NpgsqlParameter("@n", name),
        new Npgsql.NpgsqlParameter("@c", cena),
        new Npgsql.NpgsqlParameter("@k", kategoriya),
        new Npgsql.NpgsqlParameter("@o", opisanie)
    };

            string sql;

            if (bludoId == -1)
            {
                sql = @"
        INSERT INTO bludo
        (nazvanie, cena, kategoriya_id, opisanie)
        VALUES
        (@n, @c, @k, @o)
        RETURNING bludo_id
        ";

                var dt = Db.GetData(sql, parameters.ToArray());
                bludoId = Convert.ToInt32(dt.Rows[0][0]);
            }
            else
            {
                sql = @"
        UPDATE bludo
        SET nazvanie=@n,
            cena=@c,
            kategoriya_id=@k,
            opisanie=@o
        WHERE bludo_id=@id
        ";

                parameters.Add(new Npgsql.NpgsqlParameter("@id", bludoId));

                Db.ekzekuttranzakcii(sql, parameters.ToArray());
            }

            MessageBox.Show("Блюдо сохранено");
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (bludoId == -1)
            {
                MessageBox.Show("Сначала сохраните блюдо, чтобы добавить ингредиенты.");
                return;
            }
            SostavBludaForm sostavBludaForm = new SostavBludaForm(bludoId);
            sostavBludaForm.ShowDialog();
        }
    }


}
