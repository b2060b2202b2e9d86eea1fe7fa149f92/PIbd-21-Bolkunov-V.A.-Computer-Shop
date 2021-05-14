using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace ComputerShopBusinessLogic.BindingModels
{
    [DataContract]
    public class ClientBindingModel
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string ClientLogin { get; set; }
        [DataMember]
        public string PasswordHash { get; set; }
    }
}
