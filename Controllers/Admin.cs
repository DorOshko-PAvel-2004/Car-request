using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCE.Models;
using MVCE.Data;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;

namespace MVCE.Controllers
{
    public class Admin : Controller
    {
        public ActionResult UserList()
        {
            List<User> users = new List<User>();
            users = Users.Select_All();
            return View(users);
        }
        public ActionResult CarList()
        {
            List<Car> cars = new List<Car>();
            cars = Cars.Select();
            return View(cars);
        }
        public ActionResult OrderList()
        {
            List<Order> orders = new List<Order>();
            orders = Orders.Select_All();
            List<OrderCars> cars = new List<OrderCars>();
            foreach (var order in orders)
            {
                cars.AddRange(OrdersCars.GetOrdersCars(order.OrderID));
            }
            ViewBag.CarsbyOrder = cars;
            return View(orders);
        }
        [HttpPost]
        public IActionResult Delete_Car(int CarID)
        {
            Car car = new Car(CarID);
            List<OrderCars> orders = new List<OrderCars>();
            OrderCars cars = new OrderCars();
            orders = OrdersCars.GetOrdersByCar(CarID);
            if(orders!=null)
            {
                Order order;
                foreach(var ordercar in orders)
                {
                    if (cars.SelectCountCars(ordercar.OrderId) == 1) 
                    {
                        order = new Order(ordercar.OrderId);
                        order.Delete();   
                    }
                    else
                    {
                        cars.CarId = CarID;
                        cars.OrderId = ordercar.OrderId;
                        cars.Delete();
                    }
                }
            }
            //1 - проверить, заказывали ли данную машину
            //2 - если да, то удалить заказ с этой машиной
            //3 - если заказ больше не имеет машин
            car.Delete_Car();
            return RedirectToAction("CarList");
        }
        [HttpPost]
        public IActionResult Update_Car(int CarID)
        {
            Car car = new Car(CarID);
            return View(car);
        }
        [HttpPost]
        public IActionResult Update()
        {
            int CarID = int.Parse(Request.Form["CarID"]);
            Car car = new Car(CarID);
            int lastCarryTime = car.CarryTime;
            string FullName = Request.Form["FullName"];
            string Mark = Request.Form["Mark"];
            string _CarryTime = Request.Form["CarryTime"];
            string _MaxValue = Request.Form["MaxValue"];
            if (!(Security.StringCheck(new string[] { FullName, Mark }) 
                && Security.NumberCheck(new string[] { _MaxValue, _CarryTime })))
            {
                ViewData["Error_Message"] = "You can enter only letters or positive integers.";
                return View("Update_Car",car);
            }
            int CarryTime = int.Parse(_CarryTime);
            int MaxValue = int.Parse(_MaxValue);
            car.CarryTime = CarryTime;
            car.FullName = FullName;
            car.Mark = Mark;
            car.MaxValue = MaxValue;

            car.Update_Car();
            List<OrderCars> ordercars = new List<OrderCars>();
            ordercars = OrdersCars.GetOrdersByCar(car.CarID);
            if (ordercars != null)
            {
                Order order;
                foreach (var ordercar in ordercars)
                {
                    order = new Order(ordercar.OrderId);
                    order.FinishTime += ordercar.Quantity * (CarryTime - lastCarryTime);
                    order.Update();
                }
            }
            return RedirectToAction("CarList");
        }
        public IActionResult Create_Car_Page()
        {
            return View("Create_Car");
        }
        [HttpPost]
        public IActionResult Create_Car()
        {
            string FullName = Request.Form["FullName"];
            string Mark = Request.Form["Mark"];
            string _CarryTime = Request.Form["CarryTime"];
            string _MaxValue = Request.Form["MaxValue"];
            if (!(Security.StringCheck(new string[] { FullName, Mark }) 
                && Security.NumberCheck(new string[] { _MaxValue, _CarryTime }))) 
            {
                ViewData["Error_Message"] = "You can use only letters or positive integers.";
                return View("Create_Car");
            }
            int CarryTime = int.Parse(_CarryTime);
            int MaxValue = int.Parse(_MaxValue);
            Car car = new Car(FullName,Mark,MaxValue,CarryTime);
            car.Create_Car();
            return RedirectToAction("CarList");
        }
        [HttpPost]
        public IActionResult Delete_User(string Name)
        {
            User user = new User(Name,"");
            //если у пользователя есть незаконченные заказы, то они удаляются вместе с юзером.
            //Стоит настройка on delete cascade
            user.Delete();
            return RedirectToAction("UserList");
        }
        public IActionResult Exit()
        {
            if (HttpContext.Session.GetString("admin")!=null)
            {
                HttpContext.Session.Remove("admin");
            }
            return RedirectToActionPreserveMethod("Index", "Home");
        }
        public IActionResult Delete_Order(int OrderID)
        {
            Order order = new Order(OrderID);
            order.Delete();
            return RedirectToAction("OrderList");
        }
        [HttpPost]
        public IActionResult Delete_OrderCar(int OrderId, int CarId)
        {
            OrderCars cars = new OrderCars(CarId, OrderId);
            Order order;
            if (cars.SelectCountCars(OrderId) == 1)
            {
                order = new Order(OrderId);
                order.Delete();
                return RedirectToAction("OrderList");
            }
            cars.Delete();
            return RedirectToAction("OrderList");
        }
        public IActionResult UsersToXml()
        {
            XmlDocument x_users = new XmlDocument();
            x_users = Users.UsersToXml();
            var memory = new MemoryStream();
            using (var xmlWriter = XmlWriter.Create(memory, new XmlWriterSettings { Indent = true }))
            {
                x_users.Save(xmlWriter);
            }
            memory.Position = 0;

            // Возвращаем файл для скачивания
            return File(memory, "application/xml", "users.xml");
        }
        public IActionResult OrdersToXml()
        {
            XmlDocument x_orders = new XmlDocument();
            x_orders = Orders.OrdersToXml();
            var memory = new MemoryStream();
            using (var xmlWriter = XmlWriter.Create(memory, new XmlWriterSettings { Indent = true }))
            {
                x_orders.Save(xmlWriter);
            }
            memory.Position = 0;

            // Возвращаем файл для скачивания
            return File(memory, "application/xml", "orders.xml");
        }
        public IActionResult CarsToXml()
        {
            XmlDocument x_cars = new XmlDocument();
            x_cars = Cars.CarsToXml();
            var memory = new MemoryStream();
            using (var xmlWriter = XmlWriter.Create(memory, new XmlWriterSettings { Indent = true }))
            {
                x_cars.Save(xmlWriter);
            }
            memory.Position = 0;

            // Возвращаем файл для скачивания
            return File(memory, "application/xml", "cars.xml");
        }
        public IActionResult OrdersCarsToXml()
        {
            XmlDocument x_orderCars = new XmlDocument();
            x_orderCars = OrdersCars.OrdersCarsToXml();
            var memory = new MemoryStream();
            using (var xmlWriter = XmlWriter.Create(memory, new XmlWriterSettings { Indent = true }))
            {
                x_orderCars.Save(xmlWriter);
            }
            memory.Position = 0;

            // Возвращаем файл для скачивания
            return File(memory, "application/xml", "orderscars.xml");
        }

        [HttpPost]
        public IActionResult XmlToWeb(IFormFile xmlFile)
        {
            if (xmlFile != null && xmlFile.Length > 0 && (Regex.Match(xmlFile.FileName, "\\.xml$").Success))
            {
                XmlDocument doc = new XmlDocument();
                using (var stream = xmlFile.OpenReadStream())
                {
                    // Чтение XML из потока
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        var xmlContent = reader.ReadToEnd();
                        // Здесь вы можете обработать XML-данные по вашему усмотрению
                        // Например, создать XmlDocument и провести операции с данными
                        doc.LoadXml(xmlContent);
                    }
                }
                //                string String = doc.InnerXml;
                XmlElement element = doc.DocumentElement;
                Car car;
                foreach (XmlElement _car in element.SelectNodes("Car"))
                {
                    car = new Car();
                    string FullName = _car.SelectSingleNode("FullName").InnerText;
                    string Mark = _car.SelectSingleNode("Mark").InnerText;
                    string MaxValue = _car.SelectSingleNode("MaxValue").InnerText;
                    string CarryTime = _car.SelectSingleNode("CarryTime").InnerText;
                    if(!Security.StringCheck(new string[] { FullName,Mark}) || !Security.NumberCheck(new string[] {MaxValue, CarryTime }))
                    {
                        throw new Exception();
                    }
                    car.FullName = FullName;
                    car.Mark = Mark;
                    car.MaxValue = int.Parse(MaxValue);
                    car.CarryTime = int.Parse(CarryTime);
                    car.Create_Car();
                }

            }
            else
            {
                ViewBag.Message = "Выберите XML-файл для загрузки.";
            }

            return RedirectToAction("CarList");
        }
    }
}
