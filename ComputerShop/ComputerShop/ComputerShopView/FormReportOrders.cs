using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using Unity;
using ComputerShopBusinessLogic.BindingModels;
using ComputerShopBusinessLogic.BusinessLogics;

namespace ComputerShopView
{
    public partial class FormReportOrders : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ReportLogic reportLogic;

        public FormReportOrders(ReportLogic reportLogic)
        {
            InitializeComponent();
            this.reportLogic = reportLogic;
        }

        private void CreateReportButton_Click(object sender, EventArgs e)
        {
            if(fromDateTimePicker.Value.Date >= toDateTimePicker.Value.Date)
            {
                MessageBox.Show("Дата начала отчета должна быть меньше даты окончания", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var parameter = new ReportParameter
                    (
                        "ReportParameterPeriod",
                        "С " + fromDateTimePicker.Value.ToShortDateString() +
                        " по " + toDateTimePicker.Value.ToShortDateString()
                    );
                ordersReportViewer.LocalReport.SetParameters(parameter);

                var dataSource = reportLogic.GetOrders(new ReportBindingModel
                {
                    DateFrom = fromDateTimePicker.Value,
                    DateTo = toDateTimePicker.Value,
                });
                var reportDateSource = new ReportDataSource("OrdersDataSet", dataSource);
                ordersReportViewer.LocalReport.DataSources.Clear();
                ordersReportViewer.LocalReport.DataSources.Add(reportDateSource);
                ordersReportViewer.RefreshReport();
                ordersReportViewer.LocalReport.Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveToPdfButton_Click(object sender, EventArgs e)
        {
            if (fromDateTimePicker.Value.Date >= toDateTimePicker.Value.Date)
            {
                MessageBox.Show("Дата начала отчета должна быть меньше даты окончания",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var dialog = new SaveFileDialog { Filter = "pdf|*.pdf"})
            {
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        reportLogic.SaveOrderToPdfFile(new ReportBindingModel
                        {
                            FileName = dialog.FileName,
                            DateFrom = fromDateTimePicker.Value,
                            DateTo = toDateTimePicker.Value
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
