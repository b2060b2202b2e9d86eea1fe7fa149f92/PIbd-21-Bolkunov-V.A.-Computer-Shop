
namespace ComputerShopView
{
    partial class FormReportOrders
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fromDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.toDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.CreateReportButton = new System.Windows.Forms.Button();
            this.SaveToPdfButton = new System.Windows.Forms.Button();
            this.ordersReportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // fromDateTimePicker
            // 
            this.fromDateTimePicker.Location = new System.Drawing.Point(28, 12);
            this.fromDateTimePicker.Name = "fromDateTimePicker";
            this.fromDateTimePicker.Size = new System.Drawing.Size(140, 20);
            this.fromDateTimePicker.TabIndex = 0;
            // 
            // toDateTimePicker
            // 
            this.toDateTimePicker.Location = new System.Drawing.Point(193, 12);
            this.toDateTimePicker.Name = "toDateTimePicker";
            this.toDateTimePicker.Size = new System.Drawing.Size(140, 20);
            this.toDateTimePicker.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "С                                                   до";
            // 
            // CreateReportButton
            // 
            this.CreateReportButton.Location = new System.Drawing.Point(540, 12);
            this.CreateReportButton.Name = "CreateReportButton";
            this.CreateReportButton.Size = new System.Drawing.Size(121, 20);
            this.CreateReportButton.TabIndex = 3;
            this.CreateReportButton.Text = "Создать отчет";
            this.CreateReportButton.UseVisualStyleBackColor = true;
            this.CreateReportButton.Click += new System.EventHandler(this.CreateReportButton_Click);
            // 
            // SaveToPdfButton
            // 
            this.SaveToPdfButton.Location = new System.Drawing.Point(667, 12);
            this.SaveToPdfButton.Name = "SaveToPdfButton";
            this.SaveToPdfButton.Size = new System.Drawing.Size(121, 20);
            this.SaveToPdfButton.TabIndex = 4;
            this.SaveToPdfButton.Text = "Сохранить в PDF";
            this.SaveToPdfButton.UseVisualStyleBackColor = true;
            this.SaveToPdfButton.Click += new System.EventHandler(this.SaveToPdfButton_Click);
            // 
            // ordersReportViewer
            // 
            this.ordersReportViewer.LocalReport.ReportEmbeddedResource = "ComputerShopView.OrdersReport.rdlc";
            this.ordersReportViewer.Location = new System.Drawing.Point(12, 38);
            this.ordersReportViewer.Name = "ordersReportViewer";
            this.ordersReportViewer.ServerReport.BearerToken = null;
            this.ordersReportViewer.Size = new System.Drawing.Size(776, 511);
            this.ordersReportViewer.TabIndex = 5;
            // 
            // FormOrders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 561);
            this.Controls.Add(this.ordersReportViewer);
            this.Controls.Add(this.SaveToPdfButton);
            this.Controls.Add(this.CreateReportButton);
            this.Controls.Add(this.toDateTimePicker);
            this.Controls.Add(this.fromDateTimePicker);
            this.Controls.Add(this.label1);
            this.Name = "FormOrders";
            this.Text = "Отчеты по заказам";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker fromDateTimePicker;
        private System.Windows.Forms.DateTimePicker toDateTimePicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CreateReportButton;
        private System.Windows.Forms.Button SaveToPdfButton;
        private Microsoft.Reporting.WinForms.ReportViewer ordersReportViewer;
    }
}