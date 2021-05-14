using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.Enums;
using ComputerShopFileImplement.Models;

namespace ComputerShopFileImplement.Implementations
{
    public class OrderStorage : IOrderStorage
    {
        private readonly FileDataListSingleton dataSource;

        public OrderStorage()
        {
            dataSource = FileDataListSingleton.GetInstance();
        }

        public List<OrderViewModel> GetFullList()
        {
            return dataSource.Orders.Select(CreateModel).ToList();
        }

        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if(model == null)
            {
                return null;
            }

            return dataSource.Orders
                .Where(ord =>
                    ((model.DateFrom != null &&
                                    (ord.DateCreate.Date >= model.DateFrom.Value.Date ||
                                    (ord.DateImplement != null &&
                                        ord.DateImplement.Value.Date >= model.DateFrom.Value.Date))) &&
                        (model.DateTo != null &&
                                    (ord.DateCreate.Date <= model.DateTo.Value.Date ||
                                    (ord.DateImplement != null &&
                                        ord.DateImplement.Value.Date <= model.DateTo.Value.Date))) ||
                         ord.DateCreate == model.DateCreate ||
                        (model.ClientId.HasValue && ord.ClientId == model.ClientId) ||
                        (model.FreeOrders.HasValue && model.FreeOrders.Value && ord.Status == OrderStatus.Принят) ||
                        (model.ImplementerId.HasValue && ord.ImplementerId == model.ImplementerId &&
                            ord.Status == OrderStatus.Выполняется)))
                .Select(CreateModel)
                .ToList();
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var order = dataSource.Orders.FirstOrDefault(ord => ord.Id == model.Id);///???
            return order == null ? null : CreateModel(order);
        }

        public void Insert(OrderBindingModel model)
        {
            if (model.ClientId == null)
            {
                throw new Exception("Клиент не указан");
            }

            int maxId = dataSource.Orders.Count > 0 ? dataSource.Orders.Max(ord => ord.Id) : 0;
            var order = new Order { Id = maxId + 1 };
            dataSource.Orders.Add(CreateModel(model, order));
        }

        public void Update(OrderBindingModel model)
        {
            if (model.ClientId == null)
            {
                throw new Exception("Клиент не указан");
            }

            var order = dataSource.Orders.FirstOrDefault(ord => ord.Id == model.Id);
            if(order == null)
            {
                throw new Exception("Заказ не найден");
            }
            CreateModel(model, order);
        }

        public void Delete(OrderBindingModel model)
        {
            var order = dataSource.Orders.FirstOrDefault(ord => ord.Id == model.Id);
            if (order == null)
            {
                throw new Exception("Заказ не найден");
            }
            else
            {
                dataSource.Orders.Remove(order);
            }
        }

        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.ComputerId = model.ComputerId;
            order.ClientId = model.ClientId.Value;
            order.ImplementerId = model.ImplementerId;
            order.Count = model.Count;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;
            order.Status = model.Status;
            order.Sum = model.Sum;
            order.FreeOrders = model.FreeOrders;
            return order;
        }

        private OrderViewModel CreateModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                ComputerId = order.ComputerId,
                ClientId = order.ClientId,
                ImplementerId = order.ImplementerId,
                ComputerName = dataSource.Computers.FirstOrDefault(comp => comp.Id == order.ComputerId)?.ComputerName,
                ClientName = dataSource.Clients.FirstOrDefault(client => client.Id == order.ClientId)?.ClientName,
                ImplementerName = dataSource.Implementers.FirstOrDefault(imp => imp.Id == order.ImplementerId)?.ImplementerName,
                Count = order.Count,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = order.Status,
                Sum = order.Sum,
                FreeOrders = order.FreeOrders
            };
        }
    }
}
