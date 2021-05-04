using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopBusinessLogic.ViewModels;

namespace ComputerShopView
{
    public partial class FormComputer : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private int? id;

        public int Id { set { id = value; } }

        private readonly ComputerLogic logic;

        private Dictionary<int, (string, int)> computerComponents;

        public FormComputer(ComputerLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormComputer_Load(object sender, EventArgs e)
        {
            if(id.HasValue)
            {
                try
                {
                    ComputerViewModel vm = logic.Read(new ComputerBindingModel { Id = id.Value })?[0];
                    
                    if(vm != null)
                    {
                        NameTextBox.Text = vm.ComputerName;
                        PriceTextBox.Text = vm.Price.ToString();
                        computerComponents = vm.ComputerComponents;
                        LoadData();
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                computerComponents = new Dictionary<int, (string, int)>();
            }
        }

        private void LoadData()
        {
            try
            {
                if(computerComponents != null)
                {
                    ComponentsDataGridView.Rows.Clear();
                    foreach (var comp in computerComponents)
                    {
                        ComponentsDataGridView.Rows.Add(new object[] { comp.Key, comp.Value.Item1, comp.Value.Item2 });
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormComputerComponent>();
            if(form.ShowDialog() == DialogResult.OK)
            {
                if(computerComponents.ContainsKey(form.Id))
                {
                    computerComponents[form.Id] = (form.ComponentName, form.Count);
                }
                else
                {
                    computerComponents.Add(form.Id,(form.ComponentName, form.Count));
                }
                LoadData();
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if(ComponentsDataGridView.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormComputerComponent>();
                int id = Convert.ToInt32(ComponentsDataGridView.SelectedRows[0].Cells[0].Value);
                form.Id = id;
                form.Count = computerComponents[id].Item2;
                if(form.ShowDialog() == DialogResult.OK)
                {
                    computerComponents[form.Id] = (form.ComponentName, form.Count);
                    LoadData();
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if(ComponentsDataGridView.SelectedRows.Count == 1)
            {
                if(MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        computerComponents.Remove(Convert.ToInt32(ComponentsDataGridView.SelectedRows[0].Cells[0].Value));
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(NameTextBox.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(PriceTextBox.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(computerComponents == null || computerComponents.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new ComputerBindingModel
                {
                    Id = id,
                    ComputerName = NameTextBox.Text,
                    Price = Convert.ToDecimal(PriceTextBox.Text),
                    ComputerComponents = computerComponents
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
