using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerShopBusinessLogic.ViewModels
{
    public class ReportComputerComponentViewModel
    {
        public string ComponentName { get; set; }

        public int TotalCount { get; set; }

        public List<(string, int)> Computers { get; set; }
    }
}
