using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopBusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ComputerShopRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientLogic clientLogic;

        public ClientController(ClientLogic clientLogic)
        {
            this.clientLogic = clientLogic;
        }

        [HttpGet]
        public ClientViewModel Login(string login, string password) => 
            clientLogic.Read(new ClientBindingModel { ClientLogin = login, PasswordHash = password})?[0];

        [HttpPost]
        public void Register(ClientBindingModel model) =>
            clientLogic.CreateOrUpdate(model);

        [HttpPost]
        public void UpdateData(ClientBindingModel model) =>
            clientLogic.CreateOrUpdate(model);
    }
}
