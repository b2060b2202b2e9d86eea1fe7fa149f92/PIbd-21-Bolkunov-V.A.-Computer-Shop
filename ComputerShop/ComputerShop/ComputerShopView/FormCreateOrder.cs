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
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopBusinessLogic.BindingModels;

namespace ComputerShopView
{
    public partial class FormCreateOrder : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ComputerLogic logicComputer;

        private readonly OrderLogic logicOrder;

        public FormCreateOrder(ComputerLogic logicComputer, OrderLogic logicOrder)
        {
            InitializeComponent();
            this.logicComputer = logicComputer;
            this.logicOrder = logicOrder;
        }

        private void FormCreateOrder_Load(object sender, EventArgs e)
        {
            try
            {
                List<ComputerViewModel> list = logicComputer.Read(null);
                if(list != null)
                {
                    ComputersComboBox.DisplayMember = "ComputerName";
                    ComputersComboBox.ValueMember = "Id";
                    ComputersComboBox.DataSource = list;
                    ComputersComboBox.SelectedItem = null;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalcPrice()
        {
            if(ComputersComboBox.SelectedValue != null && !string.IsNullOrEmpty(CountTextBox.Text))
            {
                try
                {
                    int id = Convert.ToInt32(ComputersComboBox.SelectedValue);
                    ComputerViewModel computer = logicComputer.Read(new ComputerBindingModel { Id = id })?[0];
                    int count = Convert.ToInt32(CountTextBox.Text);
                    PriceTextBox.Text = (count * (computer?.Price ?? 0)).ToString();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CountTextBox_TextChanged(object sender, EventArgs e)
        {
            CalcPrice();
        }

        private void ComputersComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcPrice();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(CountTextBox.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(ComputersComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберете изделие", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                logicOrder.CreateOrder(new CreateOrderBindingModel
                {
                    ComputerId = Convert.ToInt32(ComputersComboBox.SelectedValue),
                    Count = Convert.ToInt32(CountTextBox.Text),
                    Sum = Convert.ToDecimal(PriceTextBox.Text)
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
