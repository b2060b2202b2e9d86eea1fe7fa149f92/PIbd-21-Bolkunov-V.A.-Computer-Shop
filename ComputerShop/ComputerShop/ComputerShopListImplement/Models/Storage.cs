using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerShopListImplement.Models
{
    public class Storage
    {
        public int Id { get; set; }
        public string StorageName { get; set; }
        public string OwnerName { get; set; }
        public DateTime CreationTime { get; set; }
        public Dictionary<int, int> ComponentCounts { get; set; }
    }
}
