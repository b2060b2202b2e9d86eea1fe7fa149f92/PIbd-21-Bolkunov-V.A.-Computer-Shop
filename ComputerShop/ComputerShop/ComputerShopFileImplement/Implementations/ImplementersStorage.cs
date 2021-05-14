using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopFileImplement.Models;

namespace ComputerShopFileImplement.Implementations
{
    public class ImplementersStorage : IImplementerStorage
    {
        private readonly FileDataListSingleton dataSource;

        public ImplementersStorage()
        {
            dataSource = FileDataListSingleton.GetInstance();
        }

        public List<ImplementerViewModel> GetFullList()
        {
            return dataSource.Implementers.Select(CreateModel).ToList();
        }

        public List<ImplementerViewModel> GetFilteredList(ImplementerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            return dataSource.Implementers
                .Where(imp => imp.ImplementerName.Contains(model.ImplementerName))
                .Select(CreateModel)
                .ToList();
        }
        public ImplementerViewModel GetElement(ImplementerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var imp = dataSource.Implementers
                .FirstOrDefault(i => i.ImplementerName == model.ImplementerName || i.Id == model.Id);

            return imp == null ? null : CreateModel(imp);
        }

        public void Insert(ImplementerBindingModel model)
        {
            if (dataSource.Implementers.Exists(imp => imp.ImplementerName == model.ImplementerName))
            {
                throw new Exception("Испольнитель с таким именем уже существует");
            }

            int maxId = dataSource.Clients.Count > 0 ? dataSource.Implementers.Max(imp => imp.Id) : 0;
            dataSource.Implementers.Add(CreateModel(model, new Implementer { Id = maxId + 1 }));
        }

        public void Update(ImplementerBindingModel model)
        {
            if (dataSource.Implementers.Exists(imp => imp.Id != model.Id && imp.ImplementerName == model.ImplementerName))
            {
                throw new Exception("Испольнитель с таким именем уже существует");
            }

            var implementer = dataSource.Implementers.FirstOrDefault(imp => imp.Id == model.Id);
            if(implementer == null)
            {
                throw new Exception("Исполнитель не найден");
            }

            CreateModel(model, implementer);
        }

        public void Delete(ImplementerBindingModel model)
        {
            var implementer = dataSource.Implementers
                .FirstOrDefault(imp => imp.Id == model.Id || imp.ImplementerName == model.ImplementerName);
            if (implementer == null)
            {
                throw new Exception("Исполнитель не найден");
            }
            dataSource.Implementers.Remove(implementer);
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
