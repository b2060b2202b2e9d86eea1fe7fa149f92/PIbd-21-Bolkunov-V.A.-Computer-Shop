using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace ComputerShopBusinessLogic.BindingModels
{
    [DataContract]
    public class ImplementerBindingModel
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public string ImplementerName { get; set; }
        [DataMember]
        public int WorkingTime { get; set; }
        [DataMember]
        public int PauseTime { get; set; }
    }
}
