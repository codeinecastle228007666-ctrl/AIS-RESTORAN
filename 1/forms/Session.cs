using _1.data;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Db.Execute(sql, new NpgsqlParameter("@id", UserId.ToString()));
        }
    }
}
