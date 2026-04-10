using _1.forms;
using _1.forms.bronirovanie;
using _1.forms.Menu;
using _1.zaprosi;

namespace _1
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        int roleId;
        int userId;
        public Main(int role, int user)
        {
            InitializeComponent();
            roleId = role;
            userId = user;


        }


        private void button1_Click(object sender, EventArgs e) //кнопка открытия формы заказов
        {
            Zakazi form = new Zakazi();
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Sklad form = new Sklad();
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ZaprosiMain form = new ZaprosiMain();
            form.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BronirovanieForm form = new BronirovanieForm();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clienti form = new Clienti();
            form.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MenuForm menuForm = new MenuForm();
            menuForm.ShowDialog();
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
    }
}
