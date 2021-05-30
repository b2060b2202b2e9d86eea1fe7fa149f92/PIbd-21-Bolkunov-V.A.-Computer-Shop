using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerShopDatabaseImplement.Models
{
    public class Storage
    {
        public int Id { get; set; }
        [Required]
        public string StorageName { get; set; }
        [Required]
        public string OwnerName { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        [ForeignKey("StorageId")]
        public virtual List<StorageComponent> ComponentCounts { get; set; }
    }
}
