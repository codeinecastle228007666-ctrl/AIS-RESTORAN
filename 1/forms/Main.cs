using _1.forms;
using _1.forms.bronirovanie;
using _1.forms.Menu;
using _1.zaprosi;
using System.Diagnostics;

namespace _1
{
    public partial class Main : Form
    {
        private int roleId;
        private int userId;
        private DumpRestore dumpRestoreForm;

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

            if (Session.RoleId == 1) // Официант
            {
                button4.Enabled = false;
                button3.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button10.Enabled = false;
            }

            if (Session.RoleId == 2) // Повар
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button4.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
            }

            if (Session.RoleId == 3) // Шеф-повар
            {
                button4.Enabled = false;
                button2.Enabled = false;
                button5.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
            }

            if (Session.RoleId == 4) // Руководитель
            {
                // Все кнопки доступны
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isRestarting)
            {
                e.Cancel = false;
                return;
            }

            if (MessageBox.Show(
                "Вы уверены, что хотите выйти?",
                "Выход",
                MessageBoxButtons.YesNo
            ) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private bool _isRestarting = false;

        private void button7_Click(object sender, EventArgs e)
        {
            dumpRestoreForm = new DumpRestore();
            dumpRestoreForm.OnRestoreCompleted += DumpRestoreForm_OnRestoreCompleted;
            dumpRestoreForm.ShowDialog();
            dumpRestoreForm.OnRestoreCompleted -= DumpRestoreForm_OnRestoreCompleted;
            dumpRestoreForm.Dispose();
            dumpRestoreForm = null;
        }

        private void DumpRestoreForm_OnRestoreCompleted()
        {
            var exitResult = MessageBox.Show(
                "База данных восстановлена. Требуется перезапуск приложения.\n\n" +
                "Сохранить несохранённые данные перед выходом?",
                "Перезапуск приложения",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (exitResult == DialogResult.Yes)
            {
                SaveAllData();
            }

            _isRestarting = true;

            MessageBox.Show(
                "Приложение будет перезапущено.\n\nНажмите OK для продолжения.",
                "Перезапуск",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            RestartApplication();
        }

        private void SaveAllData()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Данные сохранены перед перезапуском");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при сохранении данных: {ex.Message}",
                    "Ошибка сохранения",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void RestartApplication()
        {
            try
            {
                string applicationPath = Application.ExecutablePath;

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = applicationPath;
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Application.StartupPath;

                Process.Start(startInfo);

                System.Threading.Thread.Sleep(500);

                Application.Exit();
            }
            catch (Exception ex)
            {
                _isRestarting = false;

                MessageBox.Show(
                    "Не удалось автоматически перезапустить приложение.\n" +
                    $"Ошибка: {ex.Message}\n\n" +
                    "Пожалуйста, перезапустите приложение вручную.",
                    "Ошибка перезапуска",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SotrudnikiForm form = new SotrudnikiForm();
            form.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            new HelpForm().ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            new Kuhnya().ShowDialog();
        }
    }
}
