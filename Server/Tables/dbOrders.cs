using Microsoft.Data.SqlClient;
using MVCE.Models;
namespace MVCE.Server.Tables
{
    public class dbOrders
    {
        SqlConnection sql = Connection.sql;
        public dbOrders()
        {

        }
        public int Create(Order order)
        {
            SqlCommand command = sql.CreateCommand();
            string text = $@"insert into [dbo].[Orders] (UserLogin, Position, FinishTime) values ('{order.UserLogin}',{order.Position}, {order.FinishTime}); SELECT SCOPE_IDENTITY()";
            command.CommandText = text;
            string reader = command.ExecuteScalar().ToString();
            return int.Parse(reader);
        }
        public SqlDataReader Select()
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText += $@"select * from [dbo].[Orders]";
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        public SqlDataReader Select(int ID)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText += $@"select * from [dbo].[Orders] where OrderID = {ID}";
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        public SqlDataReader SearchByUser(string User)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText += $@"select * from [dbo].[Orders] where Orders.UserLogin = '{User}';";
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        public void Delete(int ID)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = $@"delete from [dbo].[Orders] where [Orders].OrderID = {ID};";
            command.ExecuteNonQuery();
        }
        public int SearchLastPosition()
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = $@"select max(Position) as maxpos from [dbo].[Orders];";
            string lastPosition = command.ExecuteScalar().ToString();
            return lastPosition==string.Empty ? 0 : int.Parse(lastPosition);
        }
        public int SearchLastTime()
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = $@"select FinishTime from [dbo].[Orders] where [Orders].[OrderID] = (select max(OrderID) from [dbo].[Orders]);";
            object lastFinishTime = command.ExecuteScalar();
            return lastFinishTime == null ? 0 : int.Parse(lastFinishTime.ToString());
        }
        public void Update(Order order)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = $@"update [dbo].[Orders] set UserLogin = '{order.UserLogin}', 
Position = {order.Position}, FinishTime = {order.FinishTime} where [Orders].[OrderID] = {order.OrderID}";
            command.ExecuteNonQuery();
        }
    }
}
