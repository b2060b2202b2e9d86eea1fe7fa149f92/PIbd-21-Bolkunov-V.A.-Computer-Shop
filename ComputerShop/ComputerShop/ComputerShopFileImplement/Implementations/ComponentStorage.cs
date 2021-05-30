using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopFileImplement.Models;

namespace ComputerShopFileImplement.Implementations
{
    public class ComponentStorage : IComponentStorage
    {
        private readonly FileDataListSingleton dataSource;

        public ComponentStorage()
        {
            dataSource = FileDataListSingleton.GetInstance();
        }

        public List<ComponentViewModel> GetFullList()
        {
            return dataSource.Components.Select(CreateModel).ToList();
        }

        public List<ComponentViewModel> GetFilteredList(ComponentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            return dataSource.Components
                .Where(comp => comp.ComponentName.Contains(model.ComponentName))
                .Select(CreateModel)
                .ToList();
        }

        public ComponentViewModel GetElement(ComponentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var component = dataSource.Components
                .FirstOrDefault(comp => comp.ComponentName == model.ComponentName || comp.Id == model.Id);

            return component != null ? CreateModel(component) : null;
        }

        public void Insert(ComponentBindingModel model)
        {
            if (dataSource.Components.Any(comp => comp.ComponentName == model.ComponentName))
            {
                throw new Exception("Компонент c таким названием уже существует");
            }

            int maxId = dataSource.Components.Count > 0 ? dataSource.Components.Max(comp => comp.Id) : 0;
            var element = new Component { Id = maxId + 1 };
            dataSource.Components.Add(CreateModel(model, element));
        }

        public void Update(ComponentBindingModel model)
        {
            if (dataSource.Components.Any(comp => comp.Id != model.Id && comp.ComponentName == model.ComponentName))
            {
                throw new Exception("Компонент c таким названием уже существует");
            }

            var element = dataSource.Components.FirstOrDefault(comp => comp.Id == model.Id);

            if (element == null)
            {
                throw new Exception("Компонент не найден");
            }

            CreateModel(model, element);
        }

        public void Delete(ComponentBindingModel model)
        {
            var element = dataSource.Components
                .FirstOrDefault(comp => comp.Id == model.Id || comp.ComponentName == model.ComponentName);
            if (element == null)
            {
                throw new Exception("Компонент не найден");
            }
            dataSource.Components.Remove(element);
        }

        private Component CreateModel(ComponentBindingModel model, Component component)
        {
            component.ComponentName = model.ComponentName;
            return component;
        }

        private ComponentViewModel CreateModel(Component component)
        {
            return new ComponentViewModel
            {
                Id = component.Id,
                ComponentName = component.ComponentName
            };
        }
    }
}
