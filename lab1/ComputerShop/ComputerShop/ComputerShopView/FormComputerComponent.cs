using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopBusinessLogic.ViewModels;
using Unity;

namespace ComputerShopView
{
    public partial class FormComputerComponent : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id 
        { 
            get { return Convert.ToInt32(componentsComboBox.SelectedValue); }
            set { componentsComboBox.SelectedValue = value; }
        }

        public string ComponentName { get { return componentsComboBox.Text; } }

        public int Count 
        { 
            get { return Convert.ToInt32(countTextBox.Text); }
            set { countTextBox.Text = value.ToString(); }
        }

        public FormComputerComponent(ComponentLogic logic)
        {
            InitializeComponent();

            List<ComponentViewModel> list = logic.Read(null);
            if(list != null)
            {
                componentsComboBox.DisplayMember = "ComponentName";
                componentsComboBox.ValueMember = "Id";
                componentsComboBox.DataSource = list;
                componentsComboBox.SelectedItem = null;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(countTextBox.Text))
            {
                MessageBox.Show("Заполните поле количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(componentsComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
