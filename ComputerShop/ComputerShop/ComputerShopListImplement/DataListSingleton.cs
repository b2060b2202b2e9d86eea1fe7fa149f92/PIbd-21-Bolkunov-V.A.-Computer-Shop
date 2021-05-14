using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopListImplement.Models;

namespace ComputerShopListImplement
{
    public class DataListSingleton
    {
        private static DataListSingleton instance;

        public List<Component> Components { get; set; }
        public List<Order> Orders { get; set; }
        public List<Computer> Computers { get; set; }
        public List<Client> Clients { get; set; }

        private DataListSingleton()
        {
            Components = new List<Component>();
            Orders = new List<Order>();
            Computers = new List<Computer>();
            Clients = new List<Client>();
        }

        public static DataListSingleton GetInstance()
        {
            if(instance == null)
            {
                instance = new DataListSingleton();
            }
            return instance;
        }
    }
}
