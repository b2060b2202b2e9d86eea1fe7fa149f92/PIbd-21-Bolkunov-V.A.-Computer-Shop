using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopDatabaseImplement.Models;
using System.Linq;

namespace ComputerShopDatabaseImplement.Implementations
{
    public class ComponentStorage : IComponentStorage
    {
        public List<ComponentViewModel> GetFullList()
        {
            using (var context = new ComputerShopDatabase())
            {
                return context.Components.Select
                    (
                        comp => new ComponentViewModel
                        {
                            Id = comp.Id,
                            ComponentName = comp.ComponentName
                        }
                    ).ToList();
            }
        }
        public List<ComponentViewModel> GetFilteredList(ComponentBindingModel model)
        {
            if(model == null)
            {
                return null;
            }

            using (var context = new ComputerShopDatabase())
            {
                return context.Components
                    .Where(comp => comp.ComponentName.Contains(model.ComponentName))
                    .Select
                    (
                        comp => new ComponentViewModel
                        {
                            Id = comp.Id,
                            ComponentName = comp.ComponentName
                        }
                    ).ToList();
            }
        }

        public ComponentViewModel GetElement(ComponentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ComputerShopDatabase())
            {
                var component = context.Components
                    .FirstOrDefault(comp => comp.ComponentName == model.ComponentName || comp.Id == model.Id);

                return component == null ?
                    null :
                    new ComponentViewModel
                    {
                        Id = component.Id,
                        ComponentName = component.ComponentName
                    };

            }
        }

        public void Insert(ComponentBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                if (context.Components.Any(c => c.ComponentName == model.ComponentName))
                {
                    throw new Exception("Компонент с таким названием уже существует");
                }

                context.Components.Add(CreateModel(model, new Component()));
                context.SaveChanges();
            }
        }

        public void Update(ComponentBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                if (context.Components.Any(c => c.Id != model.Id && c.ComponentName == model.ComponentName))
                {
                    throw new Exception("Компонент с таким названием уже существует");
                }

                var component = context.Components
                    .FirstOrDefault(comp => comp.Id == model.Id);

                if (component == null)
                {
                    throw new Exception("Компонент не найден");
                }

                CreateModel(model, component);
                context.SaveChanges();
            }
        }

        public void Delete(ComponentBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                var component = context.Components
                    .FirstOrDefault(comp => comp.Id == model.Id || comp.ComponentName == model.ComponentName);

                if (component == null)
                {
                    throw new Exception("Компонент не найден");
                }
                else
                {
                    context.Components.Remove(component);
                    context.SaveChanges();
                }
            }
        }

        private Component CreateModel(ComponentBindingModel model, Component component)
        {
            component.ComponentName = model.ComponentName;
            return component;
        }
    }
}
