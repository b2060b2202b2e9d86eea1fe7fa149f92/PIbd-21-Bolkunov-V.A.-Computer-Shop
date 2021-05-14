﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Unity;
using Unity.Lifetime;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.HelperModels;
using ComputerShopBusinessLogic.Attributes;
using ComputerShopDatabaseImplement.Implementations;


namespace ComputerShopView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            MailLogic.MailConfig(new MailConfig
            {
                SmtpClientHost = ConfigurationManager.AppSettings["SmtpClientHost"],
                SmtpClientPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpClientPort"]),
                MailLogin = ConfigurationManager.AppSettings["MailLogin"],
                MailPassword = ConfigurationManager.AppSettings["MailPassword"]
            });

            var timer = new System.Threading.Timer(new TimerCallback(MailCheck), new MailCheckInfo
            {
                PopHost = ConfigurationManager.AppSettings["PopHost"],
                PopPort = Convert.ToInt32(ConfigurationManager.AppSettings["PopPort"]),
                Storage = container.Resolve<IMessageInfoStorage>(),
                ClientStorage = container.Resolve<IClientStorage>()
            }, 0, 5000);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();

            currentContainer.RegisterType<IComponentStorage, ComponentStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IOrderStorage, OrderStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IComputerStorage, ComputerStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IClientStorage, ClientStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IImplementerStorage, ImplementersStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMessageInfoStorage, MessageInfoStorage>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<BackupAbstractLogic, BackUpLogic>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<ComponentLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<OrderLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ComputerLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ReportLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ClientLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ImplementerLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<WorkModeling>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<MailLogic>(new HierarchicalLifetimeManager());

            return currentContainer;
        }

        private static void MailCheck(object obj)
        {
            MailLogic.MailCheck((MailCheckInfo)obj);
        }

        public static void ConfigureGrid<T>(List<T> data, DataGridView dataGridView)
        {
            var type = typeof(T);
            var config = new List<string>();
            dataGridView.Columns.Clear();

            foreach (var property in type.GetProperties())
            {
                var attributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);
                if(attributes != null && attributes.Length > 0)
                {
                    foreach (var attribute in attributes)
                    {
                        if (attribute is ColumnAttribute columnAttribute)
                        {
                            config.Add(property.Name);
                            var column = new DataGridViewTextBoxColumn
                            {
                                Name = property.Name,
                                ReadOnly = true,
                                HeaderText = columnAttribute.Title,
                                Visible = columnAttribute.Visible,
                                Width = columnAttribute.Width
                            };
                            if(columnAttribute.GridViewAutoSize != GridViewAutoSize.None)
                            {
                                column.AutoSizeMode = (DataGridViewAutoSizeColumnMode)Enum.Parse
                                    (
                                        typeof(DataGridViewAutoSizeColumnMode),
                                        columnAttribute.GridViewAutoSize.ToString()
                                    );
                            }
                            dataGridView.Columns.Add(column);
                        }
                    }
                }
            }

            foreach (var element in data)
            {
                var objects = new List<object>();
                foreach (var conf in config)
                {
                    var value = element.GetType().GetProperty(conf).GetValue(element);
                    objects.Add(value);
                }
                dataGridView.Rows.Add(objects.ToArray());
            }
        }
    }
}
