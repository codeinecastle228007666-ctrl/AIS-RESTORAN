// Форма авторизации пользователя
using _1.data;
using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace _1.forms
{
    // Форма входа в систему. Проверяет логин и пароль через pgcrypto (bcrypt).
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
        }

        // Показать/скрыть пароль.
        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.PasswordChar = checkBoxShowPassword.Checked ? '\0' : '*';
        }

        // Обработчик кнопки "Войти". Выполняет проверку логина и заполняет сессию.
        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;

            // Поиск пользователя через функцию crypt() модуля pgcrypto (bcrypt)
            string sql = @"
SELECT 
u.user_id,
u.role_id,
r.nazvanie AS role_name
FROM users u
JOIN role r ON r.role_id = u.role_id
WHERE u.login = @l
AND u.password_hash = crypt(@p, u.password_hash)
";

            try
            {
                DataTable dt = Db.GetData(sql,
                    new NpgsqlParameter("@l", login),
                    new NpgsqlParameter("@p", password)
                );

                // Если запрос не вернул строк — логин/пароль неверны
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Неверный логин или пароль");
                    // Логируем неудачную попытку
                    Db.Execute(
"INSERT INTO login_log(user_id, success) VALUES(NULL, false)"
);
                    return;
                }

                // Записываем данные в Session — глобальное состояние пользователя
                Session.UserId = Convert.ToInt32(dt.Rows[0]["user_id"]);
                Session.RoleId = Convert.ToInt32(dt.Rows[0]["role_id"]);
                Session.RoleName = dt.Rows[0]["role_name"].ToString();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка авторизации:\n" + ex.Message);
            }
        }
    }
}
