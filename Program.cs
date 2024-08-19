
using Microsoft.Data.SqlClient;
using MVCE.Server;

namespace MVCE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Connection.sql = new SqlConnection(Connection.connection);
            if (Connection.sql.State == System.Data.ConnectionState.Closed)
            {
                Connection.sql.Open();
            }
            Console.WriteLine("Подключение открыто");
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMvc();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            var app = builder.Build();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}");
            app.UseSession();
            app.Run();
        }
    }
}