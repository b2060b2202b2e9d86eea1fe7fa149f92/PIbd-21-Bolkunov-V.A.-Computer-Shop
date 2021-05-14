using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerShopDatabaseImplement
{
    public class ComputerShopDatabase:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer
                (
                    @"Data Source=(local)\SQLEXPRESS;
                    Initial Catalog=ComputerShopDatabase;
                    Integrated Security=True;
                    MultipleActiveResultSets=True;"
                );
            }
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Component> Components { set; get; }
        public virtual DbSet<Computer> Computers { set; get; }
        public virtual DbSet<ComputerComponent> ComputerComponents { set; get; }
        public virtual DbSet<Order> Orders { set; get; }
        public virtual DbSet<Client> Clients { set; get; }
        public virtual DbSet<Implementer> Implementers { set; get; }
        public virtual DbSet<MessageInfo> MessageInfos { set; get; }
    }
}
