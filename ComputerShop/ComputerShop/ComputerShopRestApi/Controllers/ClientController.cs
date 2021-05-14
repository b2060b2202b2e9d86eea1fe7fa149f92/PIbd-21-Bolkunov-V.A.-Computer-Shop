using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopBusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ComputerShopRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientLogic clientLogic;
        private readonly MailLogic mailLogic;
        private readonly int passwordMaxLength = 50;
        private readonly int passwordMinLength = 10;

        public ClientController(ClientLogic clientLogic, MailLogic mailLogic)
        {
            this.clientLogic = clientLogic;
            this.mailLogic = mailLogic;
        }

        [HttpGet]
        public ClientViewModel Login(string login, string password) =>
            clientLogic.Read(new ClientBindingModel { ClientLogin = login, PasswordHash = password })?[0];

        [HttpGet]
        public List<MessageInfoViewModel> GetMessages(int clientId) =>
            mailLogic.Read(new MessageInfoBindingModel { ClientId = clientId });

        [HttpPost]
        public void Register(ClientBindingModel model)
        {
            CheckData(model);
            clientLogic.CreateOrUpdate(model);
        }

        [HttpPost]
        public void UpdateData(ClientBindingModel model)
        {
            CheckData(model);
            clientLogic.CreateOrUpdate(model);
        }

        private void CheckData(ClientBindingModel model)
        {
            if(!Regex.IsMatch(model.ClientLogin, @".*@.*\..*"))
            {
                throw new Exception("В качестве логина должна быть указана почта");
            }
            if (model.PasswordHash.Length > passwordMaxLength || model.PasswordHash.Length < passwordMinLength
                || !Regex.IsMatch(model.PasswordHash, @"^((\w+\d+\W+)|(\w+\W+\d+)|(\d+\w+\W+)|(\d+\W+\w+)|(\W+\w+\d+)|(\W+\d+\w+))[\w\d\W]*$"))
            {
                throw new Exception($"Пароль должен быть длиной от {passwordMinLength} до " +
                    $"{passwordMaxLength} и состоять из цифр, букв и небуквенных символов");
            }
        }
    }
}
