using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ComputerShopBusinessLogic.ViewModels
{
    public class ComputerViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название изделия")]
        public string ComputerName { get; set; }

        [DisplayName("Цена")]
        public decimal Price { get; set; }

        public Dictionary<int, (string, int)> ComputerComponents { get; set; }
    }
}
