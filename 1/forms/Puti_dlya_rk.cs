// Класс путей PostgreSQL (pg_dump, pg_restore, psql) на диске
using System;
using System.Diagnostics;
using System.IO;

namespace _1.forms
{
    // Статический класс для поиска исполняемых файлов PostgreSQL.
    // Сначала ищет через PATH (where.exe), затем сканирует Program Files.
    internal class Puti_dlya_rk
    {
        // Возвращает путь к pg_dump.exe.
        public static string GetPgDump()
        {
            string path = FindInPath("pg_dump");
            if (path != null) return path;

            string found = ScanPostgresBin("pg_dump.exe");
            if (found != null) return found;

            throw new Exception("pg_dump не найден. Установите PostgreSQL или укажите путь в PATH.");
        }

        // Возвращает путь к pg_restore.exe.
        public static string GetPgRestore()
        {
            string path = FindInPath("pg_restore");
            if (path != null) return path;

            string found = ScanPostgresBin("pg_restore.exe");
            if (found != null) return found;

            throw new Exception("pg_restore не найден. Установите PostgreSQL или укажите путь в PATH.");
        }

        // Возвращает путь к psql.exe.
        public static string GetPsql()
        {
            string path = FindInPath("psql");
            if (path != null) return path;

            string found = ScanPostgresBin("psql.exe");
            if (found != null) return found;

            throw new Exception("psql не найден. Установите PostgreSQL или укажите путь в PATH.");
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
                        return output;
                    }
                }
            }
            catch { }
            return null;
        }

        // Сканирование Program Files на наличие PostgreSQL\версия\bin\файл
        private static string ScanPostgresBin(string fileName)
        {
            string[] programDirs =
            {
                @"C:\Program Files\PostgreSQL",
                @"C:\Program Files (x86)\PostgreSQL",
                @"D:\Program Files\PostgreSQL",
                @"E:\Program Files\PostgreSQL"
            };

            // Версии PostgreSQL, которые ищем (от 10 до 17)
            for (int v = 17; v >= 10; v--)
            {
                foreach (var dir in programDirs)
                {
                    string fullPath = Path.Combine(dir, v.ToString(), "bin", fileName);
                    if (File.Exists(fullPath))
                    {
                        System.Diagnostics.Debug.WriteLine($"{fileName} найден: {fullPath}");
                        return fullPath;
                    }
                }
            }

            // Дополнительно ищем в pgAdmin runtime
            string[] pgAdminPaths =
            {
                @"C:\Program Files\pgAdmin 4\runtime",
                @"C:\Program Files\pgAdmin 5\runtime",
                @"C:\Program Files\pgAdmin 6\runtime",
                @"C:\Program Files\pgAdmin 7\runtime",
                @"C:\Program Files (x86)\pgAdmin 4\runtime",
                @"C:\Program Files (x86)\pgAdmin 5\runtime",
                @"C:\Program Files (x86)\pgAdmin 6\runtime",
                @"C:\Program Files (x86)\pgAdmin 7\runtime"
            };

            foreach (var dir in pgAdminPaths)
            {
                string fullPath = Path.Combine(dir, fileName);
                if (File.Exists(fullPath))
                {
                    System.Diagnostics.Debug.WriteLine($"{fileName} найден в pgAdmin: {fullPath}");
                    return fullPath;
                }
            }

            return null;
        }
    }
}
