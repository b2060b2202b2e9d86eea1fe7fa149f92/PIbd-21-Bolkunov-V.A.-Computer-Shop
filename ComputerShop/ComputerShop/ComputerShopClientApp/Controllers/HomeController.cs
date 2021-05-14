using ComputerShopClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;

namespace ComputerShopClientApp.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }

        public IActionResult Index()
        {
            if(Program.Client == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<OrderViewModel>>
                ($"api/main/getorders?clientId={Program.Client.Id}"));
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            if (Program.Client == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(Program.Client);
        }

        [HttpPost]
        public void Privacy(string login, string password, string name)
        {
            if(!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(name))
            {
                APIClient.PostRequest($"api/client/updateData", new ClientBindingModel
                { 
                    Id = Program.Client.Id,
                    ClientLogin = login,
                    ClientName = name,
                    PasswordHash = password
                });
                Program.Client.ClientName = name;
                Program.Client.ClientLogin = login;
                Program.Client.PasswordHash = password;
                Response.Redirect("Index");
                return;
            }
            throw new Exception("Введите логин, пароль и ФИО");
        }

        [HttpGet]
        public IActionResult Enter()
        {
            return View();
        }

        [HttpPost]
        public void Enter(string login, string password)
        {
            if(!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {
                Program.Client = 
                    APIClient.GetRequest<ClientViewModel>($"api/client/login?login={login}&password={password}");

                if(Program.Client == null)
                {
                    throw new Exception("Неверный логин/пароль");
                }

                Response.Redirect("Index");
                return;
            }
            throw new Exception("Введите логин и пароль");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public void Register(string login, string password, string name)
        {
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(name))
            {
                APIClient.PostRequest("api/client/register", new ClientBindingModel
                {
                    ClientName = name,
                    ClientLogin = login,
                    PasswordHash = password
                });
                Response.Redirect("Enter");
                return;
            }
            throw new Exception("Введите логин, пароль и ФИО");
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Computers = APIClient.GetRequest<List<ComputerViewModel>>("api/main/getcomputerlist");
            return View();
        }

        [HttpPost]
        public void Create(int computer, int count, decimal sum)
        {
            if (count == 0 || sum == 0 || Program.Client == null)
            {
                return;
            }
            APIClient.PostRequest("api/main/createorder", new CreateOrderBindingModel
            {
                ClientId = Program.Client.Id,
                ComputerId = computer,
                Count = count,
                Sum = sum
            });
            Response.Redirect("Index");
        }

        [HttpPost]
        public decimal Calc(decimal count, int computer)
        {
            var comp = APIClient.GetRequest<ComputerViewModel>($"api/main/getcomputer?computerId={computer}");
            return count * comp.Price;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
