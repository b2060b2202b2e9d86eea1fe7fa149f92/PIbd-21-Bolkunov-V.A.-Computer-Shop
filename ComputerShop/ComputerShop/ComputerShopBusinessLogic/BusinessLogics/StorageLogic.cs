using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.Enums;

namespace ComputerShopBusinessLogic.BusinessLogics
{
    public class StorageLogic
    {
        private readonly IStorageStorage storagesStorage;
        private readonly IComponentStorage componentStorage;

        public StorageLogic(IStorageStorage storagesStorage, IComponentStorage componentStorage)
        {
            this.storagesStorage = storagesStorage;
            this.componentStorage = componentStorage;
        }

        public List<StorageViewModel> Read(StorageBindingModel model)
        {
            if (model == null)
            {
                return storagesStorage.GetFullList();
            }

            if (model.Id.HasValue)
            {
                return new List<StorageViewModel> { storagesStorage.GetElement(model) };
            }

            return storagesStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(StorageBindingModel model)
        {
            var comp = storagesStorage.GetElement(model);

            if (comp != null && comp.Id != model.Id)
            {
                throw new Exception("Уже есть хранилище с таким названием");
            }

            if (model.Id.HasValue)
            {
                storagesStorage.Update(model);
            }
            else
            {
                storagesStorage.Insert(model);
            }
        }

        public void Delete(StorageBindingModel model)
        {
            var comp = storagesStorage.GetElement(model);

            if (comp == null)
            {
                throw new Exception("Элемент не найден");
            }

            storagesStorage.Delete(model);
        }

        public void AddCountOrAddComponent(StorageAddComponentBindingModel model)
        {
            if(model != null && model.ComponentCount >= 0)
            {
                StorageViewModel vm = storagesStorage
                    .GetElement(new StorageBindingModel() { Id = model.StorageID });
                var dict = vm.ComponentCounts;
                if(dict.ContainsKey(model.ComponentID))
                {
                    dict[model.ComponentID] = 
                        (dict[model.ComponentID].Item1, 
                        dict[model.ComponentID].Item2 + model.ComponentCount);
                }
                else
                {
                    dict.Add
                        (model.ComponentID, 
                        (componentStorage.GetElement
                            (new ComponentBindingModel() { Id = model.ComponentID}).ComponentName,
                            model.ComponentCount));
                }
                storagesStorage.Update(new StorageBindingModel()
                { 
                    Id = model.StorageID, 
                    StorageName = vm.StorageName, 
                    OwnerName = vm.OwnerName, 
                    CreationTime = vm.CreationTime, 
                    ComponentCounts = dict 
                });
            }
        }
    }
}
