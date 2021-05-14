using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ComputerShopBusinessLogic.ViewModels
{
    public class ImplementerViewModel
    {
        public int Id { get; set; }

        [DisplayName("ФИО исполнителя")]
        public string ImplementerName { get; set; }

        [DisplayName("Время на заказ")]
        public int WorkingTime { get; set; }

        [DisplayName("Время на перерыв")]
        public int PauseTime { get; set; }
    }
}
