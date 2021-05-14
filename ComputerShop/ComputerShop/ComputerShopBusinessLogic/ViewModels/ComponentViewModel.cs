using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ComputerShopBusinessLogic.ViewModels
{
    [DataContract]
    public class ComponentViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [DisplayName("Название компонента")]
        public string ComponentName { get; set; }
    }
}
