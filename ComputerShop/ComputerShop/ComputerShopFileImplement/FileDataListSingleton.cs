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
        private readonly string ClientsFileName = "Clients.xml";
        private readonly string ImplementersFileName = "Implementers.xml";

        public List<Component> Components { get; set; }
        public List<Order> Orders { get; set; }
        public List<Computer> Computers { get; set; }
        public List<Client> Clients { get; set; }
        public List<Implementer> Implementers { get; set; }

        private FileDataListSingleton()
        {
            Orders = LoadOrders();
            Components = LoadComponents();
            Computers = LoadComputers();
            Clients = LoadClients();
            Implementers = LoadImplementers();
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
            SaveClients();
            SaveImplementers();
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
                    if (!String.IsNullOrEmpty(element.Element("DateImplement").Value))
                    {
                        dateImplement = DateTime.Parse(element.Element("DateImplement").Value);
                    }

                    bool? freeOrders = null;
                    if (!String.IsNullOrEmpty(element.Element("FreeOrders").Value))
                    {
                        freeOrders = Boolean.Parse(element.Element("FreeOrders").Value);
                    }

                    int? implementerId = null;
                    if (!String.IsNullOrEmpty(element.Element("ImplementerId").Value))
                    {
                        implementerId = int.Parse(element.Element("ImplementerId").Value);
                    }

                    list.Add(new Order
                    {
                        Id = Convert.ToInt32(element.Attribute("Id").Value),
                        ComputerId = Convert.ToInt32(element.Element("ComputerId").Value),
                        ClientId = Convert.ToInt32(element.Element("ClientId").Value),
                        ImplementerId = implementerId,
                        Count = Convert.ToInt32(element.Element("Count").Value),
                        Status = (OrderStatus)Convert.ToInt32(element.Element("Status").Value),
                        Sum = Convert.ToDecimal(element.Element("Sum").Value),
                        DateCreate = DateTime.Parse(element.Element("DateCreate").Value),
                        DateImplement = dateImplement,
                        FreeOrders = freeOrders
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

        private List<Client> LoadClients()
        {
            var list = new List<Client>();

            if(File.Exists(ClientsFileName))
            {
                var xDocument = XDocument.Load(ClientsFileName);
                var xElements = xDocument.Root.Elements("Clients").ToList();

                foreach (var elem in xElements)
                {
                    list.Add(new Client 
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        ClientName = elem.Element("ClientName").Value,
                        ClientLogin = elem.Element("ClientLogin").Value,
                        PasswordHash = elem.Element("PasswordHash").Value
                    });
                }
            }

            return list;
        }

        private List<Implementer> LoadImplementers()
        {
            var list = new List<Implementer>();

            if (File.Exists(ImplementersFileName))
            {
                var xDocument = XDocument.Load(ClientsFileName);
                var xElements = xDocument.Root.Elements("Implementers").ToList();

                foreach (var elem in xElements)
                {
                    list.Add(new Implementer
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        ImplementerName = elem.Element("ImplementerName").Value,
                        WorkingTime = Convert.ToInt32(elem.Element("WorkingTime").Value),
                        PauseTime = Convert.ToInt32(elem.Element("PauseTime").Value)
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
                            new XElement("ClientId", order.ClientId),
                            new XElement("ImplementerId", order.ImplementerId),
                            new XElement("Count", order.Count),
                            new XElement("Status", (int)order.Status),
                            new XElement("Sum", order.Sum),
                            new XElement("DateCreate", order.DateCreate.ToString()),
                            new XElement("DateImplement", order.DateImplement.ToString()),
                            new XElement("FreeOrders", order.FreeOrders.ToString())
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

        private void SaveClients()
        {
            if (Clients != null)
            {
                var xElement = new XElement("Clients");

                foreach(var client in Clients)
                {
                    xElement.Add(new XElement
                        (
                            "Client",
                            new XAttribute("Id", client.Id),
                            new XElement("ClientName", client.ClientName),
                            new XElement("ClientLogin", client.ClientLogin),
                            new XElement("PasswordHash", client.PasswordHash)
                        ));
                }

                var xDocument = new XDocument(xElement);
                xDocument.Save(ClientsFileName);
            }
        }

        private void SaveImplementers()
        {
            if(Implementers != null)
            {
                var xElement = new XElement("Implementers");

                foreach (var imp in Implementers)
                {
                    xElement.Add(new XElement
                    (
                        "Implementer",
                        new XAttribute("Id", imp.Id),
                        new XElement("ImplementerName", imp.ImplementerName),
                        new XElement("WorkingTime", imp.WorkingTime),
                        new XElement("PauseTime", imp.PauseTime)
                    ));
                }

                var xDocument = new XDocument(xElement);
                xDocument.Save(ImplementersFileName);
            }
        }
    }
}
