using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ComputerShopBusinessLogic.Enums;

namespace ComputerShopBusinessLogic.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int ComputerId {get; set;}

        [DisplayName("Компьютер")]
        public string ComputerName { get; set; }

        [DisplayName("Количество")]
        public int Count { get; set; }

        [DisplayName("Сумма")]
        public decimal Sum { get; set; }

        [DisplayName("Статус")]
        public OrderStatus Status { get; set; }

        [DisplayName("Дата создания")]
        public DateTime DateCreate { get; set; }

        [DisplayName("Дата выполнения")]
        public DateTime? DateImplement { get; set; }
    }
}
