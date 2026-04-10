using System;
using System.Data;
using Npgsql;

namespace _1.data
{
    public static class Db
    {
        private static string connectionString =
            "Host=localhost;Port=5432;Database=cursed_zxc_V2;Username=postgres;Password=1234";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        public static DataTable GetData(string sql)
        {
            using var conn = GetConnection();
            using var cmd = new NpgsqlCommand(sql, conn);
            using var adapter = new NpgsqlDataAdapter(cmd);

            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public static DataTable GetData(string sql, params NpgsqlParameter[] parameters)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        using (var da = new NpgsqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка базы данных:\n" + ex.Message);
                return new DataTable();
            }
        }

        public static void ekzekuttranzakcii(string sql)
        {
            using var conn = GetConnection();
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

        public static void ekzekuttranzakcii(string sql, params NpgsqlParameter[] parameters)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка выполнения запроса:\n" + ex.Message);
            }
        }
    }
}