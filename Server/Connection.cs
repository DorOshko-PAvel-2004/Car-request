using Microsoft.Data.SqlClient;

namespace MVCE.Server
{
    public static class Connection
    {
        public static string connection = "Data Source=DESKTOP-93G6P48;Database=factory;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Max Pool Size=200";
        public static SqlConnection sql;
        //public Connection()
        //{
        //    sql = new SqlConnection(connection);
        //    if(sql.State==System.Data.ConnectionState.Closed)
        //    {
        //        sql.Open();
        //    }
        //    Console.WriteLine("Подключение открыто");
        //}
    }
}
