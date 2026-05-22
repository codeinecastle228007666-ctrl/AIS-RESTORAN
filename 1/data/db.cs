// Слой доступа к данным: подключение и работа с PostgreSQL
using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace _1.data
{
    // Статический класс для работы с базой данных PostgreSQL. Управляет сессионным подключением и предоставляет методы выполнения SQL-запросов.
    public static class Db
    {
        // Строка подключения к локальной БД
        private static string connectionString =
            "Host=localhost;Port=5432;Database=cursed_zxc_V2;Username=postgres;Password=1234";

        // Единое сессионное подключение для всего приложения
        private static NpgsqlConnection _sessionConnection;
        private static readonly object _lock = new object();

        // Создаёт новое подключение к БД (не сессионное).
        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        // Устанавливает переменную сессии PostgreSQL app.user_id для аудита (используется в триггерах audit_log).
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
                catch { /* Если переменная не установлена — игнорируем */ }
            }
        }

        // Открывает сессионное подключение (потокобезопасно).
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

        // Закрывает и освобождает сессионное подключение (потокобезопасно).
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

        // Возвращает текущее сессионное подключение (открывает при необходимости).
        public static NpgsqlConnection GetSessionConnection()
        {
            if (_sessionConnection == null || _sessionConnection.State == ConnectionState.Closed)
            {
                OpenSession();
            }
            return _sessionConnection;
        }

        // Проверяет, открыто ли сессионное подключение.
        public static bool IsSessionOpen()
        {
            return _sessionConnection != null && _sessionConnection.State == ConnectionState.Open;
        }

        // Выполняет SELECT-запрос и возвращает результат в виде DataTable.
        public static DataTable GetData(string sql)
        {
            return GetData(sql, null);
        }

        // Выполняет SELECT-запрос с параметрами и возвращает результат в виде DataTable.
        public static DataTable GetData(string sql, params NpgsqlParameter[] parameters)
        {
            try
            {
                // Если есть открытая сессия — используем её
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

                // Иначе создаём новое подключение
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

        // Выполняет SQL-запрос на изменение данных (INSERT/UPDATE/DELETE) без параметров.
        public static void ekzekuttranzakcii(string sql)
        {
            ekzekuttranzakcii(sql, null);
        }

        // Выполняет SQL-запрос на изменение данных (INSERT/UPDATE/DELETE) с параметрами.
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

        // Псевдоним для ekzekuttranzakcii.
        public static void Execute(string sql, params NpgsqlParameter[] parameters)
        {
            ekzekuttranzakcii(sql, parameters);
        }
    }
}
