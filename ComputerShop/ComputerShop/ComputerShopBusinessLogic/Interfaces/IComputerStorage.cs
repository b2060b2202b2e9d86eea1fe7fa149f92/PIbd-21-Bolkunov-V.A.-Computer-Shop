using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.BindingModels;

namespace ComputerShopBusinessLogic.Interfaces
{
    public interface IComputerStorage
    {
        List<ComputerViewModel> GetFullList();
        List<ComputerViewModel> GetFilteredList(ComputerBindingModel model);
        ComputerViewModel GetElement(ComputerBindingModel model);
        void Insert(ComputerBindingModel model);
        void Update(ComputerBindingModel model);
        void Delete(ComputerBindingModel model);
    }
}
