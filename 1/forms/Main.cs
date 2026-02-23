using _1.forms;
using _1.zaprosi;

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

        private void button4_Click(object sender, EventArgs e)
        {
            ZaprosiMain form = new ZaprosiMain();
            form.ShowDialog();
        }
    }
}
