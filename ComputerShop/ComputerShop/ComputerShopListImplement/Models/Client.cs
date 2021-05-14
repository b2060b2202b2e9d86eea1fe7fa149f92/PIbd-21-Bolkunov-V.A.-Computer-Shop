using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerShopListImplement.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string ClientName { get; set; }

        public string ClientLogin { get; set; }

        public string PasswordHash { get; set; }
    }
}
