﻿using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.Enums;

namespace ComputerShopBusinessLogic.BusinessLogics
{
    public class ComponentLogic
    {
        private readonly IComponentStorage componentStorage;

        public ComponentLogic(IComponentStorage componentStorage)
        {
            this.componentStorage = componentStorage;
        }

        public List<ComponentViewModel> Read(ComponentBindingModel model)
        {
            if (model == null)
            {
                return componentStorage.GetFullList();
            }

            if (model.Id.HasValue)
            {
                return new List<ComponentViewModel> { componentStorage.GetElement(model) };
            }

            return componentStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(ComponentBindingModel model)
        {
            var element = componentStorage
                .GetElement(new ComponentBindingModel { ComponentName = model.ComponentName });

            if(element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }

            if (model.Id.HasValue)
            {
                componentStorage.Update(model);
            }
            else
            {
                componentStorage.Insert(model);
            }
        }

        public void Delete(ComponentBindingModel model)
        {
            var element = componentStorage.GetElement(new ComponentBindingModel { Id = model.Id });
            
            if(element == null)
            {
                throw new Exception("Компонент не найден");
            }

            componentStorage.Delete(model);
        }
    }
}
