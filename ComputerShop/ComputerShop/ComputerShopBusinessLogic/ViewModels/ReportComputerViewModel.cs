using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerShopBusinessLogic.ViewModels
{
    public class ReportComputerViewModel
    {
        public string ComputerName { get; set; }

        public List<(string, int)> ComputerComponents { get; set; }

        public int TotalCount { get; set; }
    }
}
