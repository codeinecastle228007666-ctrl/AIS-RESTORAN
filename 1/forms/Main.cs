using _1.forms;
using _1.forms.bronirovanie;
using _1.forms.Menu;
using _1.zaprosi;

namespace _1
{
    public partial class Main : Form
    {
        private int roleId;
        private int userId;

        // Конструктор с параметрами
        public Main(int role, int user)
        {
            InitializeComponent();
            roleId = role;
            userId = user;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Zakazi form = new Zakazi();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка открытия формы:\n" + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Sklad form = new Sklad();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка открытия формы:\n" + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                ZaprosiMain form = new ZaprosiMain();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка открытия формы:\n" + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                BronirovanieForm form = new BronirovanieForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка открытия формы:\n" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Clienti form = new Clienti();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка открытия формы:\n" + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                MenuForm menuForm = new MenuForm();
                menuForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка открытия формы:\n" + ex.Message);
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            labelRole.Text = Session.RoleName;

            if (Session.RoleId == 1) // официант
            {
                button4.Enabled = false;
                button3.Enabled = false;
            }

            if (Session.RoleId == 2) // повар
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button4.Enabled = false;
            }

            if (Session.RoleId == 3) // шеф
            {
                button4.Enabled = false;
                button2.Enabled = false;
                button5.Enabled = false;
            }

            if (Session.RoleId == 4) // руководитель
            {
                // доступ ко всему
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(
                "Вы уверены, что хотите выйти?",
                "Выход",
                MessageBoxButtons.YesNo
            ) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}