
namespace ComputerShopView
{
    partial class FormReportComputerComponents
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
            this.componentsDataGridView = new System.Windows.Forms.DataGridView();
            this.SaveButton = new System.Windows.Forms.Button();
            this.componentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.computerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.countColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.componentsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // componentsDataGridView
            // 
            this.componentsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.componentsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.componentColumn,
            this.computerColumn,
            this.countColumn});
            this.componentsDataGridView.Location = new System.Drawing.Point(12, 41);
            this.componentsDataGridView.Name = "componentsDataGridView";
            this.componentsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.componentsDataGridView.Size = new System.Drawing.Size(483, 397);
            this.componentsDataGridView.TabIndex = 0;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(12, 12);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(147, 23);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Сохранить в Excel";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // componentColumn
            // 
            this.componentColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.componentColumn.HeaderText = "Компонент";
            this.componentColumn.Name = "componentColumn";
            this.componentColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // computerColumn
            // 
            this.computerColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.computerColumn.HeaderText = "Компьютер";
            this.computerColumn.Name = "computerColumn";
            this.computerColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // countColumn
            // 
            this.countColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.countColumn.HeaderText = "Количество";
            this.countColumn.Name = "countColumn";
            this.countColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FormReportComputerComponents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 450);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.componentsDataGridView);
            this.Name = "FormReportComputerComponents";
            this.Text = "Компьютерные компоненты";
            this.Load += new System.EventHandler(this.FormReportComputerComponents_Load);
            ((System.ComponentModel.ISupportInitialize)(this.componentsDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView componentsDataGridView;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn componentColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn computerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn countColumn;
    }
}