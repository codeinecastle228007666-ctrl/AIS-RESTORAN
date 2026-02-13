using System;
using System.Data;
using Npgsql;

namespace _1.data
{
    public static class Db
    {
        private static string connectionString =
            "Host=localhost;Port=5432;Database=cursed_zxc_V2;Username=postgres;Password=1234"; //строка подключения к базе данных

        public static DataTable GetData(string sql)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(sql, conn);
            using var adapter = new NpgsqlDataAdapter(cmd);

            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }

        // старый вариант оставлен для обратной совместимости
        public static void ekzekuttranzakcii(string sql)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var tranzakciya = conn.BeginTransaction();
            try
            {
                using var cmd = new NpgsqlCommand(sql, conn, tranzakciya);
                cmd.ExecuteNonQuery();
                tranzakciya.Commit();
            }
            catch
            {
                tranzakciya.Rollback();
                throw;
            }
        }

        // новая перегрузка с поддержкой параметров
        public static void ekzekuttranzakcii(string sql, params NpgsqlParameter[] parameters)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var tranzakciya = conn.BeginTransaction();
            try
            {
                using var cmd = new NpgsqlCommand(sql, conn, tranzakciya);
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                cmd.ExecuteNonQuery();
                tranzakciya.Commit();
            }
            catch
            {
                tranzakciya.Rollback();
                throw;
            }
        }
    }
}