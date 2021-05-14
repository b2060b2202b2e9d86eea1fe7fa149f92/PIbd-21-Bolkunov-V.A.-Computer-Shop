using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using ComputerShopBusinessLogic.Enums;
using ComputerShopBusinessLogic.Attributes;

namespace ComputerShopBusinessLogic.ViewModels
{
    [DataContract]
    public class OrderViewModel
    {
        [Column(title: "Номер", width: 100)]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        public int? ImplementerId { get; set; }

        [DataMember]
        public int ComputerId {get; set;}

        [Column(title: "Клиент", width: 150)]
        [DataMember]
        public string ClientName { get; set; }

        [Column(title: "Исполнитель", width: 150)]
        [DataMember]
        public string ImplementerName { get; set; }

        //без GridViewAutoSize.AllCells те колонки, у которых назначен только GridViewAutoSize.Fill не масштабируемыми
        [Column(title: "Компьютер", width:150, gridViewAutoSize: GridViewAutoSize.Fill & GridViewAutoSize.AllCells)]
        [DataMember]
        public string ComputerName { get; set; }

        [Column(title: "Количество", width: 100)]
        [DataMember]
        public int Count { get; set; }

        [Column(title: "Сумма", width: 100)]
        [DataMember]
        public decimal Sum { get; set; }

        [Column(title: "Статус", width: 100)]
        [DataMember]
        public OrderStatus Status { get; set; }

        [Column(title: "Дата создания", width: 100)]
        [DataMember]
        public DateTime DateCreate { get; set; }

        [Column(title: "Дата выполнения", width: 100)]
        [DataMember]
        public DateTime? DateImplement { get; set; }

        [DataMember]
        public bool? FreeOrders { get; set; }
    }
}
