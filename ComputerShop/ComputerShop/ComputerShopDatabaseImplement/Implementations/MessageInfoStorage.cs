using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopDatabaseImplement.Models;
using ComputerShopBusinessLogic.BindingModels;

namespace ComputerShopDatabaseImplement.Implementations
{
    public class MessageInfoStorage : IMessageInfoStorage
    {
        public List<MessageInfoViewModel> GetFullList()
        {
            using (var context = new ComputerShopDatabase())
            {
                return context.MessageInfos
                    .Select(mi => new MessageInfoViewModel
                    {
                        MessageId = mi.MessageId,
                        SenderName = mi.SenderName,
                        DateDelivery = mi.DateDelivery,
                        Subject = mi.Subject,
                        Body = mi.Body
                    })
                    .ToList();
            }
        }

        public List<MessageInfoViewModel> GetFilteredList(MessageInfoBindingModel model)
        {
            if(model == null)
            {
                return null;
            }

            using (var context = new ComputerShopDatabase())
            {
                return context.MessageInfos
                    .Where(mi => (model.ClientId.HasValue && mi.ClientId == model.ClientId) || 
                        (!model.ClientId.HasValue && mi.DateDelivery.Date == model.DateDelivery.Date))
                    .Select(mi => new MessageInfoViewModel
                    {
                        MessageId = mi.MessageId,
                        SenderName = mi.SenderName,
                        DateDelivery = mi.DateDelivery,
                        Subject = mi.Subject,
                        Body = mi.Body
                    })
                    .ToList();
            }
        }

        public void Insert(MessageInfoBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                MessageInfo messageInfo = 
                    context.MessageInfos.FirstOrDefault(mi => mi.MessageId == model.MessageId);
                if(messageInfo != null)
                {
                    throw new Exception("Уже есть письмо с таким идентификатором");
                }//такая проверка нужна из-за настроек моей почты

                context.MessageInfos.Add(new MessageInfo 
                { 
                    MessageId = model.MessageId,
                    ClientId = model.ClientId,
                    SenderName = model.FromMailAddress,
                    DateDelivery = model.DateDelivery,
                    Subject = model.Subject,
                    Body = model.Body
                });
                context.SaveChanges();
            }
        }
    }
}
