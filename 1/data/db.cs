// Класс доступа к БД: подключение и работа с PostgreSQL
using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace _1.data
{
    // Статический класс для работы с базой данных PostgreSQL. Содержит методы подключения и выполнения SQL-запросов.
    // Режимы подключения:
    //   - Сессионное (GetSessionConnection/OpenSession): одно соединение на всю сессию, кэшируется.
    //     Используется для транзакций, которые должны быть в рамках одного подключения.
    //   - Временное (GetConnection): новое соединение на каждый запрос (автономные SELECT/INSERT/UPDATE).
    //
    // Все SQL-запросы принимают параметры через NpgsqlParameter[] для защиты от SQL-инъекций.
    // Исключение: DataView.Filter в Clienti.cs — фильтр строится строкой, но источник локальный DataTable.
    public static class Db
    {
        // Строка подключения к тестовой БД: Host, Port, Database, Username, Password
        // Пароль хранится в открытом виде (учебный проект). В production — через переменные окружения или Vault.
        private static string connectionString =
            "Host=localhost;Port=5432;Database=cursed_zxc_V2;Username=postgres;Password=1234";

        // Постоянное открытое соединение для всей сессии (кэш).
        // Потокобезопасность обеспечивается через _lock (Monitor.Enter/Exit).
        private static NpgsqlConnection _sessionConnection;
        private static readonly object _lock = new object();

        // Создание нового НЕзависимого подключения к БД (без кэширования).
        // Вызывающий должен самостоятельно вызвать conn.Open() и Dispose().
        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        // Установка пользовательской переменной PostgreSQL app.user_id для сессии (используется в триггерах audit_log).
        // set_config(..., false) — переменная действует только в рамках текущего подключения.
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
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"EnsureAppUserId: {ex.Message}"); }
            }
        }

        // Создание сессионного подключения (кэшированное).
        // Только одно постоянное соединение. Используется для: транзакций, audit_log, TableIsBusy.
        // Если соединение было закрыто/сброшено — пересоздаётся.
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

        // Закрытие и освобождение сессионного подключения (кэшированное).
        // Вызывается при выходе из приложения (FormClosing у Main).
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

        // Получение существующего сессионного подключения (создаёт при необходимости).
        // Вызов без предварительного OpenSession() сам откроет соединение.
        public static NpgsqlConnection GetSessionConnection()
        {
            if (_sessionConnection == null || _sessionConnection.State == ConnectionState.Closed)
            {
                OpenSession();
            }
            return _sessionConnection;
        }

        // Проверка, открыто ли сессионное подключение.
        // Используется GetData/ekzekuttranzakcii для выбора режима: сессионный vs временный.
        public static bool IsSessionOpen()
        {
            return _sessionConnection != null && _sessionConnection.State == ConnectionState.Open;
        }

        // Выполнение SELECT-запроса и возврат результата в виде DataTable (без параметров).
        // Делегирует GetData(sql, null).
        public static DataTable GetData(string sql)
        {
            return GetData(sql, null);
        }

        // Выполнение SELECT-запроса с параметрами и возврат результата в виде DataTable.
        // Параметры: NpgsqlParameter[] — @name в SQL соответствует имени параметра.
        // Пример: GetData("SELECT * FROM t WHERE id = @id", new NpgsqlParameter("@id", 5))
        //
        // Логика выбора подключения:
        //   - Если открыта сессия (IsSessionOpen) — используем сессионное соединение.
        //   - Иначе — создаём новое временное, открываем, исполняем, закрываем (using).
        //   - EnsureAppUserId — для временных подключений передаём ID пользователя в триггеры.
        public static DataTable GetData(string sql, params NpgsqlParameter[] parameters)
        {
            try
            {
                // Если есть открытая сессия — используем её (транзакции, audit_log).
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

                // Иначе создаём временное подключение (автономные SELECT-запросы).
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
                // Перебрасываем с префиксом "Ошибка SQL запроса:" для единого формата ошибок.
                throw new Exception("Ошибка SQL запроса:\n" + ex.Message, ex);
            }
        }

        // Выполнение SQL-команды без возврата данных (INSERT/UPDATE/DELETE) без параметров.
        // Делегирует ekzekuttranzakcii(sql, null).
        public static void ekzekuttranzakcii(string sql)
        {
            ekzekuttranzakcii(sql, null);
        }

        // Выполнение SQL-команды без возврата данных (INSERT/UPDATE/DELETE) с параметрами.
        // Параметры передаются через NpgsqlParameter[].
        // Пример: ekzekuttranzakcii("UPDATE t SET x=@x WHERE id=@id", new NpgsqlParameter("@x", 1), new NpgsqlParameter("@id", 5))
        //
        // Логика выбора подключения — та же, что в GetData:
        //   - Если сессия открыта — используем её.
        //   - Иначе — временное подключение + EnsureAppUserId.
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
                throw new Exception("Ошибка выполнения команды:\n" + ex.Message, ex);
            }
        }

        // Синоним для ekzekuttranzakcii (англоязычное название).
        // Используется для read-only операций (DELETE без возврата данных).
        public static void Execute(string sql, params NpgsqlParameter[] parameters)
        {
            ekzekuttranzakcii(sql, parameters);
        }
    }
}
