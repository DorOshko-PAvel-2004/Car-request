using MVCE.Models;
using MVCE.Server.Tables;
using System.Xml;

namespace MVCE.Data
{
    public class Orders
    {
        internal static dbOrders db = new dbOrders();
        public static List<Order> Select_All()
        {
            var reader = db.Select();
            List<Order> orders = new List<Order>();
            if (reader.HasRows)
            {
                Order order;
                while (reader.Read())
                {
                    order = new Order();
                    order.OrderID = int.Parse(reader["OrderID"].ToString());
                    order.FinishTime = reader["FinishTime"].ToString()==string.Empty ? 9999 : int.Parse(reader["FinishTime"].ToString());
                    order.Position = int.Parse(reader["Position"].ToString());
                    order.UserLogin = reader["UserLogin"].ToString();
                    orders.Add(order);
                }
            }
            reader.Close();
            return orders;
        }
        public static List<Order> SelectByUser(string User)
        {
            var reader = db.SearchByUser(User);
            List<Order> orders = new List<Order>();
            if (reader.HasRows)
            {
                Order order;
                while (reader.Read())
                {
                    order = new Order();
                    order.OrderID = int.Parse(reader["OrderID"].ToString());
                    order.Position = int.Parse(reader["Position"].ToString());
                    order.FinishTime = reader["FinishTime"].ToString() == string.Empty ? 9999 : int.Parse(reader["FinishTime"].ToString());
                    order.UserLogin = reader["UserLogin"].ToString();
                    orders.Add(order);
                }
            }
            reader.Close();
            return orders;
        }
        public static XmlDocument OrdersToXml()
        {
            List<Order> orders = new List<Order>();
            orders = Select_All();

            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
            //(2) string.Empty makes cleaner code
            XmlElement body = doc.CreateElement(string.Empty, "body", string.Empty);
            doc.AppendChild(body);
            foreach (var order in orders)
            {
                XmlElement xorder = doc.CreateElement(string.Empty, "Order", string.Empty);
                body.AppendChild(xorder);

                XmlElement OrderID = doc.CreateElement(string.Empty, "OrderID", string.Empty);
                XmlText _OrderID = doc.CreateTextNode(order.OrderID.ToString());
                OrderID.AppendChild(_OrderID);
                xorder.AppendChild(OrderID);

                XmlElement Position = doc.CreateElement(string.Empty, "Position", string.Empty);
                XmlText _Position = doc.CreateTextNode(order.Position.ToString());
                Position.AppendChild(_Position);
                xorder.AppendChild(Position);

                XmlElement FinishTime = doc.CreateElement(string.Empty, "FinishTime", string.Empty);
                XmlText _FinishTime = doc.CreateTextNode(order.FinishTime.ToString());
                FinishTime.AppendChild(_FinishTime);
                xorder.AppendChild(FinishTime);

                XmlElement UserLogin = doc.CreateElement(string.Empty, "UserLogin", string.Empty);
                XmlText _UserLogin = doc.CreateTextNode(order.UserLogin);
                UserLogin.AppendChild(_UserLogin);
                xorder.AppendChild(UserLogin);
            }
            //doc.Save("D:\\orders.xml");
            return doc;
        }
    }
}
