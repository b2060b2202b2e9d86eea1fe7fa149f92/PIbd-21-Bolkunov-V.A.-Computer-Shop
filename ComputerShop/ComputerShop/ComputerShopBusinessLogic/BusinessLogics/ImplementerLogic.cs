using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.Enums;

namespace ComputerShopBusinessLogic.BusinessLogics
{
    public class ImplementerLogic
    {
        private readonly IImplementerStorage implementerStorage;

        public ImplementerLogic(IImplementerStorage implementerStorage)
        {
            this.implementerStorage = implementerStorage;
        }

        public List<ImplementerViewModel> Read(ImplementerBindingModel model)
        {
            if(model == null)
            {
                return implementerStorage.GetFullList();
            }

            if(model.Id.HasValue)
            {
                return new List<ImplementerViewModel> { implementerStorage.GetElement(model) };
            }

            return implementerStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(ImplementerBindingModel model)
        {
            var element = implementerStorage
                .GetElement(new ImplementerBindingModel { ImplementerName = model.ImplementerName });

            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть исполнитель с таким именем");
            }

            if(model.Id.HasValue)
            {
                implementerStorage.Update(model);
            }
            else
            {
                implementerStorage.Insert(model);
            }
        }

        public void Delete(ImplementerBindingModel model)
        {
            var element = implementerStorage.GetElement(new ImplementerBindingModel { Id = model.Id});

            if(element == null)
            {
                throw new Exception("Исполнитель не найден");
            }

            implementerStorage.Delete(model);
        }
    }
}
