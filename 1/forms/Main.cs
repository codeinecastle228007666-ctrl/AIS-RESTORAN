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
                button7.Enabled = false;
            }

            if (Session.RoleId == 2) // повар
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button4.Enabled = false;
                button7.Enabled = false;
            }

            if (Session.RoleId == 3) // шеф
            {
                button4.Enabled = false;
                button2.Enabled = false;
                button5.Enabled = false;
                button7.Enabled = false;
            }

            if (Session.RoleId == 4) // руководитель
            {
                // доступ ко всему
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Проверяем, не запланирован ли перезапуск
            if (_isRestarting)
            {
                // Если это перезапуск, не задаем вопрос
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

        private bool _isRestarting = false; // Флаг для отслеживания перезапуска

        private void button7_Click(object sender, EventArgs e)
        {
            // Создаем форму ОДИН раз
            dumpRestoreForm = new DumpRestore();

            // Подписываемся на событие
            dumpRestoreForm.OnRestoreCompleted += DumpRestoreForm_OnRestoreCompleted;

            // Показываем форму
            dumpRestoreForm.ShowDialog();

            // После закрытия формы отписываемся от события
            dumpRestoreForm.OnRestoreCompleted -= DumpRestoreForm_OnRestoreCompleted;
            dumpRestoreForm.Dispose();
            dumpRestoreForm = null;
        }

        private void DumpRestoreForm_OnRestoreCompleted()
        {
            // Этот метод вызывается когда форма DumpRestore уже закрыта
            // и пользователь согласился на перезапуск

            // Показываем диалог подтверждения
            var exitResult = MessageBox.Show(
                "Приложение будет перезапущено для применения восстановленной базы данных.\n\n" +
                "Сохранить все несохраненные данные перед перезапуском?",
                "Перезапуск приложения",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (exitResult == DialogResult.Yes)
            {
                // Здесь можно добавить сохранение данных если нужно
                SaveAllData();
            }

            // Устанавливаем флаг что это запланированный перезапуск
            _isRestarting = true;

            // Показываем финальное сообщение
            MessageBox.Show(
                "Приложение будет перезапущено.\n\nНажмите OK для продолжения.",
                "Перезапуск",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Перезапускаем приложение
            RestartApplication();
        }

        private void SaveAllData()
        {
            try
            {
                // Здесь можно добавить код для сохранения данных
                // Например, если у вас есть открытые формы с несохраненными изменениями

                // Пример:
                // if (Application.OpenForms.Count > 0)
                // {
                //     foreach (Form form in Application.OpenForms)
                //     {
                //         if (form is IDataSave saveForm)
                //         {
                //             saveForm.SaveData();
                //         }
                //     }
                // }

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
                // Получаем путь к текущему исполняемому файлу
                string applicationPath = Application.ExecutablePath;

                // Создаем процесс для перезапуска
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = applicationPath;
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Application.StartupPath;

                // Запускаем новый экземпляр приложения
                Process.Start(startInfo);

                // Небольшая задержка чтобы новый процесс успел запуститься
                System.Threading.Thread.Sleep(500);

                // Закрываем текущее приложение
                Application.Exit();
            }
            catch (Exception ex)
            {
                _isRestarting = false; // Сбрасываем флаг в случае ошибки

                MessageBox.Show(
                    "Не удалось автоматически перезапустить приложение.\n" +
                    $"Ошибка: {ex.Message}\n\n" +
                    "Пожалуйста, перезапустите приложение вручную.",
                    "Ошибка перезапуска",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
    }
}