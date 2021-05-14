using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using ComputerShopBusinessLogic.Attributes;

namespace ComputerShopBusinessLogic.ViewModels
{
    [DataContract]
    public class ClientViewModel
    {
        [DataMember]
        [Column(title: "Номер", width: 100)]
        public int Id { get; set; }

        //без GridViewAutoSize.AllCells те колонки, у которых назначен только GridViewAutoSize.Fill становятся не масштабируемыми
        [DataMember]
        [Column(title: "Имя клиента", width: 150, gridViewAutoSize: GridViewAutoSize.Fill & GridViewAutoSize.AllCells)]
        public string ClientName { get; set; }

        [DataMember]
        [Column(title: "Электронная почта", width: 150)]
        public string ClientLogin { get; set; }

        [DataMember]
        [Column(title: "Пароль", width: 150)]
        public string PasswordHash { get; set; }
    }
}
