using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopListImplement.Models;

namespace ComputerShopListImplement.Implementations
{
    public class ComponentStorage : IComponentStorage
    {
        private readonly DataListSingleton dataSource;

        public ComponentStorage()
        {
            dataSource = DataListSingleton.GetInstance();
        }

        public List<ComponentViewModel> GetFullList()
        {
            List<ComponentViewModel> result = new List<ComponentViewModel>();
            foreach (var component in dataSource.Components)
            {
                result.Add(CreateModel(component));
            }
            return result;
        }

        public List<ComponentViewModel> GetFilteredList(ComponentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var result = new List<ComponentViewModel>();
            foreach (var component in dataSource.Components)
            {
                if (component.ComponentName.Contains(model.ComponentName))
                {
                    result.Add(CreateModel(component));
                }
            }
            return result;
        }

        public ComponentViewModel GetElement(ComponentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            foreach (var component in dataSource.Components)
            {
                if (component.Id == model.Id || component.ComponentName == model.ComponentName)
                {
                    return CreateModel(component);
                }
            }
            return null;
        }

        public void Insert(ComponentBindingModel model)
        {
            var temp = new Component { Id = 1 };
            foreach (var component in dataSource.Components)
            {
                if (component.ComponentName == model.ComponentName)
                {
                    throw new Exception("Компонент с таким названием уже существует");
                }
                if (component.Id >= temp.Id)
                {
                    temp.Id = component.Id + 1;
                }
            }
            dataSource.Components.Add(CreateModel(model, temp));
        }

        public void Update(ComponentBindingModel model)
        {
            Component temp = null;
            foreach (var component in dataSource.Components)
            {
                if (component.Id == model.Id)
                {
                    temp = component;
                }
                else if (component.ComponentName == model.ComponentName)
                {
                    throw new Exception("Компонент с таким названием уже существует");
                }
            }

            if (temp == null)
            {
                throw new Exception("Компонент не найден");
            }

            CreateModel(model, temp);
        }

        public void Delete(ComponentBindingModel model)
        {
            for (int i = 0; i < dataSource.Components.Count; ++i)
            {
                if (dataSource.Components[i].Id == model.Id.Value ||
                    dataSource.Components[i].ComponentName == model.ComponentName)
                {
                    dataSource.Components.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Компонент не найден");
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
