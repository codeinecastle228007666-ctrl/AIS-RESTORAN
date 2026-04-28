using System;
using System.Data;
using System.Windows.Forms;
using _1.data;

namespace _1.forms
{
    public partial class SelectRoleForm : Form
    {
        public int SelectedRoleId { get; private set; } = -1;

        public SelectRoleForm()
        {
            InitializeComponent();
            LoadRoles();
        }

        private void LoadRoles()
        {
            string sql = "SELECT role_id, nazvanie FROM role ORDER BY nazvanie";
            DataTable dt = Db.GetData(sql);
            comboBoxRole.DataSource = dt;
            comboBoxRole.DisplayMember = "nazvanie";
            comboBoxRole.ValueMember = "role_id";
            if (dt.Rows.Count > 0)
                comboBoxRole.SelectedIndex = 0;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (comboBoxRole.SelectedValue == null || comboBoxRole.SelectedValue == DBNull.Value)
            {
                MessageBox.Show("Выберите роль.");
                return;
            }
            SelectedRoleId = Convert.ToInt32(comboBoxRole.SelectedValue);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}