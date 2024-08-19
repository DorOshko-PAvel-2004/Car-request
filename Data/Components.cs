using MVCE.Models;
using Microsoft.Data.SqlClient;
namespace MVCE.Data
{
    public static class Components
    {
        public static List<Component> Select(SqlConnection sql)
        {
            var list = new List<Component>();
            SqlCommand command = sql.CreateCommand();
            command.CommandText = "select * from [dbo].[Component];";
            SqlDataReader reader = command.ExecuteReader();
            Component component = new Component();
            if(reader.HasRows)
            {
                while(reader.Read()) 
                {
                    component.Indentifier = reader["Indentifier"].ToString();
                    component.Type = reader["Type"].ToString();
                    component.ComponentID = int.Parse(reader["ComponentID"].ToString());
                    list.Add(component);
                }
            }
            return list;
        }
    }
}
