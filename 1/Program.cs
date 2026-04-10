using _1.forms;

namespace _1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += GlobalException;

            // Показываем форму авторизации
            using (var auth = new Auth())
            {
                if (auth.ShowDialog() == DialogResult.OK)
                {
                    // Если авторизация успешна, запускаем главную форму
                    Application.Run(new Main(Session.RoleId, Session.UserId));
                }
                else
                {
                    // Если авторизация не успешна или закрыта, завершаем приложение
                    Application.Exit();
                }
            }
        }

        static void GlobalException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(
                "Произошла ошибка:\n" + e.Exception.Message,
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}