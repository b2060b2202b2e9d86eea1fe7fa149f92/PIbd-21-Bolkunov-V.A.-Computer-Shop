using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using ComputerShopBusinessLogic.Attributes;

namespace ComputerShopBusinessLogic.ViewModels
{
    [DataContract]
    public class ComputerViewModel
    {
        [Column(title: "Номер", width: 100)]
        [DataMember]
        public int Id { get; set; }

        //без GridViewAutoSize.AllCells те колонки, у которых назначен только GridViewAutoSize.Fill становятся не масштабируемыми
        [Column(title: "Название компьютера", width: 150, gridViewAutoSize: GridViewAutoSize.Fill & GridViewAutoSize.AllCells)]
        [DataMember]
        public string ComputerName { get; set; }

        [Column(title: "Цена", width: 100)]
        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public Dictionary<int, (string, int)> ComputerComponents { get; set; }
    }
}
