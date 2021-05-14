using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.Enums;

namespace ComputerShopBusinessLogic.BusinessLogics
{
    public class OrderLogic
    {
        private readonly object locker = new object();

        private readonly IOrderStorage orderStorage;

        public OrderLogic(IOrderStorage orderStorage)
        {
            this.orderStorage = orderStorage;
        }

        public List<OrderViewModel> Read(OrderBindingModel model)
        {
            if(model == null)
            {
                return orderStorage.GetFullList();
            }

            if(model.Id.HasValue)
            {
                return new List<OrderViewModel> { orderStorage.GetElement(model) };
            }

            return orderStorage.GetFilteredList(model);
        }

        public void CreateOrder(CreateOrderBindingModel model)
        {
            orderStorage.Insert(new OrderBindingModel
            {
                ComputerId = model.ComputerId, ClientId = model.ClientId, ImplementerId = null,
                Count = model.Count, Sum = model.Sum, DateCreate = DateTime.Now, FreeOrders = true,
                Status = OrderStatus.Принят
            });
        }

        public void TakeOrderInWork(ChangeStatusBindingModel model)
        {
            lock (locker)
            {
                var order = orderStorage.GetElement(new OrderBindingModel { Id = model.OrderId });

                if (order == null)
                {
                    throw new Exception("Не найден заказ");
                }

                if (order.Status != OrderStatus.Принят)
                {
                    throw new Exception("Заказ не в статусе \"Принят\"");
                }

                if (!order.FreeOrders.HasValue || !order.FreeOrders.Value || order.ImplementerId.HasValue)
                {
                    throw new Exception("На заказ уже назначен исполнитель");
                }

                if (!model.ImplementerId.HasValue)
                {
                    throw new Exception("Отсутствует исполнитель, берущий заказ");
                }

                orderStorage.Update(new OrderBindingModel
                {
                    Id = order.Id,
                    ComputerId = order.ComputerId,
                    ClientId = order.ClientId,
                    ImplementerId = model.ImplementerId,
                    Count = order.Count,
                    Sum = order.Sum,
                    DateCreate = order.DateCreate,
                    DateImplement = order.DateImplement,
                    Status = OrderStatus.Выполняется,
                    FreeOrders = false
                });
            }
        }

        public void FinishOrder(ChangeStatusBindingModel model)
        {
            var order = orderStorage.GetElement(new OrderBindingModel { Id = model.OrderId });
            if(order == null)
            {
                throw new Exception("Не найден заказ");
            }
            if(order.Status != OrderStatus.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }
            orderStorage.Update(new OrderBindingModel
            {
                Id = order.Id,
                ComputerId = order.ComputerId,
                ClientId = order.ClientId,
                ImplementerId = order.ImplementerId,
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = OrderStatus.Готов,
                FreeOrders = order.FreeOrders
            });
        }

        public void PayOrder(ChangeStatusBindingModel model)
        {
            var order = orderStorage.GetElement(new OrderBindingModel { Id = model.OrderId });
            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }
            if (order.Status != OrderStatus.Готов)
            {
                throw new Exception("Заказ не в статусе \"Готов\"");
            }
            orderStorage.Update(new OrderBindingModel
            {
                Id = order.Id,
                ComputerId = order.ComputerId,
                ClientId = order.ClientId,
                ImplementerId = order.ImplementerId,
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = DateTime.Now,
                Status = OrderStatus.Оплачен,
                FreeOrders = order.FreeOrders
            });
        }
    }
}
