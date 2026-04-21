using System;
using System.IO;

namespace _1.forms
{
    internal class Puti_dlya_rk
    {
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