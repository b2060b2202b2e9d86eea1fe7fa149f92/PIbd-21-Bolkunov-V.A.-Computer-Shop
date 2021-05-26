using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerShopBusinessLogic.BindingModels
{
    public class StorageAddComponentBindingModel
    {
        public int? StorageID { get; set; }
        public int ComponentID { get; set; }
        public int ComponentCount { get; set; }
    }
}
