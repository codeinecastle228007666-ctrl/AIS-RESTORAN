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
        SELECT user_id, role_id, role_name
        FROM auth_user(@login,@password)";

            try
            {
                DataTable dt = Db.GetData(sql,
                    new NpgsqlParameter("@login", login),
                    new NpgsqlParameter("@password", password)
                );

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Неверный логин или пароль");
                    return;
                }

                // Сохраняем данные в сессию
                Session.UserId = Convert.ToInt32(dt.Rows[0]["user_id"]);
                Session.RoleId = Convert.ToInt32(dt.Rows[0]["role_id"]);
                Session.RoleName = dt.Rows[0]["role_name"].ToString();

                // Закрываем форму авторизации с успешным результатом
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