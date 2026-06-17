// Форма создания резервной копии и восстановления базы PostgreSQL
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
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

            // Ищем pg_dump на UI-потоке (чтобы диалог выбора мог показаться)
            string pgDumpPath;
            try
            {
                pgDumpPath = Puti_dlya_rk.GetPgDump();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
                return;
            }

            try
            {
                // pg_dump в пользовательском формате (-F c) с блобами (-b)
                var output = RunProcess(pgDumpPath,
                    $"-h localhost -p 5432 -U {user} -F c -b -v -f \"{dialog.FileName}\" {dbName}",
                    password, 60000);

                if (output.ExitCode != 0)
                {
                    MessageBox.Show("Ошибка создания резервной копии:\n" + output.Error);
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

            // Ищем утилиты на UI-потоке — чтобы BrowseForExe мог показать диалог
            string pgRestorePath, psqlPath;
            try
            {
                pgRestorePath = Puti_dlya_rk.GetPgRestore();
                psqlPath = Puti_dlya_rk.GetPsql();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                progressBar1.Visible = true;
                labelStatus.Text = "Подготовка к восстановлению...";

                // Запускаем фоновое восстановление (только работу с процессами, без UI-диалогов)
                bool restoreSuccess = await Task.Run(() =>
                    RestoreDatabase(dialog.FileName, pgRestorePath, psqlPath));

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
        // Все пути к утилитам уже найдены на UI-потоке — здесь только работа с процессами.
        private bool RestoreDatabase(string backupFile, string pgRestorePath, string psqlPath)
        {
            try
            {
                this.Invoke(new Action(() => labelStatus.Text = "Завершение активных сессий..."));

                // Шаг 1: Принудительное завершение всех подключений к БД
                string terminateSql = $"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{dbName}' AND pid <> pg_backend_pid();";
                var termResult = RunPsql(psqlPath, terminateSql, 15000);
                if (termResult.ExitCode != 0)
                    System.Diagnostics.Debug.WriteLine("terminate sessions: " + termResult.Error);

                this.Invoke(new Action(() =>
                {
                    labelStatus.Text = "Удаление старой базы данных...";
                    progressBar1.Value = 20;
                }));

                // Шаг 2: Удаление существующей БД
                var dropResult = RunPsql(psqlPath, $"DROP DATABASE IF EXISTS \"{dbName}\";", 30000);
                if (dropResult.ExitCode != 0)
                    System.Diagnostics.Debug.WriteLine("drop db: " + dropResult.Error);

                this.Invoke(new Action(() =>
                {
                    labelStatus.Text = "Создание новой базы данных...";
                    progressBar1.Value = 40;
                }));

                // Шаг 3: Создание новой БД
                var createResult = RunPsql(psqlPath, $"CREATE DATABASE \"{dbName}\" ENCODING 'UTF8';", 30000);
                if (createResult.ExitCode != 0)
                    throw new Exception($"Ошибка создания БД: {createResult.Error}");

                this.Invoke(new Action(() =>
                {
                    labelStatus.Text = "Восстановление данных из резервной копии...";
                    progressBar1.Value = 50;
                }));

                // Шаг 4: Непосредственное восстановление через pg_restore
                var restoreResult = RunProcess(pgRestorePath,
                    $"-h localhost -p 5432 -U {user} -d {dbName} -v \"{backupFile}\"",
                    password, 120000);

                if (restoreResult.ExitCode != 0)
                    throw new Exception($"Ошибка восстановления. Код: {restoreResult.ExitCode}\n{restoreResult.Error}");

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

        // Выполнение psql с одной SQL-командой через -c, читает stderr асинхронно
        private ProcessResult RunPsql(string psqlPath, string sql, int timeoutMs)
        {
            string escaped = sql.Replace("\"", "\\\"");
            string args = $"-h localhost -p 5432 -U {user} -d postgres -c \"{escaped}\"";
            return RunProcess(psqlPath, args, password, timeoutMs);
        }

        // Безопасный запуск процесса: stderr читается асинхронно (нет deadlock),
        // есть таймаут, пароль через PGPASSWORD.
        private ProcessResult RunProcess(string exePath, string args, string pgPassword, int timeoutMs)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = exePath;
            psi.Arguments = args;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.EnvironmentVariables["PGPASSWORD"] = pgPassword;

            using (Process proc = new Process())
            {
                proc.StartInfo = psi;

                StringBuilder stdOut = new StringBuilder();
                StringBuilder stdErr = new StringBuilder();

                proc.OutputDataReceived += (s, ev) =>
                {
                    if (ev.Data != null) stdOut.AppendLine(ev.Data);
                };
                proc.ErrorDataReceived += (s, ev) =>
                {
                    if (ev.Data != null) stdErr.AppendLine(ev.Data);
                };

                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();

                // Ждём с таймаутом — если процесс завис, убьём его
                if (!proc.WaitForExit(timeoutMs))
                {
                    try { proc.Kill(); } catch { }
                    proc.WaitForExit();
                    return new ProcessResult { ExitCode = -1, Error = $"Процесс не завершился за {timeoutMs}мс и был принудительно остановлен." };
                }

                proc.CancelOutputRead();
                proc.CancelErrorRead();

                return new ProcessResult
                {
                    ExitCode = proc.ExitCode,
                    Output = stdOut.ToString(),
                    Error = stdErr.ToString()
                };
            }
        }

        // Результат выполнения внешнего процесса
        private struct ProcessResult
        {
            public int ExitCode;
            public string Output;
            public string Error;
        }
    }
}
