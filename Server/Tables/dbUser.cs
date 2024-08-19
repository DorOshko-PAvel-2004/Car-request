using Microsoft.Data.SqlClient;
using MVCE.Models;
namespace MVCE.Server.Tables
{
    public class dbUser
    {
        SqlConnection sql = Connection.sql;
        public dbUser()
        {   }
        public SqlDataReader Select_All()
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = $@"select * from [dbo].[User];";
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        public SqlDataReader Authorization(string Name, string Password)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = $@"select * from [dbo].[User] where 
                [User].[Name]='{Name}' and [User].[Password]='{Password}';";
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        public object Search_User(string Name)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = $@"select [User].[Name] from [dbo].[User] where 
                [User].[Name]='{Name}';";
            var reader = command.ExecuteScalar();
            return reader;
        }
        public bool Registration(User user)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = @$"insert into [dbo].[User] ([User].[Name], [User].[Password]) 
            values ('{user.Name}','{user.Password}');";
            bool success = command.ExecuteNonQuery() == 1 ? true:false;
            return success;
        }
        public SqlDataReader SearchByName(string Name)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = $@"select * from [dbo].[User] where 
                [User].[Name]='{Name}';";
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        public void Update_User(string LastName, User user)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = $@"update [dbo].[User] set [User].[Name] = '{user.Name}', [User].[Password]='{user.Password}' where [User].[Name] = '{LastName}'";
            command.ExecuteNonQuery();
        }
        public void Delete_User(string Name)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = @$"delete from [dbo].[User] 
            where [User].[Name]='{Name}';";
            command.ExecuteNonQuery();
        }

    }
}
