using _1.data;
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

            using (var auth = new Auth())
            {
                if (auth.ShowDialog() == DialogResult.OK)
                {
                    Db.OpenSession();
                    Session.ApplyToDb();

                    Application.Run(new Main(Session.RoleId, Session.UserId));

                    Db.CloseSession();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

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
