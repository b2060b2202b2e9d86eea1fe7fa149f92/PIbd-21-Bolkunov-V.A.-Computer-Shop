using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.Interfaces;
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
                .Where(ord => ord.ComputerId == model.ComputerId)//???
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
            int maxId = dataSource.Orders.Count > 0 ? dataSource.Orders.Max(ord => ord.Id) : 0;
            var order = new Order { Id = maxId + 1 };
            dataSource.Orders.Add(CreateModel(model, order));
        }

        public void Update(OrderBindingModel model)
        {
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
            order.Count = model.Count;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;
            order.Status = model.Status;
            order.Sum = model.Sum;
            return order;
        }

        private OrderViewModel CreateModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                ComputerId = order.ComputerId,
                ComputerName = dataSource.Computers.FirstOrDefault(comp => comp.Id == order.ComputerId)?.ComputerName,
                Count = order.Count,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = order.Status,
                Sum = order.Sum
            };
        }
    }
}
