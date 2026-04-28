using _1.data;
using Npgsql;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace _1.forms
{
    public partial class SotrudnikiForm : Form
    {
        public SotrudnikiForm()
        {
            InitializeComponent();
            LoadSotrudniki();
        }

        private void LoadSotrudniki()
        {
            string sql = @"
        SELECT 
            s.sotrudnik_id AS ""ID"",
            s.fio AS ""ФИО"",
            d.nazvanie AS ""Должность"",
            s.nomer_telefona AS ""Телефон"",
            s.email AS ""Email"",
            s.adres AS ""Адрес"",
            s.data_rojdeniya AS ""Дата рождения""
        FROM sotrudniki s
        JOIN dolzhnost d ON s.dolzhnost_id = d.dolzhnost_id
        ORDER BY s.fio
    ";
            dataGridView1.DataSource = Db.GetData(sql);
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new SotrudnikEditForm();
            if (form.ShowDialog() == DialogResult.OK)
                LoadSotrudniki();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            var form = new SotrudnikEditForm(id);
            if (form.ShowDialog() == DialogResult.OK)
                LoadSotrudniki();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            if (MessageBox.Show("Удалить сотрудника?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            string sql = "DELETE FROM sotrudniki WHERE sotrudnik_id = @id";
            Db.Execute(sql, new NpgsqlParameter("@id", id));
            LoadSotrudniki();
        }
        // Вставь в класс SotrudnikiForm
        private string Transliterate(string text)
        {
            var translitMap = new Dictionary<char, string>
    {
        {'А',"A"}, {'Б',"B"}, {'В',"V"}, {'Г',"G"}, {'Д',"D"}, {'Е',"E"}, {'Ё',"E"},
        {'Ж',"Zh"}, {'З',"Z"}, {'И',"I"}, {'Й',"I"}, {'К',"K"}, {'Л',"L"}, {'М',"M"},
        {'Н',"N"}, {'О',"O"}, {'П',"P"}, {'Р',"R"}, {'С',"S"}, {'Т',"T"}, {'У',"U"},
        {'Ф',"F"}, {'Х',"Kh"}, {'Ц',"Ts"}, {'Ч',"Ch"}, {'Ш',"Sh"}, {'Щ',"Sch"},
        {'Ъ',""}, {'Ы',"Y"}, {'Ь',""}, {'Э',"E"}, {'Ю',"Yu"}, {'Я',"Ya"},
        {'а',"a"}, {'б',"b"}, {'в',"v"}, {'г',"g"}, {'д',"d"}, {'е',"e"}, {'ё',"e"},
        {'ж',"zh"}, {'з',"z"}, {'и',"i"}, {'й',"i"}, {'к',"k"}, {'л',"l"}, {'м',"m"},
        {'н',"n"}, {'о',"o"}, {'п',"p"}, {'р',"r"}, {'с',"s"}, {'т',"t"}, {'у',"u"},
        {'ф',"f"}, {'х',"kh"}, {'ц',"ts"}, {'ч',"ch"}, {'ш',"sh"}, {'щ',"sch"},
        {'ъ',""}, {'ы',"y"}, {'ь',""}, {'э',"e"}, {'ю',"yu"}, {'я',"ya"}
    };
            StringBuilder sb = new StringBuilder();
            foreach (char c in text)
                if (translitMap.ContainsKey(c))
                    sb.Append(translitMap[c]);
                else
                    sb.Append(c);
            return sb.ToString();
        }

        private string GenerateLogin(string fio)
        {
            string[] parts = fio.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                string firstName = parts[1]; // предполагаем: Фамилия Имя Отчество
                string lastName = parts[0];
                // Логин = первая буква имени + фамилия
                string login = Transliterate(firstName[0] + lastName).ToLower();
                // Убираем апострофы и пробелы
                login = new string(login.Where(c => char.IsLetterOrDigit(c)).ToArray());
                return login;
            }
            return "user";
        }

        private string GeneratePassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void buttonCreateUser_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите сотрудника.");
                return;
            }

            int sotrudnikId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            string fio = dataGridView1.CurrentRow.Cells["ФИО"].Value.ToString();

            // Проверим, нет ли уже учётной записи (опционально)

            // Выбор роли
            var roleForm = new SelectRoleForm();
            if (roleForm.ShowDialog() != DialogResult.OK)
                return;
            int roleId = roleForm.SelectedRoleId;

            string login = GenerateLogin(fio);
            string password = GeneratePassword();

            // Хеширование пароля с помощью pgcrypto (через SQL)
            string hashSql = "SELECT crypt(@p, gen_salt('bf', 6))";
            DataTable hashDt = Db.GetData(hashSql, new NpgsqlParameter("@p", password));
            string passwordHash = hashDt.Rows[0][0].ToString();

            string insertSql = "INSERT INTO users (login, password_hash, role_id) VALUES (@login, @hash, @role)";
            try
            {
                Db.Execute(insertSql,
                    new NpgsqlParameter("@login", login),
                    new NpgsqlParameter("@hash", passwordHash),
                    new NpgsqlParameter("@role", roleId));
                MessageBox.Show($"Учётная запись создана.\nЛогин: {login}\nПароль: {password}\n\nСохраните пароль!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void buttonSalary_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите сотрудника.");
                return;
            }

            int sotrudnikId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            string fio = dataGridView1.CurrentRow.Cells["ФИО"].Value.ToString();
            string dolzhnost = dataGridView1.CurrentRow.Cells["Должность"].Value.ToString();

            var form = new SalaryForm(sotrudnikId, fio, dolzhnost);
            form.ShowDialog();
        }
    }
}