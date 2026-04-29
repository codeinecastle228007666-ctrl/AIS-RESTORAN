using System;
using System.Data;
using System.Windows.Forms;
using _1.data;
using Npgsql;

namespace _1.forms
{
    public partial class redzakazaform : Form
    {
        public redzakazaform()
        {
            InitializeComponent();
        }

        private void redzakazaform_Load(object sender, EventArgs e)
        {
            LoadCombo(comboBoxClient, "client_id", "fio", "SELECT client_id, fio FROM client ORDER BY fio");
            LoadCombo(comboBoxStol, "stol_id", "nomer", "SELECT stol_id, nomer FROM stol ORDER BY nomer");
            LoadCombo(comboBoxSotrudnik, "sotrudnik_id", "fio", "SELECT sotrudnik_id, fio FROM sotrudniki ORDER BY fio");

            dateTimePicker1.Value = DateTime.Now;
            comboBoxStatus.Enabled = false;
            comboBoxStatus.Text = "Новый";
        }

        private void LoadCombo(ComboBox cb, string valueMember, string displayMember, string sql)
        {
            DataTable dt = Db.GetData(sql);
            cb.DisplayMember = displayMember;
            cb.ValueMember = valueMember;
            cb.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxClient.SelectedItem is not DataRowView drvClient ||
                comboBoxStol.SelectedItem is not DataRowView drvStol ||
                comboBoxSotrudnik.SelectedItem is not DataRowView drvSotrudnik)
            {
                MessageBox.Show("Пожалуйста, выберите клиента, стол и сотрудника.");
                return;
            }

            int clientId = Convert.ToInt32(drvClient["client_id"]);
            int stolId = Convert.ToInt32(drvStol["stol_id"]);
            int sotrudnikId = Convert.ToInt32(drvSotrudnik["sotrudnik_id"]);

            try
            {
                var dt = Db.GetData("SELECT * FROM sp_create_zakaz(@p_client_id, @p_stol_id, @p_sotrudnik_id)",
                    new NpgsqlParameter("@p_client_id", clientId),
                    new NpgsqlParameter("@p_stol_id", stolId),
                    new NpgsqlParameter("@p_sotrudnik_id", sotrudnikId)
                );

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Заказ не был создан.");
                    return;
                }

                int newZakazId = Convert.ToInt32(dt.Rows[0]["p_zakaz_id"]);
                MessageBox.Show($"Заказ #{newZakazId} успешно создан");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка создания заказа:\n" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
