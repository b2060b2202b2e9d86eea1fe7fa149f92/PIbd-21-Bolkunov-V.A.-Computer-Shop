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

namespace ComputerShopView
{
    public partial class FormImplementer : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private int? id;
        
        public int Id { set => id = value; }

        private readonly ImplementerLogic logic;

        public FormImplementer(ImplementerLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormImplementer_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new ImplementerBindingModel { Id = id })?[0];
                    if (view != null)
                    {
                        nameTextBox.Text = view.ImplementerName;
                        workTimeTextBox.Text = view.WorkingTime.ToString();
                        pauseTimeTextBox.Text = view.PauseTime.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(nameTextBox.Text) ||
                string.IsNullOrEmpty(workTimeTextBox.Text) || !int.TryParse(workTimeTextBox.Text, out _) ||
                string.IsNullOrEmpty(pauseTimeTextBox.Text) || !int.TryParse(pauseTimeTextBox.Text, out _))
            {
                MessageBox.Show("Заполните ФИО, время работы в милисекундах и время перерыва в милисекундах", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new ImplementerBindingModel 
                { 
                    Id = id, 
                    ImplementerName = nameTextBox.Text,
                    WorkingTime = Convert.ToInt32(workTimeTextBox.Text),
                    PauseTime = Convert.ToInt32(pauseTimeTextBox.Text)
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
