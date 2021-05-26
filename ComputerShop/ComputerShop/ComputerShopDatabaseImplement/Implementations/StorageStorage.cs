using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopDatabaseImplement.Models;
using System.Linq;
using System.Linq.Expressions;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopDatabaseImplement.Models;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ComputerShopDatabaseImplement.Implementations
{
    public class StorageStorage : IStorageStorage
    {
        public List<StorageViewModel> GetFullList()
        {
            using (var context = new ComputerShopDatabase())
            {
                return context.Storages
                    .Include(stor => stor.ComponentCounts)
                    .ThenInclude(cc => cc.Component)
                    .ToList()
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public List<StorageViewModel> GetFilteredList(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ComputerShopDatabase())
            {
                return context.Storages
                    .Include(stor => stor.ComponentCounts)
                    .ThenInclude(cc => cc.Component)
                    .Where(stor => stor.StorageName.Contains(model.StorageName) 
                        || stor.OwnerName.Contains(model.OwnerName))
                    .ToList()
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public StorageViewModel GetElement(StorageBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ComputerShopDatabase())
            {
                var storage = context.Storages
                    .Include(stor => stor.ComponentCounts)
                    .ThenInclude(cc => cc.Component)
                    .ToList()
                    .FirstOrDefault(stor => stor.StorageName == model.StorageName
                        || stor.Id == model.Id);

                return storage == null ? null : CreateModel(storage);
            }
        }
        public bool HasComponents(StorageAddComponentBindingModel model)
        {
            if (model == null)
            {
                return false;
            }

            using (var context = new ComputerShopDatabase())
            {
                if (model.StorageID.HasValue)
                {
                    var storage = context.Storages
                        .Include(stor => stor.ComponentCounts)
                        .ThenInclude(cc => cc.Component)
                        .ToList()
                        .FirstOrDefault(stor => stor.Id == model.StorageID);

                    return storage == null ? 
                            false : 
                            storage.ComponentCounts.Exists
                                (sc => sc.ComponentId == model.ComponentID 
                                && sc.Count >= model.ComponentCount);
                }

                int requiredCount = model.ComponentCount;
                foreach(var stor in context.Storages)
                {
                    StorageComponent storComp = stor.ComponentCounts
                        .FirstOrDefault(sc => sc.ComponentId == model.ComponentID);
                    if(storComp != null)
                    {
                        if(storComp.Count >= requiredCount)
                        {
                            return true;
                        }
                        else
                        {
                            requiredCount -= storComp.Count;
                        }
                    }
                }
                return false;
            }
        }

        public void RemoveComponents(StorageAddComponentBindingModel model)
        {
            if (model == null)
            {
                return;
            }

            using (var context = new ComputerShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    if (model.StorageID.HasValue)
                    {
                        var storage = context.Storages
                            .Include(stor => stor.ComponentCounts)
                            .ThenInclude(cc => cc.Component)
                            .ToList()
                            .FirstOrDefault(stor => stor.Id == model.StorageID);

                        if (storage == null && storage.ComponentCounts
                            .Exists(sc => sc.ComponentId == model.ComponentID && sc.Count >= model.ComponentCount))
                        {
                            storage.ComponentCounts
                                .FirstOrDefault(sc => sc.ComponentId == model.ComponentID)
                                .Count -= model.ComponentCount;
                            context.SaveChanges();
                            transaction.Commit();
                            return;
                        }
                    }
                    else
                    {
                        int requiredCount = model.ComponentCount;
                        foreach (var stor in context.Storages
                            .Include(stor => stor.ComponentCounts)
                            .ThenInclude(cc => cc.Component))
                        {
                            StorageComponent storComp = stor.ComponentCounts
                                .FirstOrDefault(sc => sc.ComponentId == model.ComponentID);

                            if (storComp != null)
                            {
                                if (storComp.Count >= requiredCount)
                                {
                                    storComp.Count -= requiredCount;
                                    requiredCount = 0;
                                    context.SaveChanges();
                                    break;
                                }
                                else
                                {
                                    requiredCount -= storComp.Count;
                                    storComp.Count = 0;
                                    context.SaveChanges();
                                }
                            }
                        }

                        if (requiredCount == 0)
                        {
                            transaction.Commit();
                            return;
                        }
                    }
                    transaction.Rollback();
                    throw new Exception("На складах недостаточно компонентов");
                }
            }
        }

        public void Insert(StorageBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                if (context.Storages.Any(comp => comp.StorageName == model.StorageName))
                {
                    throw new Exception("Хранилище с таким названием уже существует");
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var storage = new Storage();
                        context.Storages.Add(storage);
                        CreateModel(model, storage, context);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(StorageBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                if (context.Storages.Any(comp => comp.Id != model.Id && comp.StorageName == model.StorageName))
                {
                    throw new Exception("Хранилище с таким названием уже существует");
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var storage = context.Storages.FirstOrDefault(comp => comp.Id == model.Id);

                        if (storage == null)
                        {
                            throw new Exception("Хранилище не найдено");
                        }

                        CreateModel(model, storage, context);

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(StorageBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                var storage = context.Storages
                    .FirstOrDefault(comp => comp.Id == model.Id || comp.StorageName == model.StorageName);

                if (storage == null)
                {
                    throw new Exception("Хранилище не найдено");
                }
                else
                {
                    context.Storages.Remove(storage);
                    context.SaveChanges();
                }
            }
        }

        private (string, int) CreateTuple(string str, int num)
        {
            return (str, num);
        }

        private StorageViewModel CreateModel(Storage storage)
        {
            return new StorageViewModel
            {
                Id = storage.Id,
                StorageName = storage.StorageName,
                OwnerName = storage.OwnerName,
                CreationTime = storage.CreationTime,
                ComponentCounts = storage.ComponentCounts
                            .ToDictionary
                            (
                                cc => cc.ComponentId,
                                cc => CreateTuple(cc.Component.ComponentName, cc.Count)
                            )
            };
        }

        private Storage CreateModel(StorageBindingModel model, Storage storage, ComputerShopDatabase context)
        {
            storage.StorageName = model.StorageName;
            storage.OwnerName = model.OwnerName;
            storage.CreationTime = model.CreationTime;

            if (model.Id.HasValue)
            {
                var storageComponents = context.StorageComponents.Where(sc => sc.StorageId == model.Id.Value).ToList();

                context.StorageComponents.RemoveRange
                    (storageComponents.Where(sc => !model.ComponentCounts.ContainsKey(sc.ComponentId)).ToList());
                context.SaveChanges();

                foreach (var updateComponent in storageComponents)
                {
                    updateComponent.Count = model.ComponentCounts[updateComponent.ComponentId].Item2;
                    model.ComponentCounts.Remove(updateComponent.ComponentId);
                }

                context.SaveChanges();
            }

            foreach (var cc in model.ComponentCounts)
            {
                context.StorageComponents.Add(new StorageComponent
                {
                    StorageId = storage.Id,
                    ComponentId = cc.Key,
                    Count = cc.Value.Item2,
                    Storage = storage,
                    Component = context.Components.FirstOrDefault(c => c.Id == cc.Key)
                });
                context.SaveChanges();
            }

            return storage;
        }
    }
}
