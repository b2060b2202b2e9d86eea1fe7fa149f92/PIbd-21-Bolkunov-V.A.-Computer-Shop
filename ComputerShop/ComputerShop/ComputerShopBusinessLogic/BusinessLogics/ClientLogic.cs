using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.Enums;

namespace ComputerShopBusinessLogic.BusinessLogics
{
    public class ClientLogic
    {
        private readonly IClientStorage clientStorage;

        public ClientLogic(IClientStorage clientStorage)
        {
            this.clientStorage = clientStorage;
        }

        public List<ClientViewModel> Read(ClientBindingModel model)
        {
            if (model == null)
            {
                return clientStorage.GetFullList();
            }

            if (model.Id.HasValue || (model.ClientLogin != null && model.PasswordHash != null))
            {
                //проверка пароля
                var client = clientStorage.GetElement(model);
                if (client.PasswordHash == model.PasswordHash)
                {
                    return new List<ClientViewModel> { client };
                }
                else
                {
                    return null;
                }
            }

            return clientStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(ClientBindingModel model)
        {
            var client = clientStorage.GetElement(new ClientBindingModel { ClientLogin = model.ClientLogin });

            if(client != null && client.Id != model.Id)
            {
                throw new Exception("Клиент с таким логином уже существует");
            }

            if(model.Id.HasValue)
            {
                clientStorage.Update(model);
            }
            else
            {
                clientStorage.Insert(model);
            }
        }

        public void Delete(ClientBindingModel model)
        {
            var client = clientStorage.GetElement(model);

            if(client == null)
            {
                throw new Exception("Клиент не найден");
            }

            clientStorage.Delete(model);
        }
    }
}
