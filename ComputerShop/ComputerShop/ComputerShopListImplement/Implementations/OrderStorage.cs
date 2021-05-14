using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopListImplement.Models;

namespace ComputerShopListImplement.Implementations
{
    public class OrderStorage: IOrderStorage
    {
        private readonly DataListSingleton dataSource;

        public OrderStorage()
        {
            dataSource = DataListSingleton.GetInstance();
        }

        public List<OrderViewModel> GetFullList()
        {
            var res = new List<OrderViewModel>();
            foreach (var ord in dataSource.Orders)
            {
                res.Add(CreateModel(ord));
            }
            return res;
        }

        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if(model == null)
            {
                return null;
            }

            var res = new List<OrderViewModel>();
            foreach (var ord in dataSource.Orders)
            {
                if ((model.DateFrom != null &&
                                    (ord.DateCreate.Date >= model.DateFrom.Value.Date ||
                                    (ord.DateImplement != null &&
                                        ord.DateImplement.Value.Date >= model.DateFrom.Value.Date))) &&
                        (model.DateTo != null &&
                                    (ord.DateCreate.Date <= model.DateTo.Value.Date ||
                                    (ord.DateImplement != null &&
                                        ord.DateImplement.Value.Date <= model.DateTo.Value.Date))) ||
                        (model.ClientId.HasValue &&
                        ord.ClientId == model.ClientId) ||
                        ord.DateCreate == model.DateCreate)
                {
                    res.Add(CreateModel(ord));
                }
            }
            return res;
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            foreach (var ord in dataSource.Orders)
            {
                if (ord.Id == model.Id)
                {
                    return CreateModel(ord);
                }
            }
            return null;
        }

        public void Insert(OrderBindingModel model)
        {
            if(model.ClientId == null)
            {
                throw new Exception("Клиент не указан");
            }
            var temp = new Order { Id = 1 };
            foreach (var ord in dataSource.Orders)
            {
                if(ord.Id >= temp.Id)
                {
                    temp.Id = ord.Id + 1;
                }
            }
            dataSource.Orders.Add(CreateModel(model, temp));
        }

        public void Update(OrderBindingModel model)
        {
            if (model.ClientId == null)
            {
                throw new Exception("Клиент не указан");
            }

            Order temp = null;
            foreach (var ord in dataSource.Orders)
            {
                if (ord.Id == model.Id)
                {
                    temp = ord;
                    break;
                }
            }

            if (temp == null)
            {
                throw new Exception("Заказ не найден");
            }

            CreateModel(model, temp);
        }

        public void Delete(OrderBindingModel model)
        {
            for(int i = 0; i < dataSource.Orders.Count; ++i)
            {
                if(dataSource.Orders[i].Id == model.Id)
                {
                    dataSource.Orders.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Заказ не найден");
        }

        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.ComputerId = model.ComputerId;
            order.ClientId = model.ClientId.Value;
            order.Count = model.Count;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;
            order.Status = model.Status;
            order.Sum = model.Sum;
            return order;
        }

        private OrderViewModel CreateModel(Order order)
        {
            foreach (var comp in dataSource.Computers)
            {
                if (comp.Id == order.ComputerId)
                {
                    foreach (var client in dataSource.Clients)
                    {
                        if (client.Id == order.ClientId)
                        {
                            return new OrderViewModel
                            {
                                Id = order.Id,
                                ComputerId = order.ComputerId,
                                ClientId = order.ClientId,
                                ComputerName = comp.ComputerName,
                                ClientName = client.ClientName,
                                Count = order.Count,
                                Sum = order.Sum,
                                Status = order.Status,
                                DateCreate = order.DateCreate,
                                DateImplement = order.DateImplement
                            };
                        }
                    }
                    throw new Exception("Клиент не найден");
                }
            }
            throw new Exception("Компьютер не найден");
        }
    }
}
