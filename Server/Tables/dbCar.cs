using Microsoft.Data.SqlClient;
using MVCE.Models;

namespace MVCE.Server.Tables
{
    public class dbCar
    {
        SqlConnection sql = Connection.sql;
        public dbCar():base()
        {

        }
        public void Insert(Car car)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = @$"insert into [dbo].[Car] (FullName,Mark,MaxValue,CarryTime) values ('{car.FullName}','{car.Mark}',{car.MaxValue},{car.CarryTime});";
            command.ExecuteNonQuery();
        }
        public SqlDataReader Select()
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = @"select * from [dbo].[Car];";
            return (SqlDataReader?)command.ExecuteReader();
        }
        public SqlDataReader Select(int[] Ids)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = @"select * from [dbo].[Car] where [Car].[CarID] in ( ";
            int i;
            for (i = 0; i<Ids.Length-1;i++)
            {
                command.CommandText += $"{Ids[i]}, ";
            }
            command.CommandText += $"{Ids[i]}); ";
            return (SqlDataReader?)command.ExecuteReader();
        }
        public SqlDataReader SelectCar(int Id)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText += @$"select * from [dbo].[Car] where [Car].[CarID] = {Id};";
            return (SqlDataReader?)command.ExecuteReader();
        }
        public void Delete_Car(int ID)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText += @$"delete from [dbo].[Car] where [Car].[CarID] = {ID};";
            command.ExecuteNonQuery();
        }
        public void Update_Car(Car car)
        {
            SqlCommand command = sql.CreateCommand();
            command.CommandText = $@"Update [dbo].[Car] set 
            [Car].[FullName]='{car.FullName}',
            [Car].[Mark]='{car.Mark}',
            [Car].[MaxValue]={car.MaxValue},
            [Car].[CarryTime]={car.CarryTime} where [Car].[CarID]={car.CarID};";
            command.ExecuteNonQuery();
        }
    }
}
