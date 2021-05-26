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
    public partial class FormStorage : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly StorageLogic logic;

        public FormStorage(StorageLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(OwnerTextBox.Text))
            {
                MessageBox.Show("Заполните ФИО владельца", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new StorageBindingModel
                {
                    StorageName = NameTextBox.Text,
                    OwnerName = NameTextBox.Text,
                    CreationTime = DateTime.Now,
                    ComponentCounts = new Dictionary<int, (string, int)>()
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
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
    }
}
