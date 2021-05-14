using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopListImplement.Models;

namespace ComputerShopListImplement.Implementations
{
    public class ImplementersStorage : IImplementerStorage
    {
        private readonly DataListSingleton dataSource;

        public ImplementersStorage()
        {
            dataSource = DataListSingleton.GetInstance();
        }

        public List<ImplementerViewModel> GetFullList()
        {
            var res = new List<ImplementerViewModel>();
            foreach(var imp in dataSource.Implementers)
            {
                res.Add(CreateModel(imp));
            }
            return res;
        }

        public List<ImplementerViewModel> GetFilteredList(ImplementerBindingModel model)
        {
            if(model == null)
            {
                return null;
            }

            var res = new List<ImplementerViewModel>();
            foreach (var imp in dataSource.Implementers)
            {
                if (imp.ImplementerName.Contains(model.ImplementerName))
                {
                    res.Add(CreateModel(imp));
                }
            }
            return res;
        }

        public ImplementerViewModel GetElement(ImplementerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            foreach (var imp in dataSource.Implementers)
            {
                if (imp.ImplementerName == model.ImplementerName || imp.Id == model.Id)
                {
                    return CreateModel(imp);
                }
            }
            return null;
        }

        public void Insert(ImplementerBindingModel model)
        {
            var temp = new Implementer { Id = 1 };
            foreach (var imp in dataSource.Implementers)
            {
                if (imp.ImplementerName == model.ImplementerName)
                {
                    throw new Exception("Исполнитель с таким ФИО уже существует");
                }

                if (imp.Id >= temp.Id)
                {
                    temp.Id = imp.Id + 1;
                }
            }
            dataSource.Implementers.Add(CreateModel(model, temp));
        }

        public void Update(ImplementerBindingModel model)
        {
            Implementer temp = null;
            foreach (var imp in dataSource.Implementers)
            {
                if (imp.Id == model.Id)
                {
                    temp = imp;
                }
                else if(imp.ImplementerName == model.ImplementerName)
                {
                    throw new Exception("Исполнитель с таким ФИО уже существует");
                }
            }

            if(temp == null)
            {
                throw new Exception("Исполнитель не найден");
            }
            CreateModel(model, temp);
        }

        public void Delete(ImplementerBindingModel model)
        {
            for (int i = 0; i < dataSource.Implementers.Count; i++)
            {
                if (dataSource.Implementers[i].Id == model.Id || 
                    dataSource.Implementers[i].ImplementerName == model.ImplementerName)
                {
                    dataSource.Implementers.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Исполнитель не найден");
        }

        private Implementer CreateModel(ImplementerBindingModel model, Implementer imp)
        {
            imp.ImplementerName = model.ImplementerName;
            imp.WorkingTime = model.WorkingTime;
            imp.PauseTime = model.PauseTime;
            return imp;
        }

        private ImplementerViewModel CreateModel(Implementer imp)
        {
            return new ImplementerViewModel
            {
                Id = imp.Id,
                ImplementerName = imp.ImplementerName,
                WorkingTime = imp.WorkingTime,
                PauseTime = imp.PauseTime
            };
        }
    }
}
