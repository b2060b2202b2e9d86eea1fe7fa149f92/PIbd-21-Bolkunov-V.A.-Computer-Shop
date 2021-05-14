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
    public class MainController : ControllerBase
    {
        private readonly OrderLogic orderLogic;

        private readonly ComputerLogic computerLogic;

        private readonly OrderLogic mainLogic;

        public MainController(OrderLogic orderLogic, ComputerLogic computerLogic, OrderLogic mainLogic)
        {
            this.orderLogic = orderLogic;
            this.computerLogic = computerLogic;
            this.mainLogic = mainLogic;
        }

        [HttpGet]
        public List<ComputerViewModel> GetComputerList() => 
            computerLogic.Read(null)?.ToList();
        
        [HttpGet]
        public ComputerViewModel GetComputer(int computerId) => 
            computerLogic.Read(new ComputerBindingModel { Id = computerId })?[0];

        [HttpGet]
        public List<OrderViewModel> GetOrders(int clientId) =>
            orderLogic.Read(new OrderBindingModel { ClientId = clientId });

        [HttpPost]
        public void CreateOrder(CreateOrderBindingModel model) =>
            mainLogic.CreateOrder(model);
    }
}
