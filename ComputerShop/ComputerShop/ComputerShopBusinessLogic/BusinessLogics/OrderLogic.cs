using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.Enums;
using System.Linq;

namespace ComputerShopBusinessLogic.BusinessLogics
{
    public class OrderLogic
    {
        private readonly IOrderStorage orderStorage;
        private readonly IStorageStorage storageStorage;
        private readonly IComputerStorage computerStorage;

        public OrderLogic(IOrderStorage orderStorage, 
            IStorageStorage storageStorage, IComputerStorage computerStorage)
        {
            this.orderStorage = orderStorage;
            this.storageStorage = storageStorage;
            this.computerStorage = computerStorage;
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
                ComputerId = model.ComputerId, Count = model.Count,
                Sum = model.Sum, DateCreate = DateTime.Now,
                Status = OrderStatus.Принят
            });
        }

        public void TakeOrderInWork(ChangeStatusBindingModel model)
        {
            var order = orderStorage.GetElement(new OrderBindingModel { Id = model.OrderId });

            if(order == null)
            {
                throw new Exception("Не найден заказ");
            }
            if(order.Status != OrderStatus.Принят)
            {
                throw new Exception("Заказ не в статусе \"Принят\"");
            }
            var computer = computerStorage.GetElement(new ComputerBindingModel { Id = order.ComputerId});
            if(computer == null)
            {
                throw new Exception("Компьютер, принадлежащий этому заказу, больше не существует");
            }
            foreach (var kvp in computer.ComputerComponents)
            {
                storageStorage.RemoveComponents(new StorageAddComponentBindingModel
                    { ComponentID = kvp.Key, ComponentCount = kvp.Value.Item2 * order.Count });
            }
            orderStorage.Update(new OrderBindingModel
            {
                Id = order.Id,
                ComputerId = order.ComputerId,
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = OrderStatus.Выполняется
            });
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
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = OrderStatus.Готов
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
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = DateTime.Now,
                Status = OrderStatus.Оплачен
            });
        }
    }
}
