using Microsoft.Data.SqlClient;
using MVCE.Server.Tables;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace MVCE.Models
{
    public record class Car
    {
        [Key] internal int CarID { get; set; } = 0;
        internal string FullName { get; set; }
        internal string Mark { get; set; }
        internal int MaxValue { get; set; }
        internal int CarryTime { get; set; }
        internal dbCar db = new dbCar();
        internal List<Component> Components { get; set; }
        public Car()
        {
            CarID++;
            FullName = "ABS";
            Mark = "RENO";
            MaxValue = 1000;
            CarryTime = 1;
        }
        public Car(string fullName, string mark, int maxValue, int carryTime)
        {
            FullName = fullName;
            Mark = mark;
            MaxValue = maxValue;
            CarryTime = carryTime;
        }
        public Car(int ID)
        {
            SqlDataReader reader = db.SelectCar(ID);
            if (reader.HasRows) 
            {
                reader.Read();
                this.MaxValue = int.Parse(reader["MaxValue"].ToString());
                this.Mark = reader["Mark"].ToString();
                this.CarryTime = int.Parse(reader["CarryTime"].ToString());
                this.FullName = reader["FullName"].ToString();
            }
            CarID = ID;
            reader.Close();
        }
        public void Create_Car()
        {
            db.Insert(this);
        }
        public void Update_Car() 
        {
            db.Update_Car(this);
        }
        public void Delete_Car()
        {
            db.Delete_Car(this.CarID);
        }
    }
}
