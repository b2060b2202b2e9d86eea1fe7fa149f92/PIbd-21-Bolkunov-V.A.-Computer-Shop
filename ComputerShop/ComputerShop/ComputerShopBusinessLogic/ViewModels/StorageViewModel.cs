using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ComputerShopBusinessLogic.ViewModels
{
    public class StorageViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название хранилища")]
        public string StorageName { get; set; }
        [DisplayName("ФИО ответственного")]
        public string OwnerName { get; set; }
        [DisplayName("Дата созадния")]
        public DateTime CreationTime { get; set; }
        public Dictionary<int, (string, int)> ComponentCounts { get; set; }
    }
}
