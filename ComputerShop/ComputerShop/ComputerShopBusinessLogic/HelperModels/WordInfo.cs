using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.ViewModels;

namespace ComputerShopBusinessLogic.HelperModels
{
    class WordInfo
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<ComponentViewModel> Components { get; set; }

        public List<ComputerViewModel> Computers { get; set; }
    }
}
