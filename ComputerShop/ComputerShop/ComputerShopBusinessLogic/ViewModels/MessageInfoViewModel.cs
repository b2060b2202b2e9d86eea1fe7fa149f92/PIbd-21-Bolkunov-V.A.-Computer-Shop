using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using ComputerShopBusinessLogic.Attributes;

namespace ComputerShopBusinessLogic.ViewModels
{
    [DataContract]
    public class MessageInfoViewModel
    {
        [DataMember]
        [Column(title: "Идентификатор сообщения", width: 100)]
        public string MessageId { get; set; }

        [DataMember]
        [Column(title: "Отправитель", width: 120)]
        public string SenderName { get; set; }

        [DataMember]
        [Column(title: "Дата письма", width: 100)]
        public DateTime DateDelivery { get; set; }

        [DataMember]
        [Column(title: "Заголовок", width: 100)]
        public string Subject { get; set; }

        //без GridViewAutoSize.AllCells те колонки, у которых назначен только GridViewAutoSize.Fill становятся не масштабируемыми
        [DataMember]
        [Column(title: "Текст", width: 200, gridViewAutoSize: GridViewAutoSize.Fill & GridViewAutoSize.AllCells)]
        public string Body { get; set; }
    }
}
