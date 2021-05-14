using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MailKit.Net.Pop3;
using MailKit.Security;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.HelperModels;

namespace ComputerShopBusinessLogic.BusinessLogics
{
    public class MailLogic
    {
        private static string smtpClientHost;
        private static int smtpClientPort;
        private static string mailLogin;
        private static string mailPassword;

        private readonly IMessageInfoStorage messageInfoStorage;
        private readonly IClientStorage clientStorage;

        public MailLogic(IMessageInfoStorage messageInfoStorage, IClientStorage clientStorage)
        {
            this.messageInfoStorage = messageInfoStorage;
            this.clientStorage = clientStorage;
        }

        public List<MessageInfoViewModel> Read(MessageInfoBindingModel model)
        {
            if (model == null)
            {
                return messageInfoStorage.GetFullList();
            }

            return messageInfoStorage.GetFilteredList(model);
        }

        public void CreateOrder(MessageInfoBindingModel model)
        {
            var client = clientStorage.GetElement(new ClientBindingModel { ClientLogin = model.FromMailAddress });
            model.ClientId = client?.Id;
            messageInfoStorage.Insert(model);
        }

        public static void MailConfig(MailConfig config)
        {
            smtpClientHost = config.SmtpClientHost;
            smtpClientPort = config.SmtpClientPort;
            mailLogin = config.MailLogin;
            mailPassword = config.MailPassword;
        }

        public static async void MailSendAsync(MailSendInfo info)
        {
            if(string.IsNullOrEmpty(smtpClientHost) || smtpClientPort == 0 || 
                string.IsNullOrEmpty(mailLogin) || string.IsNullOrEmpty(mailPassword) ||
                string.IsNullOrEmpty(info.MailAddress) || string.IsNullOrEmpty(info.Subject) || 
                string.IsNullOrEmpty(info.Text))
            {
                return;
            }

            using (var objMailMessage = new MailMessage())
            {
                using (var objSmtpClient = new SmtpClient(smtpClientHost, smtpClientPort))
                {
                    try
                    {
                        objMailMessage.From = new MailAddress(mailLogin);
                        objMailMessage.To.Add(new MailAddress(info.MailAddress));
                        objMailMessage.Subject = info.Subject;
                        objMailMessage.Body = info.Text;
                        objMailMessage.SubjectEncoding = Encoding.UTF8;
                        objMailMessage.BodyEncoding = Encoding.UTF8;

                        objSmtpClient.UseDefaultCredentials = false;
                        objSmtpClient.EnableSsl = true;
                        objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        objSmtpClient.Credentials = new NetworkCredential(mailLogin, mailPassword);

                        await Task.Run(() => objSmtpClient.Send(objMailMessage));
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public static async void MailCheck(MailCheckInfo info)
        {
            if (string.IsNullOrEmpty(info.PopHost) || info.PopPort == 0 ||
                string.IsNullOrEmpty(mailLogin) || string.IsNullOrEmpty(mailPassword) ||
                info.Storage == null)
            {
                return;
            }

            using (var client = new Pop3Client())
            {
                await Task.Run(() => 
                { 
                    try
                    {
                        client.Connect(info.PopHost, info.PopPort, SecureSocketOptions.SslOnConnect);
                        
                        client.Authenticate(mailLogin, mailPassword);

                        for (int i = 0; i < client.Count; i++)
                        {
                            var message = client.GetMessage(i);
                            foreach (var mail in message.From.Mailboxes)
                            {
                                try
                                {
                                    var c = info.ClientStorage.GetElement(new ClientBindingModel { ClientLogin = mail.Address });
                                    int? clientId = null;
                                    if (c != null)
                                    {
                                        clientId = c.Id;
                                    }

                                    info.Storage.Insert(new MessageInfoBindingModel
                                    {
                                        ClientId = clientId,
                                        DateDelivery = message.Date.DateTime,
                                        MessageId = message.MessageId,
                                        FromMailAddress = mail.Address,
                                        Subject = message.Subject,
                                        Body = message.TextBody
                                    });
                                }
                                catch (Exception) {  }
                                /*Оказываемся тут, если уже сохраняли это письмо. 
                                 *У меня почта настроена так, что бы письма не удалялись при загрузке*/
                            }
                        }
                    }
                    finally
                    {
                        client.Disconnect(true);
                    }
                });
            }
        }
    }
}
