using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopDatabaseImplement.Models;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ComputerShopDatabaseImplement.Implementations
{
    public class ClientStorage : IClientStorage
    {
        public List<ClientViewModel> GetFullList()
        {
            using (var context = new ComputerShopDatabase())
            {
                return context.Clients
                    .Select(client => new ClientViewModel
                    {
                        Id = client.Id,
                        ClientName = client.ClientName,
                        ClientLogin = client.ClientLogin,
                        PasswordHash = client.PasswordHash
                    }).ToList();
            }
        }

        public List<ClientViewModel> GetFilteredList(ClientBindingModel model)
        {
            if(model == null)
            {
                return null;
            }

            using (var context = new ComputerShopDatabase())
            {
                return context.Clients
                    .Where(client => client.ClientName.Contains(model.ClientName) ||
                        client.ClientLogin.Contains(model.ClientLogin))
                    .Select(client => new ClientViewModel
                    {
                        Id = client.Id,
                        ClientName = client.ClientName,
                        ClientLogin = client.ClientLogin,
                        PasswordHash = client.PasswordHash
                    }).ToList();
            }
        }

        public ClientViewModel GetElement(ClientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ComputerShopDatabase())
            {
                var client = context.Clients
                    .FirstOrDefault(c => c.Id == model.Id || c.ClientLogin == model.ClientLogin);
                return client == null ? null : new ClientViewModel
                {
                    Id = client.Id,
                    ClientName = client.ClientName,
                    ClientLogin = client.ClientLogin,
                    PasswordHash = client.PasswordHash
                };
            }
        }

        public void Insert(ClientBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                if (context.Clients.Any(cl => cl.ClientLogin == model.ClientLogin))
                {
                    throw new Exception("Клиент с таким логином уже существует");
                }

                context.Clients.Add(CreateModel(model, new Client()));
                context.SaveChanges();
            }
        }

        public void Update(ClientBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                if (context.Clients.Any(cl => cl.Id != model.Id && cl.ClientLogin == model.ClientLogin))
                {
                    throw new Exception("Клиент с таким логином уже существует");
                }

                var client = context.Clients
                    .FirstOrDefault(c => c.Id == model.Id);
                
                if(client == null)
                {
                    throw new Exception("Клиент не найден");
                }

                CreateModel(model, client);
                context.SaveChanges();
            }
        }

        public void Delete(ClientBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                var client = context.Clients
                    .FirstOrDefault(c => c.Id == model.Id || (
                    c.ClientLogin == model.ClientLogin && 
                    c.PasswordHash == model.PasswordHash));

                if (client == null)
                {
                    throw new Exception("Клиент не найден");
                }

                context.Clients.Remove(client);
                context.SaveChanges();
            }
        }

        private Client CreateModel(ClientBindingModel model, Client client)
        {
            client.ClientName = model.ClientName;
            client.ClientLogin = model.ClientLogin;
            client.PasswordHash = model.PasswordHash;
            return client;
        }
    }
}
