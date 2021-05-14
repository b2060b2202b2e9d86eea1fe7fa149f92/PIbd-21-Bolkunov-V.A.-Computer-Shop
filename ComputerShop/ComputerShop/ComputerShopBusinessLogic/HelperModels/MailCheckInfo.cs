using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.Interfaces;

namespace ComputerShopBusinessLogic.HelperModels
{
    public class MailCheckInfo
    {
        public string PopHost { get; set; }
        public int PopPort { get; set; }
        public IMessageInfoStorage Storage { get; set; }
        public IClientStorage ClientStorage { get; set; }
    }
}
