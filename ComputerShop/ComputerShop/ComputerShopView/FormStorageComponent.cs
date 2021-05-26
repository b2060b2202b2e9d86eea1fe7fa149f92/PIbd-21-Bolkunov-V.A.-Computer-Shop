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
using ComputerShopBusinessLogic.ViewModels;
using ComputerShopBusinessLogic.BusinessLogics;
using Unity;

namespace ComputerShopView
{
    public partial class FormStorageComponent : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly StorageLogic storageLogic;
        private readonly ComponentLogic componentLogic;

        public FormStorageComponent(StorageLogic storageLogic, ComponentLogic componentLogic)
        {
            InitializeComponent();
            this.componentLogic = componentLogic;
            this.storageLogic = storageLogic;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(storageComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберете склад", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (componentComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберете компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(CountTextBox.Text) && Int32.TryParse(CountTextBox.Text, out var count) && count >= 0)
            {
                MessageBox.Show("Заполните кол-во", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                storageLogic.AddCountOrAddComponent(new StorageAddComponentBindingModel
                {
                    StorageID = (int)storageComboBox.SelectedValue,
                    ComponentID = (int)componentComboBox.SelectedValue,
                    ComponentCount = Int32.Parse(CountTextBox.Text)
                });
                MessageBox.Show("Пополнение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FormStorageComponent_Load(object sender, EventArgs e)
        {
            var componentList = componentLogic.Read(null);
            if (componentList != null)
            {
                componentComboBox.DisplayMember = "ComponentName";
                componentComboBox.ValueMember = "Id";
                componentComboBox.DataSource = componentList;
                componentComboBox.SelectedItem = null;
            }

            var storageList = storageLogic.Read(null);
            if (storageList != null)
            {
                storageComboBox.DisplayMember = "StorageName";
                storageComboBox.ValueMember = "Id";
                storageComboBox.DataSource = storageList;
                storageComboBox.SelectedItem = null;
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void storageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            if (storageComboBox.SelectedItem != null)
            {
                List<KeyValuePair<string, int>> list = storageLogic
                    .Read(new StorageBindingModel() { Id = (int)storageComboBox.SelectedValue })[0]
                    .ComponentCounts.Values
                    .Select(item => new KeyValuePair<string, int>(item.Item1, item.Item2))
                    .ToList();

                if (list != null)
                {
                    componentsDataGridView.DataSource = list;
                    componentsDataGridView.Columns[0].HeaderText = "Название";
                    componentsDataGridView.Columns[1].HeaderText = "Кол-во";
                }
            }
        }
    }
}
