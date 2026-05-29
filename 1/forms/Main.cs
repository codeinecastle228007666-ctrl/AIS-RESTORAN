// Главная навигационная форма приложения
using _1.forms;
using _1.forms.bronirovanie;
using _1.forms.Menu;
using _1.zaprosi;
using System.Diagnostics;

namespace _1
{
    // Главное меню приложения. Содержит кнопки для перехода ко всем формам.
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

        // Кнопка "Заказы".
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

        // Кнопка "Склад".
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

        // Кнопка "Финансы".
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Fin form = new Fin();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка открытия формы:\n" + ex.Message);
            }
        }

        // Кнопка "Бронирование".
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

        // Кнопка "Клиенты".
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

        // Кнопка "Меню".
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                MenuForm form = new MenuForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка открытия формы:\n" + ex.Message);
            }
        }

        // Загрузка формы. Настройка доступа в зависимости от роли пользователя.
        private void Main_Load(object sender, EventArgs e)
        {
            labelRole.Text = Session.RoleName;

            // Роль 1 - Официант: только заказы, меню, залы, бронирование, помощь
            if (Session.RoleId == 1)
            {
                button4.Enabled = false; // Финансы
                button3.Enabled = false; // Склад
                button7.Enabled = false; // backup
                button8.Enabled = false; // Сотрудники
                button10.Enabled = false; // Кухня
            }

            // Роль 2 - Повар: только кухня БД
            if (Session.RoleId == 2)
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button4.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
            }

            // Роль 3 - Админ-зал: только заказы, клиенты, меню, залы, бронирование
            if (Session.RoleId == 3)
            {
                button4.Enabled = false;
                button2.Enabled = false;
                button5.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
            }

            // Роль 4 - Администратор: все разделы доступны
        }

        // При закрытии проверка намерения (кроме случая после restore).
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

        // Открытие формы создания резервной копии/восстановления БД.
        private void button7_Click(object sender, EventArgs e)
        {
            dumpRestoreForm = new DumpRestore();
            dumpRestoreForm.OnRestoreCompleted += DumpRestoreForm_OnRestoreCompleted;
            dumpRestoreForm.ShowDialog();
            dumpRestoreForm.OnRestoreCompleted -= DumpRestoreForm_OnRestoreCompleted;
            dumpRestoreForm.Dispose();
            dumpRestoreForm = null;
        }

        // Обработчик завершения восстановления: перезапуск приложения после рестора.
        private void DumpRestoreForm_OnRestoreCompleted()
        {
            var exitResult = MessageBox.Show(
                "База данных восстановлена. Приложение будет перезапущено.\n\n" +
                "Желаете сохранить текущие данные перед выходом?",
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

        // Сохранение текущих данных приложения (заглушка).
        private void SaveAllData()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Начало сохранения данных приложения");
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

        // Перезапуск приложения через запуск нового процесса.
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
                    "Не удалось перезапустить приложение.\n" +
                    $"Ошибка: {ex.Message}\n\n" +
                    "Пожалуйста, перезапустите приложение вручную.",
                    "Ошибка перезапуска",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // Кнопка "Сотрудники".
        private void button8_Click(object sender, EventArgs e)
        {
            SotrudnikiForm form = new SotrudnikiForm();
            form.ShowDialog();
        }

        // Вызов справки.
        private void button9_Click(object sender, EventArgs e)
        {
            new HelpForm().ShowDialog();
        }

        // Кнопка "Кухня".
        private void button10_Click(object sender, EventArgs e)
        {
            new Kuhnya().ShowDialog();
        }
    }
}
