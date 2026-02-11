using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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



        public static void ekzekuttranzakcii(string sql) //метод для выполнения транзакций, который принимает SQL запрос и выполняет его внутри транзакции, обеспечивая целостность данных
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
    }
}