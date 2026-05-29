// Класс путей PostgreSQL (pg_dump, pg_restore, psql) на диске
using System;
using System.IO;

namespace _1.forms
{
    // Статический класс для поиска исполняемых файлов PostgreSQL.
    internal class Puti_dlya_rk
    {
        // Возвращает путь к pg_dump.exe.
        public static string GetPgDump()
        {
            string[] possible =
            {
                @"C:\Program Files\PostgreSQL\15\bin\pg_dump.exe",
                @"C:\Program Files\PostgreSQL\14\bin\pg_dump.exe",
                @"C:\Program Files\PostgreSQL\13\bin\pg_dump.exe",
                @"E:\ZAKAZAL NA GIDRE PISTOLET\pgAdmin 4\runtime\pg_dump.exe"
            };

            foreach (var p in possible)
                if (File.Exists(p))
                    return p;

            throw new Exception("pg_dump не найден.");
        }

        // Возвращает путь к pg_restore.exe.
        public static string GetPgRestore()
        {
            string[] possible =
            {
                @"C:\Program Files\PostgreSQL\15\bin\pg_restore.exe",
                @"C:\Program Files\PostgreSQL\14\bin\pg_restore.exe",
                @"C:\Program Files\PostgreSQL\13\bin\pg_restore.exe",
                @"E:\ZAKAZAL NA GIDRE PISTOLET\pgAdmin 4\runtime\pg_restore.exe"
            };

            foreach (var p in possible)
                if (File.Exists(p))
                    return p;

            throw new Exception("pg_restore не найден.");
        }

        // Возвращает путь к psql.exe.
        public static string GetPsql()
        {
            string[] possible =
            {
                @"C:\Program Files\PostgreSQL\15\bin\psql.exe",
                @"C:\Program Files\PostgreSQL\14\bin\psql.exe",
                @"C:\Program Files\PostgreSQL\13\bin\psql.exe",
                @"E:\ZAKAZAL NA GIDRE PISTOLET\pgAdmin 4\runtime\psql.exe"
            };

            foreach (var p in possible)
                if (File.Exists(p))
                    return p;

            throw new Exception("psql не найден.");
        }
    }
}
