// Глобальный класс хранения данных сессии пользователя
using _1.data;
using Npgsql;
using System;

namespace _1.forms
{
    // Статическое хранилище с данными авторизованного пользователя (сессия).
    public static class Session
    {
        // ID пользователя в БД.
        public static int UserId;
        // ID роли (1-кассир, 2-повар, 3-официант, 4-администратор).
        public static int RoleId;
        // Название роли.
        public static string RoleName;

        // Устанавливает параметр PostgreSQL app.user_id для триггеров. Триггер audit_log записывает кто и что делал.
        public static void ApplyToDb()
        {
            string sql = "SELECT set_config('app.user_id', @id, false)";
            var conn = Db.GetSessionConnection();
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Text).Value = UserId.ToString();
            cmd.ExecuteNonQuery();
        }
    }
}
