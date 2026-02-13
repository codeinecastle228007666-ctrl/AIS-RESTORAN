using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _1.data;

namespace _1.forms
{
    public partial class redzakazaform : Form
    {
        public redzakazaform()
        {
            InitializeComponent();
        }

        private void redzakazaform_Load(object sender, EventArgs e) //загрузка данных при открытии формы
        {
            comboBoxClient.DataSource = Db.GetData("SELECT client_id, fio FROM client"); //заполнение комбобокса данными из таблицы client, отображая фамилию клиента, а в качестве значения использовать client_id
            comboBoxClient.DisplayMember = "fio";
            comboBoxClient.ValueMember = "client_id";


            comboBoxStol.DataSource = Db.GetData("SELECT stol_id, nomer FROM stol"); //заполнение комбобокса данными из таблицы stol, отображая номер стола, а в качестве значения использовать stol_id
            comboBoxStol.DisplayMember = "nomer";
            comboBoxStol.ValueMember = "stol_id";

            comboBoxSotrudnik.DataSource = Db.GetData("SELECT sotrudnik_id, fio FROM sotrudniki"); //заполнение комбобокса данными из таблицы sotrudnik, отображая фамилию сотрудника, а в качестве значения использовать sotrudnik_id
            comboBoxSotrudnik.DisplayMember = "fio";
            comboBoxSotrudnik.ValueMember = "sotrudnik_id";

            comboBoxStatus.DataSource = Db.GetData("SELECT status_zakaza_id, nazvanie FROM status_zakaza"); //заполнение комбобокса данными из таблицы status_zakaza, отображая название статуса заказа, а в качестве значения использовать status_zakaza_id
            comboBoxStatus.DisplayMember = "nazvanie";
            comboBoxStatus.ValueMember = "status_zakaza_id";

            dateTimePicker1.Value = DateTime.Now;

        }

        private void button1_Click(object sender, EventArgs e) //кнопка сохранения данных, которая собирает все выбранные значения из комбобоксов и дату из DateTimePicker, формирует SQL запрос на вставку нового заказа в таблицу zakazi и выполняет его через метод ekzekuttranzakcii
        {
            int clientId = Convert.ToInt32(comboBoxClient.SelectedValue);
            int stolId = Convert.ToInt32(comboBoxStol.SelectedValue);
            int sotrudnikId = Convert.ToInt32(comboBoxSotrudnik.SelectedValue);
            int statusZakazaId = Convert.ToInt32(comboBoxStatus.SelectedValue);
            DateTime date = dateTimePicker1.Value;

            string formattedDate = date.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = $@"
                INSERT INTO zakazi (client_id, stol_id, sotrudnik_id, data_zakaza, status_zakaza_id)
                VALUES ({clientId}, {stolId}, {sotrudnikId}, '{formattedDate}', {statusZakazaId})
            ";

            Db.ekzekuttranzakcii(sql);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
        
}