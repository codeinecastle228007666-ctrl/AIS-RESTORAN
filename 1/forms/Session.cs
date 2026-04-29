using _1.data;
using Npgsql;
using System;

namespace _1.forms
{
    public static class Session
    {
        public static int UserId;
        public static int RoleId;
        public static string RoleName;

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
