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

namespace _1.forms.clients
{
    public partial class clientsRedForm : Form
    {
        int clientId = -1;
        public clientsRedForm()
        {
            InitializeComponent();
        }

        
        public clientsRedForm(int id)
        {
            InitializeComponent();
            clientId = id;
            LoadClient();
        }

        void LoadClient()
        {
            string sql = @"
            SELECT 
                fio,
                nomer_telefona
            FROM client
            WHERE client_id = @id
                ";
            using (var con = Db.GetConnection())
            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", clientId);

                con.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        textBoxFIO.Text = reader["fio"].ToString();
                        maskedTextBox1.Text = reader["nomer_telefona"].ToString();
                    }
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string fio = textBoxFIO.Text;
            string phone = maskedTextBox1.Text;

            string sql;


            if (string.IsNullOrWhiteSpace(textBoxFIO.Text))
            {
                MessageBox.Show("Пожалуйста, введите ФИО клиента.");
                return;
            }


            if (!maskedTextBox1.MaskCompleted)
            {
                MessageBox.Show("Введите номер телефона");
                return;
            }

            if (clientId == -1)
            {
                sql = @"
        INSERT INTO client (fio, nomer_telefona)
        VALUES (@fio, @phone)";
            }
            else
            {
                sql = @"
        UPDATE client
        SET fio=@fio, nomer_telefona=@phone
        WHERE client_id=@id";
            }

            var parameters = new List<NpgsqlParameter>
    {
        new NpgsqlParameter("@fio", fio),
        new NpgsqlParameter("@phone", phone)
    };

            if (clientId != -1)
                parameters.Add(new NpgsqlParameter("@id", clientId));

            Db.ekzekuttranzakcii(sql, parameters.ToArray());

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
