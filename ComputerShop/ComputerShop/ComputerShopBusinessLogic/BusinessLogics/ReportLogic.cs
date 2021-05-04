using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.HelperModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.ViewModels;

namespace ComputerShopBusinessLogic.BusinessLogics
{
    public class ReportLogic
    {
        private readonly IComponentStorage componentStorage;

        private readonly IComputerStorage computerStorage;
        
        private readonly IOrderStorage orderStorage;

        public ReportLogic(IComponentStorage componentStorage, IComputerStorage computerStorage, IOrderStorage orderStorage)
        {
            this.componentStorage = componentStorage;
            this.computerStorage = computerStorage;
            this.orderStorage = orderStorage;
        }

        public List<ReportComputerViewModel> GetComputers()
        {
            var computers = computerStorage.GetFullList();
            var result = new List<ReportComputerViewModel>();

            foreach (var computer in computers)
            {
                result.Add(new ReportComputerViewModel
                {
                    ComputerName = computer.ComputerName,
                    ComputerComponents = computer.ComputerComponents.Values.ToList(),
                    TotalCount = computer.ComputerComponents.Values.Sum(pair => pair.Item2)
                });
            }
            
            return result;
        }

        public List<ReportComputerComponentViewModel> GetComputerComponents()
        {
            var components = componentStorage.GetFullList();
            var computers = computerStorage.GetFullList();
            var result = new List<ReportComputerComponentViewModel>();

            foreach(var component in components)
            {
                var report = new ReportComputerComponentViewModel
                {
                    ComponentName = component.ComponentName,
                    Computers = new List<(string, int)>(),
                    TotalCount = 0
                };

                foreach (var computer in computers)
                {
                    if(computer.ComputerComponents.ContainsKey(component.Id))
                    {
                        report.Computers.Add
                        ((
                            computer.ComputerName, 
                            computer.ComputerComponents[component.Id].Item2
                        ));
                        report.TotalCount += computer.ComputerComponents[component.Id].Item2;
                    }
                }

                result.Add(report);
            }

            return result;
        }

        public List<ReportOrderViewModel> GetOrders(ReportBindingModel model)
        {
            return orderStorage.GetFilteredList(new OrderBindingModel { DateFrom = model.DateFrom, DateTo = model.DateTo })
                .Select(rep => new ReportOrderViewModel
                {
                    DateCreate = rep.DateCreate,
                    ComputerName = rep.ComputerName,
                    Count = rep.Count,
                    Sum = rep.Sum,
                    Status = rep.Status
                })
                .ToList();
        }

        public void SaveComponentsToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список компонентов",
                Components = componentStorage.GetFullList(),
                Computers = computerStorage.GetFullList()
            });
        }

        public void SaveComputerComponentToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список компонентов",
                ComputerComponents = GetComputerComponents(),
                Computers = GetComputers()
            });
        }

        public void SaveOrderToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                Orders = GetOrders(model)
            });
        }
    }                      
}
