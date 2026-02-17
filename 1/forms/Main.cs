using _1.forms;

namespace _1
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
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
    }
}
