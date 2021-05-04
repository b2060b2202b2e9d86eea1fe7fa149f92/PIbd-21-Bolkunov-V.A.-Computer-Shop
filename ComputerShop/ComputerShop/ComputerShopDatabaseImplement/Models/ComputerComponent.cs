using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ComputerShopDatabaseImplement.Models
{
    public class ComputerComponent
    {
        public int Id { get; set; }

        public int ComputerId { get; set; }

        public int ComponentId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Component Component { get; set; }

        public virtual Computer Computer { get; set; }
    }
}
