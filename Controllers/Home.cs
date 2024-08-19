using Elfie.Serialization;
using MVCE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Web.Administration;
using MVCE.Models;
using System.Web;
using System;
using System.Configuration;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Collections;
using MVCE.Server;
using Security = MVCE.Data.Security;

namespace MVCE.Controllers
{
    public class Home : Controller
    {
        public Home()
        {

        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                ViewData["layout"] = "_LayoutUser";
            }
            List<Car> cars = new List<Car>();
            cars = Cars.Select();
            return View(cars);
        }
        [HttpGet]
        public IActionResult Authorization()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                ViewData["Registration_Message"] = "You're already an authorizated user.";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Registration_Page()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                RedirectToAction("Index");
            }
            return View("Registration");
        }
        /// <summary>
        /// Вызывается для подтверждения уникальности аккаунта и выполнения регистрации
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Registration()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                return RedirectToActionPermanent("Index");
            }
            User user = new User();
            string Name = Request.Form["login"];
            string Password = Request.Form["password"];
            if (!Security.StringCheck(new string[] { Name, Password }))
            {
                ViewBag.Registration_Message = "You can use only letters and numbers.";
                return View(); 
            }
            user.Name = Name;
                if (user.Search_Name())
                {
                    ViewBag.Registration_Message = "This name is already used.";
                    return View();
                }
                user.Password = Password;
                user.Insert();
                return RedirectToAction("Index");

        }
        public IActionResult Order()
        {
            if (HttpContext.Session.GetString("user") == null)
            {
                return View("Authorization");
            }
            List<Car> cars = new List<Car>();
            cars = Cars.Select();
            return View(cars);
        }
        [HttpPost]
        public IActionResult MakingOrder(int[] CarIds)
        {
            if (HttpContext.Session.GetString("user") == null)
            {
                return View("Authorization");
            }
            if (CarIds.Length == 0)
            {
                ViewData["Error_Message"] = "You didn't choose cars.";
                return View("Order",Cars.Select());
            }
            List<Car> cars = new List<Car>();
            cars = Cars.Select(CarIds);
            return View(cars);
        }
        [HttpPost]
        public IActionResult CreateOrder(int[] CarIds, int[] Quantity)
        {
            if(!Security.NumberCheck(Quantity) || CarIds.Length!=Quantity.Length)
            {
                ViewData["Error_Message"] = "Use only positive and not empty integers.";
                return View("MakingOrder", Cars.Select(CarIds));
            }
            string UserName = HttpContext.Session.GetString("user");
            Car car;
            Order order = new Order(UserName);
            for (int i = 0; i < CarIds.Length; i++)
            {
                car = new Car(CarIds[i]);
                order.FinishTime += car.CarryTime * Quantity[i];
            }
            int OrderId = order.Insert();
            OrderCars ordersCars;
            for(int i=0; i<CarIds.Length;i++)
            {
                ordersCars = new OrderCars(CarIds[i], OrderId, Quantity[i]);
                ordersCars.Insert();
                
            }
            return RedirectToAction("User_Order");
        }
        [HttpPost]
        public IActionResult Author()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                ViewData["Registration_Message"] = "You're already an authorizated user.";
                return RedirectToAction("Index");
            }
            string Name = Request.Form["Name"];
            string Password = Request.Form["Password"];
            if(!Security.StringCheck(new string[] { Name, Password }))
            {
                ViewBag.Registration_Message = "You can use only letters and numbers.";
                return View("Authorization");
            }
                User user = new User(Name, Password);
                bool is_existed = user.Select();
                if (is_existed)
                {
                    switch (user.FullAccess)
                    {
                        case Type_Access.PartialAccess:
                            HttpContext.Session.SetString("user", user.Name);
                            break;
                        case Type_Access.FullAccess:
                            HttpContext.Session.SetString("admin", user.Name);
                            return RedirectToActionPreserveMethod("UserList", "Admin");

                    }
                    return RedirectToAction("User_Order");
                }
            ViewBag.Registration_Message = "User not found.";
                return View("Authorization");
        }
        /// <summary>
        /// Возвращает список заказов для текущего пользователя
        /// </summary>
        /// <returns></returns>
        public IActionResult User_Order()
        {
            string user = HttpContext.Session.GetString("user");

            List<Order> MyOrders = Orders.SelectByUser(user);
            List<OrderCars> cars = new List<OrderCars>();
            foreach (var order in MyOrders)
            {
                cars.AddRange(OrdersCars.GetOrdersCars(order.OrderID));
            }
            Dictionary<int, string> CarNames = new Dictionary<int, string>();
            CarNames = Cars.SelectNames();
            ViewBag.CarsbyOrder = cars;
            ViewBag.Names = CarNames;
            return View(MyOrders);
        }
        public IActionResult Exit()
        {
            if(HttpContext.Session.GetString("user")!=null)
            {
                HttpContext.Session.Remove("user");
            }
            return RedirectToAction("Index");
        }
        public IActionResult Edit_Profile()
        {
            string user_name= HttpContext.Session.GetString("user");
            if (user_name== null)
            {
                RedirectToActionPermanent("Index");
            }
            User user = new User(user_name);
            user.SearchByName();
            return View(user);
        }
        [HttpPost]
        public IActionResult Edit_User(string LastName, string LastPassword)
        {
            string Name = Request.Form["Name"];
            string Password = Request.Form["Password"];
            if (!Security.StringCheck(new string[] { Name, Password }))
            {
                ViewBag.Registration_Message = "You can use only letters and numbers.";
                User _user = new User(LastName, LastPassword);
                return View("Edit_Profile",_user);
            }
            User user = new User(Name, Password);
            if (user.Search_Name())
            {
                ViewBag.Registration_Message = "This name is already used.";
                return View("Edit_Profile", new User(LastName,LastPassword));
            }
                user.Update(LastName);
                HttpContext.Session.Remove("user");
                HttpContext.Session.SetString("user", user.Name);
                return RedirectToAction("User_Order");
        }
        [HttpPost]
        public IActionResult Delete_User(string user_name)
        {
            User user = new User(user_name);
            user.Delete();
            return View();
        }
        [HttpPost]
        public IActionResult Delete_Order(int OrderID)
        {
            Order order = new Order(OrderID);
            order.Delete();
            return RedirectToAction("User_Order");
        }
        
    }
}
