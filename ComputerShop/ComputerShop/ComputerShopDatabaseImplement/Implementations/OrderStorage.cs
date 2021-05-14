using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.Enums;
using ComputerShopDatabaseImplement.Models;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ComputerShopDatabaseImplement.Implementations
{
    public class OrderStorage : IOrderStorage
    {
        public List<OrderViewModel> GetFullList()
        {
            using (var context = new ComputerShopDatabase())
            {
                return context.Orders
                    .Select<Order, OrderViewModel>
                        (
                            ord => new OrderViewModel
                            {
                                Id = ord.Id,
                                ComputerId = ord.ComputerId,
                                ClientId = ord.ClientId,
                                ImplementerId = ord.ImplementerId,
                                ComputerName = context.Computers
                                    .FirstOrDefault(comp => comp.Id == ord.ComputerId).ComputerName,
                                ClientName = context.Clients
                                    .FirstOrDefault(c => c.Id == ord.ClientId).ClientName,
                                ImplementerName = context.Implementers
                                    .FirstOrDefault(imp => imp.Id == ord.ImplementerId).ImplementerName,
                                Count = ord.Count,
                                Status = ord.Status,
                                Sum = ord.Sum,
                                DateCreate = ord.DateCreate,
                                DateImplement = ord.DateImplement,
                                FreeOrders = ord.FreeOrders
                            }
                        )
                    .ToList();
            }
        }

        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if(model == null)
            {
                return null;
            }

            using (var context = new ComputerShopDatabase())
            {
                return context.Orders
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
                    .ToList()
                    .Select
                        (
                            ord => new OrderViewModel
                            {
                                Id = ord.Id,
                                ComputerId = ord.ComputerId,
                                ComputerName = context.Computers
                                    .FirstOrDefault(comp => comp.Id == ord.ComputerId)?.ComputerName,
                                ClientId = ord.ClientId,
                                ClientName = context.Clients
                                    .FirstOrDefault(c => c.Id == ord.ClientId)?.ClientName,
                                ImplementerId = ord.ImplementerId,
                                ImplementerName = context.Implementers
                                    .FirstOrDefault(imp => imp.Id == ord.ImplementerId)?.ImplementerName,
                                Count = ord.Count,
                                Status = ord.Status,
                                Sum = ord.Sum,
                                DateCreate = ord.DateCreate,
                                DateImplement = ord.DateImplement,
                                FreeOrders = ord.FreeOrders
                            }
                        )
                    .ToList();
            }
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ComputerShopDatabase())
            {
                var order = context.Orders.FirstOrDefault(ord => ord.Id == model.Id);
                
                if(order == null)
                {
                    return null;
                }
                else
                {
                    return new OrderViewModel
                    {
                        Id = order.Id,
                        ComputerId = order.ComputerId,
                        ComputerName = context.Computers
                            .FirstOrDefault(comp => comp.Id == order.ComputerId)?.ComputerName,
                        ClientId = order.ClientId,
                        ClientName = context.Clients
                                .FirstOrDefault(c => c.Id == order.ClientId)?.ClientName,
                        ImplementerId = order.ImplementerId,
                        ImplementerName = context.Implementers
                                    .FirstOrDefault(imp => imp.Id == order.ImplementerId)?.ImplementerName,
                        Count = order.Count,
                        Status = order.Status,
                        Sum = order.Sum,
                        DateCreate = order.DateCreate,
                        DateImplement = order.DateImplement,
                        FreeOrders = order.FreeOrders
                    };
                }
            }
        }

        public void Insert(OrderBindingModel model)
        {
            if (model.ClientId == null)
            {
                throw new Exception("Клиент не указан");
            }

            using (var context = new ComputerShopDatabase())
            {
                context.Orders.Add(CreateModel(model, new Order()));
                context.SaveChanges();
            }
        }

        public void Update(OrderBindingModel model)
        {
            if (model.ClientId == null)
            {
                throw new Exception("Клиент не указан");
            }

            using (var context = new ComputerShopDatabase())
            {
                var order = context.Orders.FirstOrDefault(ord => ord.Id == model.Id);

                if(order == null)
                {
                    throw new Exception("Заказ не найден");
                }
                else
                {
                    CreateModel(model, order);
                    context.SaveChanges();
                }
            }
        }

        public void Delete(OrderBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                var order = context.Orders.FirstOrDefault(ord => ord.Id == model.Id);

                if (order == null)
                {
                    throw new Exception("Заказ не найден");
                }

                context.Orders.Remove(order);
                context.SaveChanges();
            }
        }

        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.ComputerId = model.ComputerId;
            order.ClientId = model.ClientId.Value;
            order.ImplementerId = model.ImplementerId;
            order.Count = model.Count;
            order.Status = model.Status;
            order.Sum = model.Sum;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;
            order.FreeOrders = model.FreeOrders;
            return order;
        }
    }
}
