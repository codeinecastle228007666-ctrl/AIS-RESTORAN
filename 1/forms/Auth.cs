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
            string sql = @"
            SELECT user_id, role_id
            FROM users
            WHERE login = @l AND password = @p
            ";

            DataTable dt = Db.GetData(sql,
                new NpgsqlParameter("@l", textBoxLogin.Text),
                new NpgsqlParameter("@p", textBoxPassword.Text)
            );

            if (dt.Rows.Count == 1)
            {
                int roleId = Convert.ToInt32(dt.Rows[0]["role_id"]);
                int userId = Convert.ToInt32(dt.Rows[0]["user_id"]);

                Main main = new Main(roleId,userId);
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
