using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ComputerShopBusinessLogic.ViewModels
{
    [DataContract]
    public class ClientViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [DisplayName("Ф.И.О.")]
        public string ClientName { get; set; }

        [DataMember]
        [DisplayName("Электронная почта")]
        public string ClientLogin { get; set; }

        [DataMember]
        public string PasswordHash { get; set; }
    }
}
