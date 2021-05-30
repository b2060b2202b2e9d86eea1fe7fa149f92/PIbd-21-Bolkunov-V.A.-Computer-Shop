using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.ViewModels;
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
                                ComputerName = context.Computers
                                    .FirstOrDefault(comp => comp.Id == ord.ComputerId).ComputerName,
                                Count = ord.Count,
                                Status = ord.Status,
                                Sum = ord.Sum,
                                DateCreate = ord.DateCreate,
                                DateImplement = ord.DateImplement
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
                    .Where(ord => ord.ComputerId == model.ComputerId || 
                            ord.DateCreate == model.DateCreate)//???
                    .ToList()
                    .Select<Order, OrderViewModel>
                        (
                            ord => new OrderViewModel
                            {
                                Id = ord.Id,
                                ComputerId = ord.ComputerId,
                                ComputerName = context.Computers
                                    .FirstOrDefault(comp => comp.Id == ord.ComputerId).ComputerName,
                                Count = ord.Count,
                                Status = ord.Status,
                                Sum = ord.Sum,
                                DateCreate = ord.DateCreate,
                                DateImplement = ord.DateImplement
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
                        Count = order.Count,
                        Status = order.Status,
                        Sum = order.Sum,
                        DateCreate = order.DateCreate,
                        DateImplement = order.DateImplement
                    };
                }
            }
        }

        public void Insert(OrderBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                context.Orders.Add(CreateModel(model, new Order()));
                context.SaveChanges();
            }
        }

        public void Update(OrderBindingModel model)
        {
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
            order.Count = model.Count;
            order.Status = model.Status;
            order.Sum = model.Sum;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;
            return order;
        }
    }
}
