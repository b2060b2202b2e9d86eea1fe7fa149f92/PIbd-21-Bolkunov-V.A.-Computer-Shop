using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopListImplement.Models;
using System.Linq;

namespace ComputerShopListImplement.Implementations
{
    public class StorageStorage : IStorageStorage
    {
        private readonly DataListSingleton dataSource;

        public StorageStorage()
        {
            dataSource = DataListSingleton.GetInstance();
        }

        public StorageViewModel GetElement(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            foreach (var s in dataSource.Storages)
            {
                if (s.Id == model.Id)//???
                {
                    return CreateModel(s);
                }
            }
            return null;
        }

        public List<StorageViewModel> GetFilteredList(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var result = new List<StorageViewModel>();
            foreach (var s in dataSource.Storages)
            {
                if (s.StorageName.Contains(model.StorageName))
                {
                    result.Add(CreateModel(s));
                }
            }

            return result;
        }

        public List<StorageViewModel> GetFullList()
        {
            var result = new List<StorageViewModel>();
            foreach (var s in dataSource.Storages)
            {
                result.Add(CreateModel(s));
            }
            return result;
        }

        public void Insert(StorageBindingModel model)
        {
            var temp = new Storage() 
            { 
                Id = 1,
                ComponentCounts = new Dictionary<int, int>()
            };
            foreach (var item in dataSource.Storages)
            {
                if(item.StorageName == model.StorageName)
                {
                    throw new Exception("Хранилище с таким названием уже существует");
                }
                if(item.Id >= temp.Id)
                {
                    temp.Id = item.Id + 1;
                }
            }
            dataSource.Storages.Add(CreateModel(model, temp));
        }

        public void Update(StorageBindingModel model)
        {
            Storage temp = null;
            foreach (var stor in dataSource.Storages)
            {
                if (stor.Id == model.Id)
                {
                    temp = stor;
                }
                else if (stor.StorageName == model.StorageName)
                {
                    throw new Exception("Хранилище с таким названием уже существует");
                }
            }

            if (temp == null)
            {
                throw new Exception("Хранилище не найдено");
            }

            CreateModel(model, temp);
        }

        public void Delete(StorageBindingModel model)
        {
            for (int i = 0; i < dataSource.Storages.Count; ++i)
            {
                if (dataSource.Storages[i].Id == model.Id ||
                    dataSource.Storages[i].StorageName == model.StorageName)
                {
                    dataSource.Storages.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Хранилище не найдено");
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

        public bool HasComponents(StorageAddComponentBindingModel model)
        {
            throw new NotImplementedException();
        }

        public void RemoveComponents(StorageAddComponentBindingModel model)
        {
            throw new NotImplementedException();
        }
    }
}
