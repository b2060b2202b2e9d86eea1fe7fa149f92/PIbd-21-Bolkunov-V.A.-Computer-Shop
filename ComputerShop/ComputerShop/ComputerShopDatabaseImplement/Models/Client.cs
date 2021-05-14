using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ComputerShopBusinessLogic.Enums;

namespace ComputerShopDatabaseImplement.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string ClientName { get; set; }

        [Required]
        public string ClientLogin { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [ForeignKey("ClientId")]
        public virtual List<Order> Orders { get; set; }

        [ForeignKey("ClientId")]
        public virtual List<MessageInfo> MessageInfos { get; set; }
    }
}
