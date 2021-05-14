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

        private readonly ClientLogic logicClient;

        public FormCreateOrder(ComputerLogic logicComputer, OrderLogic logicOrder, ClientLogic logicClient)
        {
            InitializeComponent();
            this.logicComputer = logicComputer;
            this.logicOrder = logicOrder;
            this.logicClient = logicClient;
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

                var clientsList = logicClient.Read(null);
                if(clientsList != null)
                {
                    ClientsComboBox.DisplayMember = "ClientName";
                    ClientsComboBox.ValueMember = "Id";
                    ClientsComboBox.DataSource = clientsList;
                    ClientsComboBox.SelectedItem = null;
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
                MessageBox.Show("Выберете компьютер", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ClientsComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберете клиента", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                logicOrder.CreateOrder(new CreateOrderBindingModel
                {
                    ComputerId = Convert.ToInt32(ComputersComboBox.SelectedValue),
                    ClientId = Convert.ToInt32(ClientsComboBox.SelectedValue),
                    Count = Convert.ToInt32(CountTextBox.Text),
                    Sum = Convert.ToDecimal(PriceTextBox.Text),
                    OrderByApp = true
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
