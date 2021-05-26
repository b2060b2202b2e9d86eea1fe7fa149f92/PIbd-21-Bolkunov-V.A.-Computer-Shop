﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopFileImplement.Implementations;

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
            currentContainer.RegisterType<IStorageStorage, StorageStorage>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<ComponentLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<OrderLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ComputerLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<StorageLogic>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
