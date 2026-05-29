// Форма создания/редактирования блюда
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
    // Форма создания или редактирования блюда, а также управления его составом.
    // Кнопки: "Сохранить" (button1), "Отмена" (button2), "Состав" (button3 - открывает SostavBludaForm)
    public partial class BludoEditForm : Form
    {
        int bludoId = -1; // -1 = новое блюдо

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
            AcceptButton = button1;
            CancelButton = button2;
        }

        // Загрузка категорий меню (без параметров — статический справочник, пользовательский ввод отсутствует).
        void LoadKategoriyaMenu()
        {
            string sql = "SELECT kategoriya_id, nazvanie FROM kategoriya_menu ORDER BY nazvanie";
            DataTable dt = Db.GetData(sql);
            comboBoxKategoriya.DataSource = dt;
            comboBoxKategoriya.DisplayMember = "nazvanie";
            comboBoxKategoriya.ValueMember = "kategoriya_id";
        }

        // Загрузка данных блюда для редактирования.
        // @id — параметр, исключает SQL-инъекцию через id блюда.
        void LoadBludo()
        {
            string sql = @"
                SELECT *
                FROM bludo
                WHERE bludo_id = @id";

            var table = Db.GetData(sql, new Npgsql.NpgsqlParameter("@id", bludoId));
            if (table.Rows.Count == 0) return;
            var row = table.Rows[0];

            textBoxName.Text = row["nazvanie"].ToString();
            textBoxCena.Text = row["cena"].ToString();
            textBoxOpisanie.Text = row["opisanie"].ToString();
            comboBoxKategoriya.SelectedValue = row["kategoriya_id"];
        }

        // Сохранение блюда (вставка с RETURNING для получения ID).
        private void buttonSave_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите название блюда.");
                return;
            }
            if (!decimal.TryParse(textBoxCena.Text, out decimal cena) || cena <= 0)
            {
                MessageBox.Show("Введите корректную цену.");
                return;
            }
            if (comboBoxKategoriya.SelectedValue == null || comboBoxKategoriya.SelectedValue == DBNull.Value)
            {
                MessageBox.Show("Выберите категорию.");
                return;
            }
            int kategoriya = Convert.ToInt32(comboBoxKategoriya.SelectedValue);
            string opisanie = textBoxOpisanie.Text;

            var parameters = new List<Npgsql.NpgsqlParameter>()
            {
                new Npgsql.NpgsqlParameter("@n", name),
                new Npgsql.NpgsqlParameter("@c", cena),
                new Npgsql.NpgsqlParameter("@k", kategoriya),
                new Npgsql.NpgsqlParameter("@o", opisanie)
            };

            if (bludoId == -1)
            {
                // Вставка с RETURNING bludo_id — получение ID одной операцией (без второго SELECT).
                // Параметры @n, @c, @k, @o — все через NpgsqlParameter.
                string sql = @"
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
                string sql = @"
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
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Вызов формы управления составом блюда (ингредиентами).
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
