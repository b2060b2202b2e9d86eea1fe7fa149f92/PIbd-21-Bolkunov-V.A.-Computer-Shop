using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopFileImplement.Models;
using ComputerShopBusinessLogic.Enums;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ComputerShopFileImplement
{
    public class FileDataListSingleton
    {
        private static FileDataListSingleton instance;

        private readonly string ComponentsFileName = "Components.xml";
        private readonly string OrdersFileName = "Orders.xml";
        private readonly string ComputersFileName = "Computers.xml";
        private readonly string StoragesFileName = "Storages.xml";

        public List<Component> Components { get; set; }
        public List<Order> Orders { get; set; }
        public List<Computer> Computers { get; set; }
        public List<Storage> Storages { get; set; }

        private FileDataListSingleton()
        {
            Orders = LoadOrders();
            Components = LoadComponents();
            Computers = LoadComputers();
            Storages = LoadStorages();
        }

        public static FileDataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new FileDataListSingleton();
            }
            return instance;
        }

        ~FileDataListSingleton()
        {
            SaveComponents();
            SaveOrders();
            SaveComputers();
            SaveStorages();
        }

        private List<Component> LoadComponents()
        {
            var list = new List<Component>();

            if(File.Exists(ComponentsFileName))
            {
                var xDocument = XDocument.Load(ComponentsFileName);
                var xElements = xDocument.Root.Elements("Component").ToList();

                foreach (var element in xElements)
                {
                    list.Add(new Component 
                    {
                        Id = Convert.ToInt32(element.Attribute("Id").Value),
                        ComponentName = element.Element("ComponentName").Value
                    });
                }
            }

            return list;
        }

        private List<Order> LoadOrders()
        {
            var list = new List<Order>();

            if(File.Exists(OrdersFileName))
            {
                var xDocument = XDocument.Load(OrdersFileName);
                var xElements = xDocument.Root.Elements("Order").ToList();

                foreach (var element in xElements)
                {
                    DateTime? dateImplement = null;
                    if(!String.IsNullOrEmpty(element.Element("DateImplement").Value))
                    {
                        dateImplement = DateTime.Parse(element.Element("DateImplement").Value);
                    }
                    list.Add(new Order
                    {
                        Id = Convert.ToInt32(element.Attribute("Id").Value),
                        ComputerId = Convert.ToInt32(element.Element("ComputerId").Value),
                        Count = Convert.ToInt32(element.Element("Count").Value),
                        Status = (OrderStatus)Convert.ToInt32(element.Element("Status").Value),
                        Sum = Convert.ToDecimal(element.Element("Sum").Value),
                        DateCreate = DateTime.Parse(element.Element("DateCreate").Value),
                        DateImplement = dateImplement
                    });
                }
            }

            return list;
        }

        private List<Computer> LoadComputers()
        {
            var list = new List<Computer>();

            if(File.Exists(ComputersFileName))
            {
                var xDocument = XDocument.Load(ComputersFileName);
                var xElements = xDocument.Root.Elements("Computer").ToList();

                foreach (var element in xElements)
                {
                    var components = new Dictionary<int, int>();
                    var componentsElements = element.Element("ComputerComponents").Elements("ComputerComponent").ToList();

                    foreach (var component in componentsElements)
                    {
                        components.Add(Convert.ToInt32(component.Element("Key").Value), Convert.ToInt32(component.Element("Value").Value));
                    }

                    list.Add(new Computer 
                    {
                        Id = Convert.ToInt32(element.Attribute("Id").Value),
                        ComputerName = element.Element("ComputerName").Value,
                        Price = Convert.ToDecimal(element.Element("Price").Value),
                        ComputerComponents = components
                    });
                }
            }

            return list;
        }

        private List<Storage> LoadStorages()
        {
            var list = new List<Storage>();

            if(File.Exists(StoragesFileName))
            {
                var xDocumnet = XDocument.Load(StoragesFileName);
                var xElements = xDocumnet.Root.Elements("Storage").ToList();

                foreach (var element in xElements)
                {
                    var components = new Dictionary<int, int>();
                    var componentsElements = element.Element("ComponentCounts").Elements("ComponentCount").ToList();

                    foreach (var component in componentsElements)
                    {
                        components.Add(Convert.ToInt32(component.Element("Key").Value), Convert.ToInt32(component.Element("Value").Value));
                    }

                    list.Add(new Storage()
                    {
                        Id = Convert.ToInt32(element.Attribute("Id").Value),
                        StorageName = element.Element("StorageName").Value,
                        OwnerName = element.Element("OwnerName").Value,
                        CreationTime = DateTime.Parse(element.Element("CreationTime").Value),
                        ComponentCounts = components
                    });
                }
            }

            return list;
        }

        private void SaveComponents()
        {
            if(Components != null)
            {
                var xElement = new XElement("Components");

                foreach (var component in Components)
                {
                    xElement.Add(new XElement
                        (
                            "Component",
                            new XAttribute("Id", component.Id),
                            new XElement("ComponentName", component.ComponentName)
                        ));
                }

                var xDocument = new XDocument(xElement);
                xDocument.Save(ComponentsFileName);
            }
        }

        private void SaveOrders()
        {
            if(Orders != null)
            {
                var xElement = new XElement("Orders");

                foreach (var order in Orders)
                {
                    xElement.Add(new XElement
                        (
                            "Order",
                            new XAttribute("Id", order.Id),
                            new XElement("ComputerId", order.ComputerId),
                            new XElement("Count", order.Count),
                            new XElement("Status", (int)order.Status),
                            new XElement("Sum", order.Sum),
                            new XElement("DateCreate", order.DateCreate.ToString()),
                            new XElement("DateImplement", order.DateImplement.ToString())
                        ));
                }

                var xDocument = new XDocument(xElement);
                xDocument.Save(OrdersFileName);
            }
        }

        private void SaveComputers()
        {
            if(Computers != null)
            {
                var xElement = new XElement("Computers");

                foreach (var computer in Computers)
                {
                    var componentElement = new XElement("ComputerComponents");
                    foreach (var component in computer.ComputerComponents)
                    {
                        componentElement.Add(new XElement 
                            (
                                "ComputerComponent",
                                new XElement("Key", component.Key),
                                new XElement("Value", component.Value)
                            ));
                    }

                    xElement.Add(new XElement
                        (
                            "Computer",
                            new XAttribute("Id", computer.Id),
                            new XElement("ComputerName", computer.ComputerName),
                            new XElement("Price", computer.Price),
                            componentElement
                        ));
                }

                var xDocument = new XDocument(xElement);
                xDocument.Save(ComputersFileName);
            }
        }

        private void SaveStorages()
        {
            if(Storages != null)
            {
                var xElement = new XElement("Storages");

                foreach (var storage in Storages)
                {
                    var componentElement = new XElement("ComponentCounts");
                    foreach (var component in storage.ComponentCounts)
                    {
                        componentElement.Add(new XElement
                            (
                                "ComponentCount",
                                new XElement("Key", component.Key),
                                new XElement("Value", component.Value)
                            ));
                    }

                    xElement.Add(new XElement
                        (
                            "Storage",
                            new XAttribute("Id", storage.Id),
                            new XElement("StorageName", storage.StorageName),
                            new XElement("OwnerName", storage.OwnerName),
                            new XElement("CreationTime", storage.CreationTime),
                            componentElement
                        ));
                }

                var xDocument = new XDocument(xElement);
                xDocument.Save(StoragesFileName);
            }    
        }
    }
}
