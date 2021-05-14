using System;
using System.Windows.Forms;
using Unity;
using System.Collections.Generic;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopBusinessLogic.ViewModels;

namespace ComputerShopView
{
    public partial class FormMain : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly OrderLogic orderLogic;

        private readonly ReportLogic reportLogic;

        private readonly WorkModeling workModeling;

        public FormMain(OrderLogic orderLogic, ReportLogic reportLogic, WorkModeling workModeling)
        {
            InitializeComponent();
            this.orderLogic = orderLogic;
            this.reportLogic = reportLogic;
            this.workModeling = workModeling;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<OrderViewModel> list = orderLogic.Read(null);
                if(list != null)
                {
                    ordersDataGridView.DataSource = list;

                    for (int i = 0; i <= 12; i++)
                    {
                        if (i <= 3 || i == 12)
                        {
                            ordersDataGridView.Columns[i].Visible = false;
                        }
                        else
                        {
                            ordersDataGridView.Columns[i].Visible = true;
                            ordersDataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                        ordersDataGridView.Columns[i].ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComponentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormComponents>();
            form.ShowDialog();
        }

        private void ComputersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormComputers>();
            form.ShowDialog();
        }

        private void CreateOrderButton_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCreateOrder>();
            form.ShowDialog();
            LoadData();
        }

        private void TakeOrderInWorkButton_Click(object sender, EventArgs e)
        {
            if(ordersDataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(ordersDataGridView.SelectedRows[0].Cells[0].Value);
                try
                {
                    orderLogic.TakeOrderInWork(new ChangeStatusBindingModel { OrderId = id });
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FinishOrderButton_Click(object sender, EventArgs e)
        {
            if (ordersDataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(ordersDataGridView.SelectedRows[0].Cells[0].Value);
                try
                {
                    orderLogic.FinishOrder(new ChangeStatusBindingModel { OrderId = id });
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PayOrderButton_Click(object sender, EventArgs e)
        {
            if (ordersDataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(ordersDataGridView.SelectedRows[0].Cells[0].Value);
                try
                {
                    orderLogic.PayOrder(new ChangeStatusBindingModel { OrderId = id });
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ComputerComponentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReportComputerComponents>();
            form.ShowDialog();
        }

        private void ComponentsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "docx|*.docx"})
            {
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    reportLogic.SaveComponentsToWordFile(new ReportBindingModel { FileName = dialog.FileName });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void OrdersListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReportOrders>();
            form.ShowDialog();
        }

        private void ClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormClients>();
            form.ShowDialog();
        }

        private void ImplementerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormImplementers>();
            form.ShowDialog();
        }

        private void DoWorkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            workModeling.DoWork();
        }
    }
}
