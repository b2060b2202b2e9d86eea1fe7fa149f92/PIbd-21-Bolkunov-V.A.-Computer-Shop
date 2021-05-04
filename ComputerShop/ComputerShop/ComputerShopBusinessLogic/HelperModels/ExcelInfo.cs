using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.ViewModels;

namespace ComputerShopBusinessLogic.HelperModels
{
    class ExcelInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<ReportComputerComponentViewModel> ComputerComponents { get; set; }
        public List<ReportComputerViewModel> Computers { get; set; }
    }
}
