using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.Interfaces;
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopDatabaseImplement.Models;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ComputerShopDatabaseImplement.Implementations
{
    public class ComputerStorage : IComputerStorage
    {
        public List<ComputerViewModel> GetFullList()
        {
            using (var context = new ComputerShopDatabase())
            {
                return context.Computers
                    .Include(comp => comp.ComputerComponents)
                    .ThenInclude(cc => cc.Component)
                    .ToList()
                    .Select(comp => new ComputerViewModel
                    {
                        Id = comp.Id,
                        ComputerName = comp.ComputerName,
                        Price = comp.Price,
                        ComputerComponents = comp.ComputerComponents
                                .ToDictionary<ComputerComponent, int, (string, int)>
                                (
                                    cc => cc.ComponentId,
                                    cc => CreateTuple(cc.Component.ComponentName, cc.Count)
                                )
                    }).ToList();
            }
        }

        public List<ComputerViewModel> GetFilteredList(ComputerBindingModel model)
        {
            if(model == null)
            {
                return null;
            }

            using (var context = new ComputerShopDatabase())
            {
                return context.Computers
                    .Include(comp => comp.ComputerComponents)
                    .ThenInclude(cc => cc.Component)
                    .Where(comp => comp.ComputerName.Contains(model.ComputerName))
                    .ToList()
                    .Select(comp => new ComputerViewModel
                    {
                        Id = comp.Id,
                        ComputerName = comp.ComputerName,
                        Price = comp.Price,
                        ComputerComponents = comp.ComputerComponents
                            .ToDictionary<ComputerComponent, int, (string, int)>
                            (
                                cc => cc.ComponentId,
                                cc => CreateTuple(cc.Component.ComponentName, cc.Count)
                            )
                    }).ToList();
            }
        }
        
        public ComputerViewModel GetElement(ComputerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ComputerShopDatabase())
            {
                var computer = context.Computers
                    .Include(comp => comp.ComputerComponents)
                    .ThenInclude(cc => cc.Component)
                    .ToList()
                    .FirstOrDefault(comp => comp.ComputerName == model.ComputerName || comp.Id == model.Id);
                    
                if(computer == null)
                {
                    return null;
                }
                else
                {
                    return new ComputerViewModel
                    {
                        Id = computer.Id,
                        ComputerName = computer.ComputerName,
                        Price = computer.Price,
                        ComputerComponents = computer.ComputerComponents
                            .ToDictionary<ComputerComponent,int,(string,int)>
                            (
                                cc => cc.ComponentId,
                                cc => CreateTuple(cc.Component.ComponentName, cc.Count)
                            )
                    };
                }
            }
        }

        public void Insert(ComputerBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                if(context.Computers.Any(comp => comp.ComputerName == model.ComputerName))
                {
                    throw new Exception("Компьютер с таким названием уже существует");
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var computer = new Computer();
                        context.Computers.Add(computer);
                        CreateModel(model, computer, context);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(ComputerBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                if (context.Computers.Any(comp => comp.Id != model.Id && comp.ComputerName == model.ComputerName))
                {
                    throw new Exception("Компьютер с таким названием уже существует");
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var computer = context.Computers.FirstOrDefault(comp => comp.Id == model.Id);

                        if(computer == null)
                        {
                            throw new Exception("Компьютер не найден");
                        }

                        CreateModel(model, computer, context);

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(ComputerBindingModel model)
        {
            using (var context = new ComputerShopDatabase())
            {
                var computer = context.Computers
                    .FirstOrDefault(comp => comp.Id == model.Id || comp.ComputerName == model.ComputerName);

                if(computer == null)
                {
                    throw new Exception("Компьютер не найден");
                }
                else
                {
                    context.Computers.Remove(computer);
                    context.SaveChanges();
                }
            }
        }

        private (string, int) CreateTuple(string str, int num)
        {
            return (str, num);
        }

        private Computer CreateModel(ComputerBindingModel model, Computer computer, ComputerShopDatabase context)
        {
            computer.ComputerName = model.ComputerName;
            computer.Price = model.Price;

            if(model.Id.HasValue)
            {
                var computerComponents = context.ComputerComponents.Where(cc => cc.ComputerId == model.Id.Value).ToList();

                context.ComputerComponents.RemoveRange
                    (computerComponents.Where(cc => !model.ComputerComponents.ContainsKey(cc.ComponentId)).ToList());
                context.SaveChanges();

                foreach(var updateComponent in computerComponents)
                {
                    updateComponent.Count = model.ComputerComponents[updateComponent.ComponentId].Item2;
                    model.ComputerComponents.Remove(updateComponent.ComponentId);
                }

                context.SaveChanges();
            }

            foreach(var cc in model.ComputerComponents)
            {
                context.ComputerComponents.Add(new ComputerComponent
                {
                    ComputerId = computer.Id,
                    ComponentId = cc.Key,
                    Count = cc.Value.Item2,
                    Computer = computer,
                    Component = context.Components.FirstOrDefault(c => c.Id == cc.Key)
                });
                context.SaveChanges();
            }

            return computer;
        }
    }
}
