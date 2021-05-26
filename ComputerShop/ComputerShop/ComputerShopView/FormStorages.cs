using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.BusinessLogics;
using Unity;

namespace ComputerShopView
{
    public partial class FormStorages : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly StorageLogic logic;

        public FormStorages(StorageLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void LoadData()
        {
            try
            {
                var list = logic.Read(null);
                if (list != null)
                {
                    storagesDataGridView.DataSource = list;
                    storagesDataGridView.Columns[0].Visible = false;
                    storagesDataGridView.Columns[0].ReadOnly = true;
                    storagesDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    storagesDataGridView.Columns[1].ReadOnly = true;
                    storagesDataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    storagesDataGridView.Columns[2].ReadOnly = true;
                    storagesDataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    storagesDataGridView.Columns[3].ReadOnly = true;
                    storagesDataGridView.Columns[4].Visible = false;
                    storagesDataGridView.Columns[4].ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormStorage>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (storagesDataGridView.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormStorageComponent>();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (storagesDataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(storagesDataGridView.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        logic.Delete(new StorageBindingModel { Id = id });
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

        private void StoragesForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
