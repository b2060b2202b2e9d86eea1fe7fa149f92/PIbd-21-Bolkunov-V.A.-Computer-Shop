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
        private readonly MailLogic mailLogic;

        public FormClients(ClientLogic logicClient, MailLogic mailLogic)
        {
            InitializeComponent();
            this.logicClient = logicClient;
            this.mailLogic = mailLogic;
        }

        private void LoadData()
        {
            try
            {
                Program.ConfigureGrid(logicClient.Read(null), clientsDataGridView);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMail(MessageInfoBindingModel model)
        {
            try
            {
                Program.ConfigureGrid(mailLogic.Read(model), clientMailDataGridView);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormClients_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadMail(null);
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
            LoadMail(null);
        }

        private void clientsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (clientsDataGridView.SelectedRows.Count > 0)
            {
                LoadMail(new MessageInfoBindingModel 
                    { ClientId = Convert.ToInt32(clientsDataGridView.SelectedRows[0].Cells[0].Value) });
            }
        }
    }
}
