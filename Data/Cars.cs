using Microsoft.Data.SqlClient;
using MVCE.Models;
using MVCE.Server.Tables;
using System.Xml;

namespace MVCE.Data
{
    public static class Cars
    {
        internal static dbCar db = new dbCar();
        public static List<Car> Select()
        {
            SqlDataReader reader = db.Select();
            List<Car> cars = new List<Car>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Car car = new Car();
                    car.CarID = int.Parse(reader["CarID"].ToString());
                    car.MaxValue = int.Parse(reader["MaxValue"].ToString());
                    car.Mark = reader["Mark"].ToString();
                    car.CarryTime = int.Parse(reader["CarryTime"].ToString());
                    car.FullName = reader["FullName"].ToString();
                    cars.Add(car);
                }
            }
            reader.Close();
            return cars;
        }
        public static List<Car> Select(int[] CarIds)
        {
            SqlDataReader reader = db.Select(CarIds);
            List<Car> cars = new List<Car>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Car car = new Car();
                    car.CarID = int.Parse(reader["CarID"].ToString());
                    //car.MaxValue = int.Parse(reader["MaxValue"].ToString());
                    //car.Mark = reader["Mark"].ToString();
                    //car.CarryTime = int.Parse(reader["CarryTime"].ToString());
                    car.FullName = reader["FullName"].ToString();
                    cars.Add(car);
                }
            }
            reader.Close();
            return cars;

        }
        public static Dictionary<int, string> SelectNames()
        {
            SqlDataReader reader = db.Select();
            Dictionary<int, string> CarNames = new Dictionary<int, string>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int CarID = int.Parse(reader["CarID"].ToString());
                    string FullName = reader["FullName"].ToString();
                    CarNames.Add(CarID, FullName);
                }
            }
            reader.Close();
            return CarNames;
        }
        public static XmlDocument CarsToXml()
        {
                List<Car> cars = new List<Car>();
                cars = Select();

                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);
                //(2) string.Empty makes cleaner code
                XmlElement body = doc.CreateElement(string.Empty, "body", string.Empty);
                doc.AppendChild(body);
                foreach (var car in cars)
                {
                    XmlElement xcar = doc.CreateElement(string.Empty, "Car", string.Empty);
                    body.AppendChild(xcar);

                XmlElement CarID = doc.CreateElement(string.Empty, "CarID", string.Empty);
                XmlText _CarID = doc.CreateTextNode(car.CarID.ToString());
                CarID.AppendChild(_CarID);
                xcar.AppendChild(CarID);

                XmlElement FullName = doc.CreateElement(string.Empty, "FullName", string.Empty);
                    XmlText _FullName = doc.CreateTextNode(car.FullName);
                    FullName.AppendChild(_FullName);
                    xcar.AppendChild(FullName);

                    XmlElement Mark = doc.CreateElement(string.Empty, "Mark", string.Empty);
                    XmlText _Mark = doc.CreateTextNode(car.Mark);
                    Mark.AppendChild(_Mark);
                    xcar.AppendChild(Mark);

                XmlElement MaxValue = doc.CreateElement(string.Empty, "MaxValue", string.Empty);
                XmlText _MaxValue = doc.CreateTextNode(car.MaxValue.ToString());
                MaxValue.AppendChild(_MaxValue);
                xcar.AppendChild(MaxValue);

                XmlElement CarryTime = doc.CreateElement(string.Empty, "CarryTime", string.Empty);
                XmlText _CarryTime = doc.CreateTextNode(car.CarryTime.ToString());
                CarryTime.AppendChild(_CarryTime);
                xcar.AppendChild(CarryTime);
            }
            return doc;
                //doc.Save("D:\\cars.xml");
        }
    }
}
