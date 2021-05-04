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
    public class ComputerStorage : IComputerStorage
    {
        private readonly FileDataListSingleton dataSource;

        public ComputerStorage()
        {
            dataSource = FileDataListSingleton.GetInstance();
        }

        public List<ComputerViewModel> GetFullList()
        {
            return dataSource.Computers.Select(CreateModel).ToList();
        }

        public List<ComputerViewModel> GetFilteredList(ComputerBindingModel model)
        {
            if(model == null)
            {
                return null;
            }

            return dataSource.Computers
                .Where(comp => comp.ComputerName == model.ComputerName || comp.Id == model.Id)
                .Select(CreateModel)
                .ToList();
        }

        public ComputerViewModel GetElement(ComputerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var computer = dataSource.Computers
                .FirstOrDefault(comp => comp.ComputerName == model.ComputerName || comp.Id == model.Id);
            return computer != null ? CreateModel(computer) : null;
        }

        public void Insert(ComputerBindingModel model)
        {
            if (dataSource.Computers.Exists(comp => comp.ComputerName == model.ComputerName))
            {
                throw new Exception("Компьютер с таким названием уже существует");
            }

            int maxId = dataSource.Computers.Count > 0 ? dataSource.Computers.Max(comp => comp.Id) : 0;
            var computer = new Computer { Id = maxId + 1, ComputerComponents = new Dictionary<int, int>() };
            dataSource.Computers.Add(CreateModel(model, computer));
        }

        public void Update(ComputerBindingModel model)
        {
            if (dataSource.Computers.Exists(comp => comp.Id != model.Id && comp.ComputerName == model.ComputerName))
            {
                throw new Exception("Компьютер с таким названием уже существует");
            }

            var computer = dataSource.Computers.FirstOrDefault(comp => comp.Id == model.Id);
            if (computer == null)
            {
                throw new Exception("Компьютер не найден");
            }

            CreateModel(model, computer);
        }
        public void Delete(ComputerBindingModel model)
        {
            var computer = dataSource.Computers
                .FirstOrDefault(comp => comp.Id == model.Id || comp.ComputerName == model.ComputerName);
            if (computer == null)
            {
                throw new Exception("Компьютер не найден");
            }
            dataSource.Computers.Remove(computer);
        }

        private Computer CreateModel(ComputerBindingModel model, Computer computer)
        {
            computer.ComputerName = model.ComputerName;
            computer.Price = model.Price;

            foreach (var key in computer.ComputerComponents.Keys.ToList())
            {
                if (!model.ComputerComponents.ContainsKey(key))
                {
                    computer.ComputerComponents.Remove(key);
                }
            }

            foreach (var component in model.ComputerComponents)
            {
                if (computer.ComputerComponents.ContainsKey(component.Key))
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
            return new ComputerViewModel
            {
                Id = computer.Id,
                ComputerName = computer.ComputerName,
                Price = computer.Price,
                ComputerComponents = computer.ComputerComponents.ToDictionary
                    (
                        comp => comp.Key,
                        comp => (dataSource.Components.FirstOrDefault(cmp => cmp.Id == comp.Key)?.ComponentName, comp.Value)
                    )
            };
        }
    }
}
