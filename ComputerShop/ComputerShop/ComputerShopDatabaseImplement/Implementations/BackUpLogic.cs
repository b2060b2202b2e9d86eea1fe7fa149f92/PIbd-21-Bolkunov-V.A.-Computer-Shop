using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ComputerShopBusinessLogic.BusinessLogics;

namespace ComputerShopDatabaseImplement.Implementations
{
    public class BackUpLogic : BackupAbstractLogic
    {
        protected override Assembly GetAssembly()
        {
            return typeof(BackUpLogic).Assembly;
        }

        protected override List<PropertyInfo> GetFullList()
        {
            using (var context = new ComputerShopDatabase())
            {
                Type type = context.GetType();
                return type.GetProperties().Where(x => 
                    x.PropertyType.FullName.StartsWith("Microsoft.EntityFrameworkCore.DbSet")).ToList();
            }
        }

        protected override List<T> GetList<T>()
        {
            using (var context = new ComputerShopDatabase())
            {
                return context.Set<T>().ToList();
            }
        }
    }
}
