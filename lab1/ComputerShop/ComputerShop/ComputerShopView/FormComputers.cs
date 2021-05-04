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
    public partial class FormComputers : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ComputerLogic computerLogic;

        public FormComputers(ComputerLogic computerLogic)
        {
            InitializeComponent();
            this.computerLogic = computerLogic;
        }

        private void FormComputers_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var list = computerLogic.Read(null);
                if (list != null)
                {
                    computersDataGridView.DataSource = list;

                    computersDataGridView.Columns[0].Visible = false;
                    computersDataGridView.Columns[0].ReadOnly = true;

                    computersDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    computersDataGridView.Columns[1].ReadOnly = true;

                    computersDataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    computersDataGridView.Columns[2].ReadOnly = true;

                    computersDataGridView.Columns[3].Visible = false;
                    computersDataGridView.Columns[3].ReadOnly = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormComputer>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (computersDataGridView.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormComputer>();
                form.Id = Convert.ToInt32(computersDataGridView.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (computersDataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить компьютер", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(computersDataGridView.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        computerLogic.Delete(new ComputerBindingModel { Id = id });
                    }
                    catch (Exception ex)
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
    }
}
