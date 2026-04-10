using _1.data;
using _1.forms.Menu;
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
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;

            string sql = @"
        SELECT * 
        FROM auth_user(@login,@password)";

            DataTable dt = Db.GetData(sql,
                new NpgsqlParameter("@login", login),
                new NpgsqlParameter("@password", password)
            );

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            Session.UserId = Convert.ToInt32(dt.Rows[0]["user_id"]);
            Session.RoleId = Convert.ToInt32(dt.Rows[0]["role_id"]);
            Session.RoleName = dt.Rows[0]["role_name"].ToString();

            Main m = new Main();
            m.Show();

            this.Hide();
        }
    }
}
