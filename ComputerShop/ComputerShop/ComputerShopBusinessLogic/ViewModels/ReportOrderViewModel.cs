using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.Enums;

namespace ComputerShopBusinessLogic.ViewModels
{
    public class ReportOrderViewModel
    {
        public DateTime DateCreate { get; set; }

        public string ComputerName { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public OrderStatus Status { get; set; }
    }
}
