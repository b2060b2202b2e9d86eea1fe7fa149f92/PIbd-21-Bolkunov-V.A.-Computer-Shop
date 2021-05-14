using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopListImplement.Models;

namespace ComputerShopListImplement.Implementations
{
    public class ComputerStorage : IComputerStorage
    {
        private readonly DataListSingleton dataSource;

        public ComputerStorage()
        {
            dataSource = DataListSingleton.GetInstance();
        }

        public List<ComputerViewModel> GetFullList()
        {
            var result = new List<ComputerViewModel>();
            foreach (var comp in dataSource.Computers)
            {
                result.Add(CreateModel(comp));
            }
            return result;
        }

        public List<ComputerViewModel> GetFilteredList(ComputerBindingModel model)
        {
            if(model == null)
            {
                return null;
            }

            var result = new List<ComputerViewModel>();
            foreach (var comp in dataSource.Computers)
            {
                if(comp.ComputerName.Contains(model.ComputerName))
                {
                    result.Add(CreateModel(comp));
                }
            }

            return result;
        }

        public ComputerViewModel GetElement(ComputerBindingModel model)
        {
            if(model == null)
            {
                return null;
            }

            foreach (var comp in dataSource.Computers)
            {
                if(comp.Id == model.Id || comp.ComputerName == model.ComputerName)
                {
                    return CreateModel(comp);
                }
            }

            return null;
        }

        public void Insert(ComputerBindingModel model)
        {
            var temp = new Computer { Id = 1, ComputerComponents = new Dictionary<int, int>() };
            foreach (var comp in dataSource.Computers)
            {
                if(comp.ComputerName == model.ComputerName)
                {
                    throw new Exception("Компьютер с таким названием уже существует");
                }
                if(comp.Id >= temp.Id)
                {
                    temp.Id = comp.Id + 1;
                }
            }
            dataSource.Computers.Add(CreateModel(model, temp));
        }

        public void Update(ComputerBindingModel model)
        {
            Computer temp = null;
            foreach (var comp in dataSource.Computers)
            {
                if(comp.Id == model.Id)
                {
                    temp = comp;
                }
                else if (comp.ComputerName == model.ComputerName)
                {
                    throw new Exception("Компьютер с таким названием уже существует");
                }
            }
            
            if(temp == null)
            {
                throw new Exception("Компьютер не найден");
            }
           
            CreateModel(model, temp);
        }

        public void Delete(ComputerBindingModel model)
        {
            for (int i = 0; i < dataSource.Computers.Count; ++i)
            {
                if(dataSource.Computers[i].Id == model.Id || 
                    dataSource.Computers[i].ComputerName == model.ComputerName)
                {
                    dataSource.Computers.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Компьютер не найден");
        }

        private Computer CreateModel(ComputerBindingModel model, Computer computer)
        {
            computer.ComputerName = model.ComputerName;
            computer.Price = model.Price;

            foreach (var key in computer.ComputerComponents.Keys.ToList())
            {
                if(!model.ComputerComponents.ContainsKey(key))
                {
                    computer.ComputerComponents.Remove(key);
                }
            }

            foreach (var component in model.ComputerComponents)
            {
                if(computer.ComputerComponents.ContainsKey(component.Key))
                {
                    computer.ComputerComponents[component.Key] = model.ComputerComponents[component.Key].Item2;
                }
                else
                {
                    computer.ComputerComponents.Add(component.Key, model.ComputerComponents[component.Key].Item2);
                }
            }

            return computer;
        }

        private ComputerViewModel CreateModel(Computer computer)
        {
            var computerComponents = new Dictionary<int, (string, int)>();

            foreach (var component in computer.ComputerComponents)
            {
                string componentName = string.Empty;
                foreach (var sourceComponent in dataSource.Components)
                {
                    if(component.Key == sourceComponent.Id)
                    {
                        componentName = sourceComponent.ComponentName;
                        break;
                    }
                }
                computerComponents.Add(component.Key, (componentName, component.Value));
            }

            return new ComputerViewModel
            {
                Id = computer.Id,
                ComputerName = computer.ComputerName,
                ComputerComponents = computerComponents,
                Price = computer.Price
            };
        }
    }
}
