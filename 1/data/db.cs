using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace _1.data
{
    public static class Db
    {
        private static string connectionString =
            "Host=localhost;Port=5432;Database=cursed_zxc_V2;Username=postgres;Password=1234";

        private static NpgsqlConnection _sessionConnection;
        private static readonly object _lock = new object();

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        private static void EnsureAppUserId(NpgsqlConnection conn)
        {
            if (_1.forms.Session.UserId > 0 && conn.State == ConnectionState.Open)
            {
                try
                {
                    using var cmd = new NpgsqlCommand("SELECT set_config('app.user_id', @id, false)", conn);
                    cmd.Parameters.AddWithValue("@id", _1.forms.Session.UserId.ToString());
                    cmd.ExecuteNonQuery();
                }
                catch { /* ignore if app.user_id already set or not available */ }
            }
        }

        public static void OpenSession()
        {
            lock (_lock)
            {
                if (_sessionConnection == null || _sessionConnection.State == ConnectionState.Closed)
                {
                    if (_sessionConnection != null)
                    {
                        _sessionConnection.Dispose();
                    }
                    _sessionConnection = new NpgsqlConnection(connectionString);
                    _sessionConnection.Open();
                }
            }
        }

        public static void CloseSession()
        {
            lock (_lock)
            {
                if (_sessionConnection != null)
                {
                    _sessionConnection.Close();
                    _sessionConnection.Dispose();
                    _sessionConnection = null;
                }
            }
        }

        public static NpgsqlConnection GetSessionConnection()
        {
            if (_sessionConnection == null || _sessionConnection.State == ConnectionState.Closed)
            {
                OpenSession();
            }
            return _sessionConnection;
        }

        public static bool IsSessionOpen()
        {
            return _sessionConnection != null && _sessionConnection.State == ConnectionState.Open;
        }

        public static DataTable GetData(string sql)
        {
            return GetData(sql, null);
        }

        public static DataTable GetData(string sql, params NpgsqlParameter[] parameters)
        {
            try
            {
                if (IsSessionOpen())
                {
                    using var cmd = new NpgsqlCommand(sql, GetSessionConnection());
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    using var adapter = new NpgsqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }

                using var conn = GetConnection();
                conn.Open();
                EnsureAppUserId(conn);
                using var cmd2 = new NpgsqlCommand(sql, conn);
                if (parameters != null)
                    cmd2.Parameters.AddRange(parameters);
                using var adapter2 = new NpgsqlDataAdapter(cmd2);
                DataTable table2 = new DataTable();
                adapter2.Fill(table2);
                return table2;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка базы данных:\n" + ex.Message, ex);
            }
        }

        public static void ekzekuttranzakcii(string sql)
        {
            ekzekuttranzakcii(sql, null);
        }

        public static void ekzekuttranzakcii(string sql, params NpgsqlParameter[] parameters)
        {
            try
            {
                if (IsSessionOpen())
                {
                    using var cmd = new NpgsqlCommand(sql, GetSessionConnection());
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                    return;
                }

                using (var conn = GetConnection())
                {
                    conn.Open();
                    EnsureAppUserId(conn);
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

        public static void Execute(string sql, params NpgsqlParameter[] parameters)
        {
            ekzekuttranzakcii(sql, parameters);
        }
    }
}
