using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopFileImplement.Models;

namespace ComputerShopFileImplement.Implementations
{
    public class ClientStorage : IClientStorage
    {
        private readonly FileDataListSingleton dataSource;

        public ClientStorage()
        {
            dataSource = FileDataListSingleton.GetInstance();
        }

        public List<ClientViewModel> GetFullList()
        {
            return dataSource.Clients.Select(CreateModel).ToList();
        }

        public List<ClientViewModel> GetFilteredList(ClientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            return dataSource.Clients
                .Where(c => c.ClientName.Contains(model.ClientName)
                            || c.ClientLogin.Contains(model.ClientLogin))
                .Select(CreateModel)
                .ToList();
        }

        public ClientViewModel GetElement(ClientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var client = dataSource.Clients
                .FirstOrDefault(c => c.Id == model.Id || c.ClientLogin == model.ClientLogin);

            return client == null ? null : CreateModel(client);
        }

        public void Insert(ClientBindingModel model)
        {
            if (dataSource.Clients.Exists(c => c.ClientLogin == model.ClientLogin))
            {
                throw new Exception("Клиент с таким логином уже существует");
            }
            int maxId = dataSource.Clients.Count > 0 ? dataSource.Clients.Max(c => c.Id) : 0;
            dataSource.Clients.Add(CreateModel(model, new Client { Id = maxId + 1 }));
        }

        public void Update(ClientBindingModel model)
        {
            if (dataSource.Clients.Exists(c => c.Id != model.Id && c.ClientLogin == model.ClientLogin))
            {
                throw new Exception("Клиент с таким логином уже существует");
            }

            var client = dataSource.Clients.FirstOrDefault(c => c.Id == model.Id);

            if (client == null)
            {
                throw new Exception("Клиент не найден");
            }

            CreateModel(model, client);
        }

        public void Delete(ClientBindingModel model)
        {
            var client = dataSource.Clients
                .FirstOrDefault(c => c.Id == model.Id || (
                c.ClientLogin == model.ClientLogin &&
                c.PasswordHash == model.PasswordHash));
            if (client == null)
            {
                throw new Exception("Клиент не найден");
            }
            dataSource.Clients.Remove(client);
        }

        private ClientViewModel CreateModel(Client client)
        {
            return new ClientViewModel
            {
                Id = client.Id,
                ClientName = client.ClientName,
                ClientLogin = client.ClientLogin,
                PasswordHash = client.PasswordHash
            };
        }

        private Client CreateModel(ClientBindingModel model, Client client)
        {
            client.ClientLogin = model.ClientLogin;
            client.ClientName = model.ClientName;
            client.PasswordHash = model.PasswordHash;
            return client;
        }
    }
}
