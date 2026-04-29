using _1.data;
using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace _1.forms
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;

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

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Неверный логин или пароль");
                    Db.Execute(
"INSERT INTO login_log(user_id, success) VALUES(NULL, false)"
);
                    return;
                }

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