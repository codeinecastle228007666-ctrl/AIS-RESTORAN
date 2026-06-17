// Класс путей PostgreSQL (pg_dump, pg_restore, psql) на диске
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace _1.forms
{
    // Статический класс для поиска исполняемых файлов PostgreSQL.
    // Порядок поиска: PATH → сканирование дисков (полные установки → pgAdmin) → ручной выбор.
    // Кэширует найденную папку bin, чтобы все три утилиты брались из одного места.
    internal class Puti_dlya_rk
    {
        // Кэш — запоминаем папку, где нашли PostgreSQL, чтобы все три утилиты были из одного места
        private static string _cachedBinDir = null;

        // Возвращает путь к pg_dump.exe.
        public static string GetPgDump()
        {
            string exe = "pg_dump.exe";
            if (_cachedBinDir != null)
            {
                string path = Path.Combine(_cachedBinDir, exe);
                if (File.Exists(path)) return path;
            }

            string result = FindTool(exe);
            if (result != null) return result;

            result = BrowseForExe(exe, "Выберите pg_dump.exe");
            if (result != null) return result;

            throw new Exception("pg_dump не найден. Установите PostgreSQL или укажите путь вручную.");
        }

        // Возвращает путь к pg_restore.exe.
        public static string GetPgRestore()
        {
            string exe = "pg_restore.exe";
            if (_cachedBinDir != null)
            {
                string path = Path.Combine(_cachedBinDir, exe);
                if (File.Exists(path)) return path;
            }

            string result = FindTool(exe);
            if (result != null) return result;

            result = BrowseForExe(exe, "Выберите pg_restore.exe");
            if (result != null) return result;

            throw new Exception("pg_restore не найден. Установите PostgreSQL или укажите путь вручную.");
        }

        // Возвращает путь к psql.exe.
        public static string GetPsql()
        {
            string exe = "psql.exe";
            if (_cachedBinDir != null)
            {
                string path = Path.Combine(_cachedBinDir, exe);
                if (File.Exists(path)) return path;
            }

            string result = FindTool(exe);
            if (result != null) return result;

            result = BrowseForExe(exe, "Выберите psql.exe");
            if (result != null) return result;

            throw new Exception("psql не найден. Установите PostgreSQL или укажите путь вручную.");
        }

        // Поиск через where.exe (проверяет PATH)
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
                        CacheDir(output);
                        return output;
                    }
                }
            }
            catch { }
            return null;
        }

        // Единый метод: сканирует папки с полноценным PostgreSQL, потом pgAdmin
        private static string FindTool(string exeName)
        {
            // Сначала ищем среди полноценных установок PostgreSQL
            string found = ScanPostgresDirs(exeName);
            if (found != null) return found;

            // Если нет — ищем в pgAdmin runtime (там могут быть не все утилиты)
            found = ScanPgAdminDirs(exeName);
            if (found != null) return found;

            return null;
        }

        // Сканирование директорий с полноценным PostgreSQL:
        // Program Files\PostgreSQL\*\bin\ и корневые папки postgre\*\bin\
        private static string ScanPostgresDirs(string exeName)
        {
            try
            {
                foreach (var drive in DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Fixed))
                {
                    string root = drive.RootDirectory.FullName;

                    // Стандартные пути: X:\Program Files\PostgreSQL\версия\bin\
                    string[] standardBases =
                    {
                        Path.Combine(root, "Program Files", "PostgreSQL"),
                        Path.Combine(root, "Program Files (x86)", "PostgreSQL")
                    };

                    // Нестандартные пути (как E:\ZAKAZAL...\postgre)
                    string[] extraBases =
                    {
                        Path.Combine(root, "postgre"),
                        Path.Combine(root, "ZAKAZAL NA GIDRE PISTOLET", "postgre")
                    };

                    foreach (var baseDir in standardBases.Concat(extraBases))
                    {
                        for (int v = 17; v >= 10; v--)
                        {
                            string fullPath = Path.Combine(baseDir, v.ToString(), "bin", exeName);
                            if (File.Exists(fullPath))
                            {
                                System.Diagnostics.Debug.WriteLine($"{exeName} найден: {fullPath}");
                                CacheDir(fullPath);
                                return fullPath;
                            }
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        // Сканирование pgAdmin runtime — в самую последнюю очередь
        private static string ScanPgAdminDirs(string exeName)
        {
            try
            {
                foreach (var drive in DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Fixed))
                {
                    string root = drive.RootDirectory.FullName;
                    string[] pgAdminDirs =
                    {
                        Path.Combine(root, "Program Files", "pgAdmin 4", "runtime"),
                        Path.Combine(root, "Program Files", "pgAdmin 5", "runtime"),
                        Path.Combine(root, "Program Files", "pgAdmin 6", "runtime"),
                        Path.Combine(root, "Program Files", "pgAdmin 7", "runtime"),
                        Path.Combine(root, "Program Files (x86)", "pgAdmin 4", "runtime"),
                        Path.Combine(root, "Program Files (x86)", "pgAdmin 5", "runtime"),
                        Path.Combine(root, "Program Files (x86)", "pgAdmin 6", "runtime"),
                        Path.Combine(root, "Program Files (x86)", "pgAdmin 7", "runtime"),
                        Path.Combine(root, "ZAKAZAL NA GIDRE PISTOLET", "pgAdmin 4", "runtime")
                    };

                    foreach (var dir in pgAdminDirs)
                    {
                        string fullPath = Path.Combine(dir, exeName);
                        if (File.Exists(fullPath))
                        {
                            System.Diagnostics.Debug.WriteLine($"{exeName} найден в pgAdmin: {fullPath}");
                            CacheDir(fullPath);
                            return fullPath;
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        // Запоминаем папку, чтобы следующие утилиты искались там же
        private static void CacheDir(string exePath)
        {
            string dir = Path.GetDirectoryName(exePath);
            if (dir != null && _cachedBinDir == null)
            {
                _cachedBinDir = dir;
                System.Diagnostics.Debug.WriteLine($"Запомнили папку PostgreSQL: {dir}");
            }
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
                        CacheDir(dialog.FileName);
                        return dialog.FileName;
                    }
                }
            }
            catch { }
            return null;
        }
    }
}
