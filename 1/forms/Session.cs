// Статический класс состояния текущего пользователя
using _1.data;
using Npgsql;
using System;

namespace _1.forms
{
    // Хранит информацию о текущем авторизованном пользователе (сессия).
    public static class Session
    {
        // ID пользователя в БД.
        public static int UserId;
        // ID роли (1-Официант, 2-Повар, 3-Шеф-повар, 4-Руководитель).
        public static int RoleId;
        // Название роли.
        public static string RoleName;

        // Устанавливает переменную PostgreSQL app.user_id для аудита. Триггеры audit_log используют это значение для логирования действий.
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
