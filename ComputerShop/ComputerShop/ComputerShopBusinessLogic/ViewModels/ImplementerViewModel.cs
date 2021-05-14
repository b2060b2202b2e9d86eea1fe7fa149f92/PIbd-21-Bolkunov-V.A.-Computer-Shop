using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ComputerShopBusinessLogic.Attributes;

namespace ComputerShopBusinessLogic.ViewModels
{
    public class ImplementerViewModel
    {
        [Column(title: "Номер", width: 100)]
        public int Id { get; set; }

        //без GridViewAutoSize.AllCells те колонки, у которых назначен только GridViewAutoSize.Fill становятся не масштабируемыми
        [Column(title: "Исполнитель", width: 150, gridViewAutoSize: GridViewAutoSize.Fill & GridViewAutoSize.AllCells)]
        public string ImplementerName { get; set; }

        [Column(title: "Время на заказ", width: 100)]
        public int WorkingTime { get; set; }

        [Column(title: "Время на перерыв", width: 100)]
        public int PauseTime { get; set; }
    }
}
