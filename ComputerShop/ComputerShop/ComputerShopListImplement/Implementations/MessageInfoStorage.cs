using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopListImplement.Models;
using ComputerShopBusinessLogic.BindingModels;

namespace ComputerShopListImplement.Implementations
{
    public class MessageInfoStorage : IMessageInfoStorage
    {
        private readonly DataListSingleton dataSource;

        public MessageInfoStorage()
        {
            dataSource = DataListSingleton.GetInstance();
        }

        public List<MessageInfoViewModel> GetFullList()
        {
            var result = new List<MessageInfoViewModel>();
            foreach (var mi in dataSource.MessageInfos)
            {
                result.Add(CreateModel(mi));
            }
            return result;
        }

        public List<MessageInfoViewModel> GetFilteredList(MessageInfoBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var result = new List<MessageInfoViewModel>();
            foreach (var mi in dataSource.MessageInfos)
            {
                if ((model.ClientId.HasValue && mi.ClientId == model.ClientId) ||
                    (!model.ClientId.HasValue && mi.DateDelivery.Date == model.DateDelivery.Date))
                {
                    result.Add(CreateModel(mi));
                }
            }
            return result;
        }

        public void Insert(MessageInfoBindingModel model)
        {
            foreach (var mi in dataSource.MessageInfos)
            {
                if (mi.MessageId == model.MessageId)
                {
                    throw new Exception("Уже есть письмо с таким идентификатором");
                }
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
