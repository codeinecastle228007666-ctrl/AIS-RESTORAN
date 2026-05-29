// Форма создания резервной копии и восстановления базы PostgreSQL
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1.forms
{
    // Форма для создания резервной копии (pg_dump) и восстановления (pg_restore) БД.
    public partial class DumpRestore : Form
    {
        string dbName = "cursed_zxc_V2";
        string user = "postgres";
        string password = "1234";

        public DumpRestore()
        {
            InitializeComponent();
        }

        // Создание резервной копии через pg_dump.
        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Backup files (*.backup)|*.backup";
            dialog.FileName = "restaurant_backup_" + DateTime.Now.ToString("yyyyMMdd_HHmm");

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                string pgDumpPath = Puti_dlya_rk.GetPgDump();

                // pg_dump в пользовательском формате (-F c) с блобами (-b)
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = pgDumpPath;
                psi.Arguments = $"-h localhost -p 5432 -U {user} -F c -b -v -f \"{dialog.FileName}\" {dbName}";
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardError = true;
                psi.RedirectStandardOutput = true;
                psi.EnvironmentVariables["PGPASSWORD"] = password;

                Process process = Process.Start(psi);
                process.WaitForExit();

                string error = process.StandardError.ReadToEnd();

                if (process.ExitCode != 0)
                {
                    MessageBox.Show("Ошибка создания резервной копии:\n" + error);
                    return;
                }

                MessageBox.Show("Резервная копия успешно создана");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка:\n" + ex.Message);
            }
        }

        // Восстановление БД из резервной копии.
        private async void button2_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Backup files (*.backup)|*.backup";

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            var result = MessageBox.Show(
                "Восстановление базы данных удалит все текущие данные.\n" +
                "Перед восстановлением рекомендуется создать резервную копию.\n\n" +
                "Продолжить?",
                "Подтверждение восстановления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                progressBar1.Visible = true;
                labelStatus.Text = "Подготовка к восстановлению...";

                // Запускаем фоновое восстановление, чтобы UI не завис
                bool restoreSuccess = await Task.Run(() => RestoreDatabase(dialog.FileName));

                if (restoreSuccess)
                {
                    progressBar1.Value = 100;
                    labelStatus.Text = "Восстановление завершено";

                    var restartResult = MessageBox.Show(
                        "База данных успешно восстановлена!\n\n" +
                        "Для применения изменений необходимо перезапустить приложение.\n" +
                        "Перезапустить сейчас?",
                        "Восстановление завершено",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information);

                    if (restartResult == DialogResult.Yes)
                    {
                        this.Close();
                        OnRestoreCompleted?.Invoke();
                    }
                    else
                    {
                        MessageBox.Show(
                        "Внимание! Некоторые данные могут быть не сохранены на диске.\n" +
                        "Рекомендуется перезапустить приложение вручную.",
                        "Восстановление",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                progressBar1.Visible = false;
                Cursor = Cursors.Default;
                labelStatus.Text = "Ошибка восстановления";

                MessageBox.Show("Ошибка восстановления:\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                progressBar1.Visible = false;
                Cursor = Cursors.Default;
            }
        }

        // Событие, вызываемое после восстановления для перезапуска приложения.
        public event Action OnRestoreCompleted;

        // Основной метод восстановления: завершение сессий, удаление/создание БД, pg_restore.
        private bool RestoreDatabase(string backupFile)
        {
            try
            {
                string pgRestorePath = Puti_dlya_rk.GetPgRestore();
                string psqlPath = Puti_dlya_rk.GetPsql();

                if (!File.Exists(pgRestorePath))
                    throw new Exception($"pg_restore не найден по пути: {pgRestorePath}");
                if (!File.Exists(psqlPath))
                    throw new Exception($"psql не найден по пути: {psqlPath}");

                this.Invoke(new Action(() => labelStatus.Text = "Завершение активных сессий..."));

                // Шаг 1: Принудительное завершение всех подключений к БД
                string terminateCommand = $"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{dbName}' AND pid <> pg_backend_pid();";
                ExecutePsqlCommand(terminateCommand);

                this.Invoke(new Action(() =>
                {
                    labelStatus.Text = "Удаление старой базы данных...";
                    progressBar1.Value = 20;
                }));

                // Шаг 2: Удаление существующей БД
                string dropCommand = $"DROP DATABASE IF EXISTS \"{dbName}\";";
                ExecutePsqlCommand(dropCommand);

                this.Invoke(new Action(() =>
                {
                    labelStatus.Text = "Создание новой базы данных...";
                    progressBar1.Value = 40;
                }));

                // Шаг 3: Создание новой БД
                string createCommand = $"CREATE DATABASE \"{dbName}\" ENCODING 'UTF8';";
                ExecutePsqlCommand(createCommand);

                this.Invoke(new Action(() =>
                {
                    labelStatus.Text = "Восстановление данных из резервной копии...";
                    progressBar1.Value = 50;
                }));

                // Шаг 4: Непосредственное восстановление через pg_restore
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = pgRestorePath;
                psi.Arguments = $"-h localhost -p 5432 -U {user} -d {dbName} -v \"{backupFile}\"";
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardError = true;
                psi.RedirectStandardOutput = true;
                psi.EnvironmentVariables["PGPASSWORD"] = password;

                using (Process restore = new Process())
                {
                    restore.StartInfo = psi;

                    StringBuilder outputBuilder = new StringBuilder();
                    StringBuilder errorBuilder = new StringBuilder();

                    restore.OutputDataReceived += (s, ev) =>
                    {
                        if (ev.Data != null)
                        {
                            outputBuilder.AppendLine(ev.Data);
                            this.Invoke(new Action(() =>
                            {
                                if (progressBar1.Value < 90)
                                    progressBar1.Value += 2;
                                labelStatus.Text = "Восстановление данных...";
                            }));
                        }
                    };

                    restore.ErrorDataReceived += (s, ev) =>
                    {
                        if (ev.Data != null)
                        {
                            errorBuilder.AppendLine(ev.Data);
                            System.Diagnostics.Debug.WriteLine($"RESTORE: {ev.Data}");
                        }
                    };

                    restore.Start();
                    restore.BeginOutputReadLine();
                    restore.BeginErrorReadLine();
                    restore.WaitForExit();

                    restore.CancelOutputRead();
                    restore.CancelErrorRead();

                    if (restore.ExitCode != 0)
                    {
                        string errorMessage = errorBuilder.ToString();
                        throw new Exception($"Ошибка восстановления файла. Код: {restore.ExitCode}\nОшибка: {errorMessage}");
                    }
                }

                this.Invoke(new Action(() =>
                {
                    labelStatus.Text = "Восстановление завершено успешно!";
                    progressBar1.Value = 100;
                }));

                return true;
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() =>
                {
                    labelStatus.Text = "Ошибка: " + ex.Message;
                }));
                throw;
            }
        }

        // Выполнение SQL-команды через psql.exe (для администрирования БД).
        private void ExecutePsqlCommand(string sqlCommand)
        {
            string psqlPath = Puti_dlya_rk.GetPsql();

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = psqlPath;

            string escapedCommand = sqlCommand.Replace("\"", "\\\"");
            psi.Arguments = $"-h localhost -p 5432 -U {user} -d postgres -c \"{escapedCommand}\"";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardError = true;
            psi.EnvironmentVariables["PGPASSWORD"] = password;

            using (Process proc = Process.Start(psi))
            {
                proc.WaitForExit();

                if (proc.ExitCode != 0)
                {
                    string error = proc.StandardError.ReadToEnd();
                    throw new Exception($"Ошибка выполнения SQL команды: {error}\nКоманда: {sqlCommand}");
                }
            }
        }
    }
}
