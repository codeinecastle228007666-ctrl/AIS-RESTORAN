// Класс путей PostgreSQL (pg_dump, pg_restore, psql) на диске
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace _1.forms
{
    // Статический класс для поиска исполняемых файлов PostgreSQL.
    // Порядок поиска: PATH → сканирование Program Files на всех дисках → ручной выбор через диалог.
    internal class Puti_dlya_rk
    {
        // Возвращает путь к pg_dump.exe.
        public static string GetPgDump()
        {
            string path = FindInPath("pg_dump");
            if (path != null) return path;

            path = ScanAllDrives("pg_dump.exe");
            if (path != null) return path;

            path = BrowseForExe("pg_dump.exe", "Выберите pg_dump.exe");
            if (path != null) return path;

            throw new Exception("pg_dump не найден. Установите PostgreSQL или укажите путь вручную.");
        }

        // Возвращает путь к pg_restore.exe.
        public static string GetPgRestore()
        {
            string path = FindInPath("pg_restore");
            if (path != null) return path;

            path = ScanAllDrives("pg_restore.exe");
            if (path != null) return path;

            path = BrowseForExe("pg_restore.exe", "Выберите pg_restore.exe");
            if (path != null) return path;

            throw new Exception("pg_restore не найден. Установите PostgreSQL или укажите путь вручную.");
        }

        // Возвращает путь к psql.exe.
        public static string GetPsql()
        {
            string path = FindInPath("psql");
            if (path != null) return path;

            path = ScanAllDrives("psql.exe");
            if (path != null) return path;

            path = BrowseForExe("psql.exe", "Выберите psql.exe");
            if (path != null) return path;

            throw new Exception("psql не найден. Установите PostgreSQL или укажите путь вручную.");
        }

        // Поиск через where.exe (проверяет PATH — сработает, если PostgreSQL прописан в переменных среды)
        private static string FindInPath(string exeName)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("where.exe", exeName);
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardOutput = true;
                using (Process p = Process.Start(psi))
                {
                    string output = p.StandardOutput.ReadLine();
                    p.WaitForExit();
                    if (p.ExitCode == 0 && !string.IsNullOrEmpty(output) && File.Exists(output))
                    {
                        System.Diagnostics.Debug.WriteLine($"{exeName} найден в PATH: {output}");
                        return output;
                    }
                }
            }
            catch { }
            return null;
        }

        // Поиск на всех дисках: Program Files\PostgreSQL\версия\bin\ и pgAdmin runtime
        private static string ScanAllDrives(string fileName)
        {
            try
            {
                // Сначала стандартные Program Files на всех дисках
                foreach (var drive in DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Fixed))
                {
                    string root = drive.RootDirectory.FullName;
                    string[] candidates =
                    {
                        Path.Combine(root, "Program Files", "PostgreSQL"),
                        Path.Combine(root, "Program Files (x86)", "PostgreSQL")
                    };

                    foreach (var baseDir in candidates)
                    {
                        for (int v = 17; v >= 10; v--)
                        {
                            string fullPath = Path.Combine(baseDir, v.ToString(), "bin", fileName);
                            if (File.Exists(fullPath))
                            {
                                System.Diagnostics.Debug.WriteLine($"{fileName} найден: {fullPath}");
                                return fullPath;
                            }
                        }
                    }

                    // pgAdmin runtime на всех дисках
                    string[] pgAdminDirs =
                    {
                        Path.Combine(root, "Program Files", "pgAdmin 4", "runtime"),
                        Path.Combine(root, "Program Files", "pgAdmin 5", "runtime"),
                        Path.Combine(root, "Program Files", "pgAdmin 6", "runtime"),
                        Path.Combine(root, "Program Files", "pgAdmin 7", "runtime"),
                        Path.Combine(root, "Program Files (x86)", "pgAdmin 4", "runtime"),
                        Path.Combine(root, "Program Files (x86)", "pgAdmin 5", "runtime"),
                        Path.Combine(root, "Program Files (x86)", "pgAdmin 6", "runtime"),
                        Path.Combine(root, "Program Files (x86)", "pgAdmin 7", "runtime")
                    };

                    foreach (var dir in pgAdminDirs)
                    {
                        string fullPath = Path.Combine(dir, fileName);
                        if (File.Exists(fullPath))
                        {
                            System.Diagnostics.Debug.WriteLine($"{fileName} найден в pgAdmin: {fullPath}");
                            return fullPath;
                        }
                    }
                }

                // Тупиковый поиск по папкам postgre на всех дисках (как у тебя на E:)
                foreach (var drive in DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Fixed))
                {
                    string root = drive.RootDirectory.FullName;
                    string postgreDir = Path.Combine(root, "postgre");
                    if (Directory.Exists(postgreDir))
                    {
                        for (int v = 17; v >= 10; v--)
                        {
                            string fullPath = Path.Combine(postgreDir, v.ToString(), "bin", fileName);
                            if (File.Exists(fullPath))
                            {
                                System.Diagnostics.Debug.WriteLine($"{fileName} найден: {fullPath}");
                                return fullPath;
                            }
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        // Если авто-поиск не сработал — показываем диалог выбора файла вручную
        private static string BrowseForExe(string fileName, string title)
        {
            try
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Title = title;
                    dialog.Filter = $"{fileName}|{fileName}";
                    dialog.CheckFileExists = true;
                    dialog.Multiselect = false;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        System.Diagnostics.Debug.WriteLine($"{fileName} выбран вручную: {dialog.FileName}");
                        return dialog.FileName;
                    }
                }
            }
            catch { }
            return null;
        }
    }
}
