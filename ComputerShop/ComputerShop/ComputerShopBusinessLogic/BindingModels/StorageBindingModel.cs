using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerShopBusinessLogic.BindingModels
{
    public class StorageBindingModel
    {
        public int? Id { get; set; }
        public string StorageName { get; set; }
        public string OwnerName { get; set; }
        public DateTime CreationTime { get; set; }
        public Dictionary<int, (string, int)> ComponentCounts { get; set; }
    }
}
