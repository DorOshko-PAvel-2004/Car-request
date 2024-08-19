using Microsoft.Data.SqlClient;
using MVCE.Models;

namespace MVCE.Server.Tables
{
    public class dbOrderCar
    {
        SqlConnection sql = Connection.sql;
        public SqlDataReader Select()
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = $@"select * from [dbo].[OrdersCars];";
            SqlDataReader reader = command.ExecuteReader();
            return reader;

        }
        public void Insert(OrderCars cars)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText += $@"insert into [dbo].[OrdersCars] (OrderId,CarId,Quantity) values ";
            command.CommandText += $"({cars.OrderId}, {cars.CarId}, {cars.Quantity});";
            try 
            { 
                int status = command.ExecuteNonQuery();
                if (status == 0)
                {
                    throw new ArgumentNullException();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("INSERTING ERROR IN ORDERCARS!!!!");
            }
        }
        public SqlDataReader FindByOrders(int OrderId)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText += @$"select OrdersCars.OrderId, OrdersCars.CarID, Quantity from OrdersCars
            where OrderId = {OrderId}";
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        public SqlDataReader FindByCar(int CarID)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText += @$"select OrdersCars.OrderId, OrdersCars.CarID, Quantity from OrdersCars
                                    where CarID = {CarID};";
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        public SqlDataReader ReturnNum_Cars(int OrderId)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText += @$"select count(CarId) as number from [dbo].[OrdersCars] where OrderId = {OrderId};";
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        public void Delete(int OrderId, int CarId)
        {
            SqlCommand command= sql.CreateCommand();
            command.CommandText = $@"delete from [dbo].[OrdersCars] where [OrdersCars].[OrderId]={OrderId} and [OrdersCars].[CarId] = {CarId}";
            command.ExecuteNonQuery();
        }
    }
}
