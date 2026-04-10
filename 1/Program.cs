using _1.forms;

namespace _1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += GlobalException;
            Application.Run(new Auth());
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