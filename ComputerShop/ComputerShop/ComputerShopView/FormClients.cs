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
    public partial class FormClients : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ClientLogic logicClient;

        public FormClients(ClientLogic logicClient)
        {
            InitializeComponent();
            this.logicClient = logicClient;
        }

        private void LoadData()
        {
            try
            {
                var list = logicClient.Read(null);
                if (list != null)
                {
                    clientsDataGridView.DataSource = list;

                    clientsDataGridView.Columns[0].Visible = false;
                    clientsDataGridView.Columns[0].ReadOnly = false;

                    clientsDataGridView.Columns[1].Visible = true;
                    clientsDataGridView.Columns[1].ReadOnly = true;

                    clientsDataGridView.Columns[2].Visible = true;
                    clientsDataGridView.Columns[2].ReadOnly = true;

                    clientsDataGridView.Columns[3].Visible = false;
                    clientsDataGridView.Columns[3].ReadOnly = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormClients_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (clientsDataGridView.SelectedRows.Count > 0 && 
                        MessageBox.Show("Удалить клиента", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    logicClient.Delete(new ClientBindingModel
                    {
                        Id = Convert.ToInt32(clientsDataGridView.SelectedRows[0].Cells[0].Value)
                    });
                    LoadData();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
