// Точка входа в приложение АСУ "Ресторан"
using _1.data;
using _1.forms;

namespace _1
{
    // Главный класс приложения. Запускает форму авторизации и главное окно.
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Глобальный обработчик необработанных исключений в UI-потоке
            Application.ThreadException += GlobalException;

            // Показываем окно авторизации; если успех — открываем главное окно
            using (var auth = new Auth())
            {
                if (auth.ShowDialog() == DialogResult.OK)
                {
                    // Открываем сессию БД и устанавливаем контекст пользователя
                    Db.OpenSession();
                    Session.ApplyToDb();

                    // Запускаем главное окно с параметрами роли и пользователя
                    Application.Run(new Main(Session.RoleId, Session.UserId));

                    // Закрываем сессию БД при выходе
                    Db.CloseSession();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        // Обработчик необработанных исключений. Показывает сообщение об ошибке.
        static void GlobalException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(
                "Необработанная ошибка:\n" + e.Exception.Message,
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
