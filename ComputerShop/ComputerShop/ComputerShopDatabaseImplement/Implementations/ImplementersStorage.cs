using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopDatabaseImplement.Models;

namespace ComputerShopDatabaseImplement.Implementations
{
    public class ImplementersStorage : IImplementerStorage
    {
        public List<ImplementerViewModel> GetFullList()
        {
            using (var context = new ComputerShopDatabase())
            {
                return context.Implementers
                    .Select(imp => new ImplementerViewModel
                    {
                        Id = imp.Id,
                        ImplementerName = imp.ImplementerName,
                        WorkingTime = imp.WorkingTime,
                        PauseTime = imp.PauseTime
                    }).ToList();
            }
        }

        public List<ImplementerViewModel> GetFilteredList(ImplementerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ComputerShopDatabase())
            {
                return context.Implementers
                    .Where(imp => imp.ImplementerName.Contains(model.ImplementerName))
                    .Select(imp => new ImplementerViewModel
                    {
                        Id = imp.Id,
                        ImplementerName = imp.ImplementerName,
                        WorkingTime = imp.WorkingTime,
                        PauseTime = imp.PauseTime
                    }).ToList();
            }
        }

        public ImplementerViewModel GetElement(ImplementerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ComputerShopDatabase())
            {
                var implementer = context.Implementers
                    .FirstOrDefault(imp => imp.Id == model.Id || imp.ImplementerName == model.ImplementerName);
                return implementer == null ? null : new ImplementerViewModel
                {
                    Id = implementer.Id,
                    ImplementerName = implementer.ImplementerName,
                    WorkingTime = implementer.WorkingTime,
                    PauseTime = implementer.PauseTime
                };
            }
        }

        public void Insert(ImplementerBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                if(context.Implementers.Any(imp => imp.ImplementerName == model.ImplementerName))
                {
                    throw new Exception("Исполнитель с таким именем уже существует");
                }

                context.Implementers.Add(CreateModel(model, new Implementer()));
                context.SaveChanges();
            }
        }

        public void Update(ImplementerBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                if(context.Implementers.Any(imp => imp.Id != model.Id && imp.ImplementerName == model.ImplementerName))
                {
                    throw new Exception("Исполнитель с таким именем уже существует");
                }

                var implementer = context.Implementers.FirstOrDefault(imp => imp.Id == model.Id);

                CreateModel(model, implementer);
                context.SaveChanges();
            }
        }

        public void Delete(ImplementerBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                var implementer = 
                    context.Implementers.FirstOrDefault(imp => imp.Id == model.Id || imp.ImplementerName == model.ImplementerName);

                if(implementer == null)
                {
                    throw new Exception("Исполнитель не найден");
                }

                context.Implementers.Remove(implementer);
                context.SaveChanges();
            }
        }

        private Implementer CreateModel(ImplementerBindingModel model, Implementer imp)
        {
            imp.ImplementerName = model.ImplementerName;
            imp.WorkingTime = model.WorkingTime;
            imp.PauseTime = model.PauseTime;
            return imp;
        }
    }
}
