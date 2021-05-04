using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerShopDatabaseImplement.Models
{
    public class Computer
    {
        public int Id { get; set; }

        [Required]
        public string ComputerName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("ComputerId")]
        public virtual List<ComputerComponent> ComputerComponents { get; set; }

        [ForeignKey("ComputerId")]
        public virtual List<Order> Orders { get; set; }
    }
}
