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
using ComputerShopBusinessLogic.BusinessLogics;
using ComputerShopBusinessLogic.BindingModels;

namespace ComputerShopView
{
    public partial class FormReportComputerComponents : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ReportLogic reportLogic;

        public FormReportComputerComponents(ReportLogic reportLogic)
        {
            InitializeComponent();
            this.reportLogic = reportLogic;
        }

        private void FormReportComputerComponents_Load(object sender, EventArgs e)
        {
            try
            {
                componentsDataGridView.Rows.Clear();
                var computersList = reportLogic.GetComputers();
                if (computersList != null)
                {
                    foreach (var comp in computersList)
                    {
                        componentsDataGridView.Rows.Add(new object[] { "", comp.ComputerName, "" });
                        foreach (var cc in comp.ComputerComponents)
                        {
                            componentsDataGridView.Rows.Add(new object[] { cc.Item1, "", cc.Item2 });
                        }
                        componentsDataGridView.Rows.Add(new object[] { "Итого", "", comp.TotalCount });
                        if (computersList.Last() != comp)
                        {
                            componentsDataGridView.Rows.Add(new object[] { });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        reportLogic.SaveComputerComponentToExcelFile(new ReportBindingModel
                        {
                            FileName = dialog.FileName
                        });
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
