using Microsoft.Data.SqlClient;
using MVCE.Models;
using MVCE.Server.Tables;
using static NuGet.Packaging.PackagingConstants;
using System.Xml;

namespace MVCE.Data
{
    public static class OrdersCars
    {
        internal static dbOrderCar db = new dbOrderCar();
        public static List<OrderCars> Select_All()
        {
            List<OrderCars> list = new List<OrderCars>();
            SqlDataReader reader = db.Select();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    OrderCars cars = new OrderCars();
                    cars.OrderId = int.Parse(reader["OrderId"].ToString());
                    cars.CarId = int.Parse(reader["CarId"].ToString());
                    cars.Quantity = int.Parse(reader["Quantity"].ToString());
                    list.Add(cars);
                }
            }
            reader.Close();
            return list;
        }
        public static List<OrderCars> GetOrdersCars(int Order) 
        {
            List<OrderCars> list = new List<OrderCars>();
            SqlDataReader reader = db.FindByOrders(Order);
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    OrderCars cars = new OrderCars();
                    cars.OrderId = int.Parse(reader["OrderId"].ToString());
                    cars.CarId = int.Parse(reader["CarId"].ToString());
                    cars.Quantity = int.Parse(reader["Quantity"].ToString());
                    list.Add(cars);
                }
            }
            reader.Close();
            return list;
        }
        public static List<OrderCars> GetOrdersByCar(int Car)
        {
            SqlDataReader reader = db.FindByCar(Car);
            List<OrderCars> OrderIds = null;
            if (reader.HasRows) 
            {
                OrderIds = new List<OrderCars>();
                while(reader.Read())
                {
                    int OrderId = int.Parse(reader["OrderId"].ToString());
                    int CarId = int.Parse(reader["CarId"].ToString());
                    int Quantity = int.Parse(reader["Quantity"].ToString());
                    OrderCars orderCars = new OrderCars(CarId, OrderId, Quantity);
                    OrderIds.Add(orderCars);
                }
            }
            reader.Close();
            return OrderIds;
        }
        public static XmlDocument OrdersCarsToXml()
        {
            List<OrderCars> orderCars = new List<OrderCars>();
            orderCars = Select_All();
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
            //(2) string.Empty makes cleaner code
            XmlElement body = doc.CreateElement(string.Empty, "body", string.Empty);
            doc.AppendChild(body);
            foreach (var ordercar in orderCars)
            {
                XmlElement xordercars = doc.CreateElement(string.Empty, "OrderCar", string.Empty);
                body.AppendChild(xordercars);

                XmlElement OrderId = doc.CreateElement(string.Empty, "OrderId", string.Empty);
                XmlText _OrderID = doc.CreateTextNode(ordercar.OrderId.ToString());
                OrderId.AppendChild(_OrderID);
                xordercars.AppendChild(OrderId);

                XmlElement CarId = doc.CreateElement(string.Empty, "CarId", string.Empty);
                XmlText _CarId = doc.CreateTextNode(ordercar.CarId.ToString());
                CarId.AppendChild(_CarId);
                xordercars.AppendChild(CarId);

                XmlElement Quantity = doc.CreateElement(string.Empty, "Quantity", string.Empty);
                XmlText _Quantity = doc.CreateTextNode(ordercar.Quantity.ToString());
                Quantity.AppendChild(_Quantity);
                xordercars.AppendChild(Quantity);
            }
            //doc.Save("D:\\orderscars.xml");
            return doc;
        }
    }
}
