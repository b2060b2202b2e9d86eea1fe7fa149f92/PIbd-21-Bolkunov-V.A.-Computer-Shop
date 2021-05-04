using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.Enums;

namespace ComputerShopBusinessLogic.BusinessLogics
{
    public class ComputerLogic
    {
        private readonly IComputerStorage computerStorage;

        public ComputerLogic(IComputerStorage computerStorage)
        {
            this.computerStorage = computerStorage;
        }

        public List<ComputerViewModel> Read(ComputerBindingModel model)
        {
            if(model == null)
            {
                return computerStorage.GetFullList();
            }

            if(model.Id.HasValue)
            {
                return new List<ComputerViewModel> { computerStorage.GetElement(model) };
            }

            return computerStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(ComputerBindingModel model)
        {
            var comp = computerStorage.GetElement(model);

            if(comp != null && comp.Id != model.Id)
            {
                throw new Exception("Уже есть компьютер с таким названием");
            }

            if(model.Id.HasValue)
            {
                computerStorage.Update(model);
            }
            else
            {
                computerStorage.Insert(model);
            }
        }

        public void Delete(ComputerBindingModel model)
        {
            var comp = computerStorage.GetElement(model);

            if(comp == null)
            {
                throw new Exception("Элемент не найден");
            }

            computerStorage.Delete(model);
        }
    }
}
