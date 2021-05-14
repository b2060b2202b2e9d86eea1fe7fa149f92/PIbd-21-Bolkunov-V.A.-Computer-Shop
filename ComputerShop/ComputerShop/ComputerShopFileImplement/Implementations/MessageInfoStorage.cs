using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopFileImplement.Models;
using ComputerShopBusinessLogic.BindingModels;

namespace ComputerShopFileImplement.Implementations
{
    public class MessageInfoStorage : IMessageInfoStorage
    {
        private readonly FileDataListSingleton dataSource;

        public MessageInfoStorage()
        {
            dataSource = FileDataListSingleton.GetInstance();
        }
        public List<MessageInfoViewModel> GetFullList()
        {
            return dataSource.MessageInfos.Select(CreateModel).ToList();
        }

        public List<MessageInfoViewModel> GetFilteredList(MessageInfoBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            return dataSource.MessageInfos
                .Where(mi => (model.ClientId.HasValue && mi.ClientId == model.ClientId) ||
                        (!model.ClientId.HasValue && mi.DateDelivery.Date == model.DateDelivery.Date))
                .Select(CreateModel)
                .ToList();
        }

        public void Insert(MessageInfoBindingModel model)
        {
            if(dataSource.MessageInfos.Exists(mi => mi.MessageId == model.MessageId))
            {
                throw new Exception("Уже есть письмо с таким идентификатором");
            }

            dataSource.MessageInfos.Add(new MessageInfo
            {
                MessageId = model.MessageId,
                ClientId = model.ClientId,
                DateDelivery = model.DateDelivery,
                SenderName = model.FromMailAddress,
                Subject = model.Subject,
                Body = model.Body
            });
        }

        public MessageInfoViewModel CreateModel(MessageInfo messageInfo)
        {
            return new MessageInfoViewModel
            {
                MessageId = messageInfo.MessageId,
                DateDelivery = messageInfo.DateDelivery,
                SenderName = messageInfo.SenderName,
                Body = messageInfo.Body,
                Subject = messageInfo.Subject
            };
        }
    }
}
