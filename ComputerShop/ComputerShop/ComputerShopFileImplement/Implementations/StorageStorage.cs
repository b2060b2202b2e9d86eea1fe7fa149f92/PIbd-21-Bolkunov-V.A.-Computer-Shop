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
    public class StorageStorage : IStorageStorage
    {
        private readonly FileDataListSingleton dataSource;

        public StorageStorage()
        {
            dataSource = FileDataListSingleton.GetInstance();
        }

        public List<StorageViewModel> GetFullList()
        {
            return dataSource.Storages.Select(CreateModel).ToList();
        }

        public List<StorageViewModel> GetFilteredList(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            return dataSource.Storages
                .Where(s => s.StorageName.Contains(model.StorageName))
                .Select(CreateModel)
                .ToList();
        }

        public StorageViewModel GetElement(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var storage = dataSource.Storages.FirstOrDefault(s => s.Id == model.Id || s.StorageName == model.StorageName);
            return storage == null ? null : CreateModel(storage);
        }

        public void Insert(StorageBindingModel model)
        {
            int maxId = dataSource.Storages.Count > 0 ? dataSource.Storages.Max(s => s.Id) : 0;
            var storage = new Storage() { Id = maxId + 1 };
            dataSource.Storages.Add(CreateModel(model, storage));
        }

        public void Update(StorageBindingModel model)
        {
            var storage = dataSource.Storages.FirstOrDefault(s => s.Id == model.Id);
            if(storage == null)
            {
                throw new Exception("Хранилище не найдено");
            }
            CreateModel(model, storage);
        }

        public void Delete(StorageBindingModel model)
        {
            var storage = dataSource.Storages.FirstOrDefault(s => s.Id == model.Id);
            if (storage == null)
            {
                throw new Exception("Хранилище не найдено");
            }
            else
            {
                dataSource.Storages.Remove(storage);
            }    
        }

        public bool HasComponents(StorageAddComponentBindingModel model)
        {
            if(model == null)
            {
                return false;
            }

            if(model.StorageID.HasValue)
            {
                var storage = dataSource.Storages.FirstOrDefault(s => s.Id == model.StorageID);
                if (storage == null)
                {
                    throw new Exception("Хранилище не найдено");
                }
                return storage.ComponentCounts.ContainsKey(model.ComponentID) ?
                    storage.ComponentCounts[model.ComponentID] >= model.ComponentCount :
                    false;
            }

            int count = 0;
            foreach (var storage in dataSource.Storages)
            {
                if(storage.ComponentCounts.ContainsKey(model.ComponentID))
                {
                    count += storage.ComponentCounts[model.ComponentID];
                    if(count >= model.ComponentCount)
                    {
                        return true;
                    }
                }    
            }

            return false;
        }

        public void RemoveComponents(StorageAddComponentBindingModel model)
        {
            if (model == null)
            {
                return;
            }

            if (model.StorageID.HasValue)
            {
                var storage = dataSource.Storages.FirstOrDefault(s => s.Id == model.StorageID);
                if (storage == null)
                {
                    throw new Exception("Хранилище не найдено");
                }
                if (storage.ComponentCounts.ContainsKey(model.ComponentID) && 
                    storage.ComponentCounts[model.ComponentID] >= model.ComponentCount)
                {
                    storage.ComponentCounts[model.ComponentID] -= model.ComponentCount;
                }
            }

            int removeCount = model.ComponentCount;
            foreach (var storage in dataSource.Storages)
            {
                if (storage.ComponentCounts.ContainsKey(model.ComponentID))
                {
                    if (storage.ComponentCounts[model.ComponentID] >= removeCount)
                    {
                        storage.ComponentCounts[model.ComponentID] -= removeCount;
                        return;
                    }
                    else
                    {
                        removeCount -= storage.ComponentCounts[model.ComponentID];
                        storage.ComponentCounts[model.ComponentID] = 0;
                    }
                }
            }
        }

        private Storage CreateModel(StorageBindingModel model, Storage storage)
        {
            storage.OwnerName = model.OwnerName;
            storage.StorageName = model.StorageName;
            storage.CreationTime = model.CreationTime;
            storage.ComponentCounts = model.ComponentCounts
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Item2);
            return storage;
        }

        private StorageViewModel CreateModel(Storage model)
        {
            var dict = new Dictionary<int, (string, int)>();
            foreach (var item in model.ComponentCounts)
            {
                dict.Add
                (
                    item.Key,
                    (
                        dataSource.Components.Find(c => c.Id == item.Key).ComponentName,
                        item.Value
                    )
                );
            }
            return new StorageViewModel()
            {
                Id = model.Id,
                StorageName = model.StorageName,
                OwnerName = model.OwnerName,
                CreationTime = model.CreationTime,
                ComponentCounts = dict
            };
        }
    }
}
