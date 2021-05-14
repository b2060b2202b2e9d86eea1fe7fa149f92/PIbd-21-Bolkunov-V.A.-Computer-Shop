using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.ViewModels;

namespace ComputerShopBusinessLogic.BusinessLogics
{
    public class WorkModeling
    {
        private readonly IImplementerStorage implementerStorage;
        private readonly IOrderStorage orderStorage;
        private readonly OrderLogic orderLogic;
        private readonly Random random;

        public WorkModeling(IImplementerStorage implementerStorage, IOrderStorage orderStorage, OrderLogic orderLogic)
        {
            random = new Random(1337);

            this.implementerStorage = implementerStorage;
            this.orderStorage = orderStorage;
            this.orderLogic = orderLogic;
        }

        public void DoWork()
        {
            var implementers = implementerStorage.GetFullList();
            var orders = orderStorage
                .GetFilteredList(new OrderBindingModel { FreeOrders = true });
            
            foreach (var imp in implementers)
            {
                WorkerWorkAsync(imp, orders);
            }
        }

        private async void WorkerWorkAsync(ImplementerViewModel implementer, List<OrderViewModel> orders)
        {
            var ordersToContinue = await Task.Run(() => 
                orderStorage.GetFilteredList(new OrderBindingModel { ImplementerId = implementer.Id }));

            await Task.Run(() =>
            {
                foreach (var ord in ordersToContinue)
                {
                    Thread.Sleep(implementer.WorkingTime * random.Next(1, 5) * orders.Count);

                    orderLogic.FinishOrder(new ChangeStatusBindingModel { OrderId = ord.Id });

                    Thread.Sleep(implementer.PauseTime);
                }
            });

            await Task.Run(() =>
            {
                foreach (var ord in orders)
                {
                    try
                    {
                        orderLogic.TakeOrderInWork(new ChangeStatusBindingModel
                        {
                            OrderId = ord.Id,
                            ImplementerId = implementer.Id
                        });

                        Thread.Sleep(implementer.WorkingTime * random.Next(1, 5) * orders.Count);

                        orderLogic.FinishOrder(new ChangeStatusBindingModel { OrderId = ord.Id });

                        Thread.Sleep(implementer.PauseTime);
                    }
                    catch (Exception) { }
                }
            });
        }
    }
}
