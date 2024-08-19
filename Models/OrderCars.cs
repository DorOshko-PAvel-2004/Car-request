using Microsoft.Data.SqlClient;
using MVCE.Server.Tables;

namespace MVCE.Models
{

    public class OrderCars
    {
        internal dbOrderCar db = new dbOrderCar();
        public int CarId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public OrderCars()
        {
            CarId = 0;
            OrderId = 0;
            Quantity = 0;
        }
        public OrderCars(int carId, int orderId, int quantity)
        {
            CarId = carId;
            OrderId = orderId;
            Quantity = quantity;
        }
        public OrderCars(int carId, int orderId)
        {
            CarId = carId;
            OrderId = orderId;
            Quantity = 0;
        }
        public void Insert()
        {
            db.Insert(this);
        }
        public int SelectCountCars(int OrderId)
        {
            SqlDataReader reader = db.ReturnNum_Cars(OrderId);
            reader.Read();
            int number = int.Parse(reader["number"].ToString());
            reader.Close();
            return number;
        }
        public void Delete()
        {
            db.Delete(this.OrderId, this.CarId);
        }
    }
}
