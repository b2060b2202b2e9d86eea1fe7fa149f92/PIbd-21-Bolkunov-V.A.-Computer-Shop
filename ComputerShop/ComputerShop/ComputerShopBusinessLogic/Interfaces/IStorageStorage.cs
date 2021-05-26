using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;

namespace ComputerShopBusinessLogic.Interfaces
{
    public interface IStorageStorage
    {
        List<StorageViewModel> GetFullList();
        List<StorageViewModel> GetFilteredList(StorageBindingModel model);
        StorageViewModel GetElement(StorageBindingModel model);
        void Insert(StorageBindingModel model);
        void Update(StorageBindingModel model);
        void Delete(StorageBindingModel model);
    }
}
