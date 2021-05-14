using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopListImplement.Models;

namespace ComputerShopListImplement.Implementations
{
    public class ClientStorage : IClientStorage
    {
        private readonly DataListSingleton dataSource;

        public ClientStorage()
        {
            dataSource = DataListSingleton.GetInstance();
        }

        public List<ClientViewModel> GetFullList()
        {
            var res = new List<ClientViewModel>();
            foreach (var c in dataSource.Clients)
            {
                res.Add(CreateModel(c));
            }
            return res;
        }

        public List<ClientViewModel> GetFilteredList(ClientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var result = new List<ClientViewModel>();
            foreach (var c in dataSource.Clients)
            {
                if (c.ClientName.Contains(model.ClientName) || c.ClientLogin.Contains(model.ClientLogin))
                {
                    result.Add(CreateModel(c));
                }
            }
            return result;
        }

        public ClientViewModel GetElement(ClientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            foreach (var c in dataSource.Clients)
            {
                if (c.Id == model.Id || c.ClientLogin == model.ClientLogin)
                {
                    return CreateModel(c);
                }
            }
            return null;
        }

        public void Insert(ClientBindingModel model)
        {
            var temp = new Client { Id = 1 };
            foreach (var client in dataSource.Clients)
            {
                if (client.ClientLogin == model.ClientLogin)
                {
                    throw new Exception("Клиент с таким логином уже существует");
                }

                if (client.Id >= temp.Id)
                {
                    temp.Id = client.Id + 1;
                }
            }
            dataSource.Clients.Add(CreateModel(model, temp));
        }

        public void Update(ClientBindingModel model)
        {
            Client temp = null;
            foreach (var client in dataSource.Clients)
            {
                if (model.Id == client.Id)
                {
                    temp = client;
                }
                else if (model.ClientLogin == model.ClientLogin)
                {
                    throw new Exception("Клиент с таким логином уже существует");
                }
            }

            if (temp == null)
            {
                throw new Exception("Клиент не найден");
            }

            CreateModel(model, temp);
        }

        public void Delete(ClientBindingModel model)
        {
            for (int i = 0; i < dataSource.Clients.Count; i++)
            {
                if (dataSource.Clients[i].Id == model.Id || (
                    dataSource.Clients[i].ClientLogin == model.ClientLogin &&
                    dataSource.Clients[i].PasswordHash == model.PasswordHash))
                {
                    dataSource.Clients.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Клиент не найден");
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
