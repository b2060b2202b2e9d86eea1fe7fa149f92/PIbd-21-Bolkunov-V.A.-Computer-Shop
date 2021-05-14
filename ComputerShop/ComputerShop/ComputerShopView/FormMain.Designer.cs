
namespace ComputerShopView
{
    partial class FormMain
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.справочникиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ComponentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ComputersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClientsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отчетToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ComponentsListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ComputerComponentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OrdersListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ordersDataGridView = new System.Windows.Forms.DataGridView();
            this.CreateOrderButton = new System.Windows.Forms.Button();
            this.TakeOrderInWorkButton = new System.Windows.Forms.Button();
            this.FinishOrderButton = new System.Windows.Forms.Button();
            this.PayOrderButton = new System.Windows.Forms.Button();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ordersDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.справочникиToolStripMenuItem,
            this.отчетToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(822, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // справочникиToolStripMenuItem
            // 
            this.справочникиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ComponentsToolStripMenuItem,
            this.ComputersToolStripMenuItem,
            this.ClientsToolStripMenuItem});
            this.справочникиToolStripMenuItem.Name = "справочникиToolStripMenuItem";
            this.справочникиToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.справочникиToolStripMenuItem.Text = "Справочники";
            // 
            // ComponentsToolStripMenuItem
            // 
            this.ComponentsToolStripMenuItem.Name = "ComponentsToolStripMenuItem";
            this.ComponentsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ComponentsToolStripMenuItem.Text = "Компоненты";
            this.ComponentsToolStripMenuItem.Click += new System.EventHandler(this.ComponentsToolStripMenuItem_Click);
            // 
            // ComputersToolStripMenuItem
            // 
            this.ComputersToolStripMenuItem.Name = "ComputersToolStripMenuItem";
            this.ComputersToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ComputersToolStripMenuItem.Text = "Компьютеры";
            this.ComputersToolStripMenuItem.Click += new System.EventHandler(this.ComputersToolStripMenuItem_Click);
            // 
            // ClientsToolStripMenuItem
            // 
            this.ClientsToolStripMenuItem.Name = "ClientsToolStripMenuItem";
            this.ClientsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ClientsToolStripMenuItem.Text = "Клиенты";
            this.ClientsToolStripMenuItem.Click += new System.EventHandler(this.ClientsToolStripMenuItem_Click);
            // 
            // отчетToolStripMenuItem
            // 
            this.отчетToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ComponentsListToolStripMenuItem,
            this.ComputerComponentsToolStripMenuItem,
            this.OrdersListToolStripMenuItem});
            this.отчетToolStripMenuItem.Name = "отчетToolStripMenuItem";
            this.отчетToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.отчетToolStripMenuItem.Text = "Отчет";
            // 
            // ComponentsListToolStripMenuItem
            // 
            this.ComponentsListToolStripMenuItem.Name = "ComponentsListToolStripMenuItem";
            this.ComponentsListToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.ComponentsListToolStripMenuItem.Text = "Список компонентов";
            this.ComponentsListToolStripMenuItem.Click += new System.EventHandler(this.ComponentsListToolStripMenuItem_Click);
            // 
            // ComputerComponentsToolStripMenuItem
            // 
            this.ComputerComponentsToolStripMenuItem.Name = "ComputerComponentsToolStripMenuItem";
            this.ComputerComponentsToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.ComputerComponentsToolStripMenuItem.Text = "Компьютерные компоненты";
            this.ComputerComponentsToolStripMenuItem.Click += new System.EventHandler(this.ComputerComponentsToolStripMenuItem_Click);
            // 
            // OrdersListToolStripMenuItem
            // 
            this.OrdersListToolStripMenuItem.Name = "OrdersListToolStripMenuItem";
            this.OrdersListToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.OrdersListToolStripMenuItem.Text = "Список заказов";
            this.OrdersListToolStripMenuItem.Click += new System.EventHandler(this.OrdersListToolStripMenuItem_Click);
            // 
            // ordersDataGridView
            // 
            this.ordersDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ordersDataGridView.Location = new System.Drawing.Point(12, 27);
            this.ordersDataGridView.Name = "ordersDataGridView";
            this.ordersDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ordersDataGridView.Size = new System.Drawing.Size(653, 411);
            this.ordersDataGridView.TabIndex = 1;
            // 
            // CreateOrderButton
            // 
            this.CreateOrderButton.Location = new System.Drawing.Point(671, 28);
            this.CreateOrderButton.Name = "CreateOrderButton";
            this.CreateOrderButton.Size = new System.Drawing.Size(143, 23);
            this.CreateOrderButton.TabIndex = 2;
            this.CreateOrderButton.Text = "Создать заказ";
            this.CreateOrderButton.UseVisualStyleBackColor = true;
            this.CreateOrderButton.Click += new System.EventHandler(this.CreateOrderButton_Click);
            // 
            // TakeOrderInWorkButton
            // 
            this.TakeOrderInWorkButton.Location = new System.Drawing.Point(671, 57);
            this.TakeOrderInWorkButton.Name = "TakeOrderInWorkButton";
            this.TakeOrderInWorkButton.Size = new System.Drawing.Size(143, 23);
            this.TakeOrderInWorkButton.TabIndex = 3;
            this.TakeOrderInWorkButton.Text = "Отдать на выполнение";
            this.TakeOrderInWorkButton.UseVisualStyleBackColor = true;
            this.TakeOrderInWorkButton.Click += new System.EventHandler(this.TakeOrderInWorkButton_Click);
            // 
            // FinishOrderButton
            // 
            this.FinishOrderButton.Location = new System.Drawing.Point(671, 86);
            this.FinishOrderButton.Name = "FinishOrderButton";
            this.FinishOrderButton.Size = new System.Drawing.Size(143, 23);
            this.FinishOrderButton.TabIndex = 4;
            this.FinishOrderButton.Text = "Зкаказ готов";
            this.FinishOrderButton.UseVisualStyleBackColor = true;
            this.FinishOrderButton.Click += new System.EventHandler(this.FinishOrderButton_Click);
            // 
            // PayOrderButton
            // 
            this.PayOrderButton.Location = new System.Drawing.Point(671, 115);
            this.PayOrderButton.Name = "PayOrderButton";
            this.PayOrderButton.Size = new System.Drawing.Size(143, 23);
            this.PayOrderButton.TabIndex = 5;
            this.PayOrderButton.Text = "Заказ оплачен";
            this.PayOrderButton.UseVisualStyleBackColor = true;
            this.PayOrderButton.Click += new System.EventHandler(this.PayOrderButton_Click);
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(671, 416);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(143, 23);
            this.RefreshButton.TabIndex = 6;
            this.RefreshButton.Text = "Обновить список";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 449);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.PayOrderButton);
            this.Controls.Add(this.FinishOrderButton);
            this.Controls.Add(this.TakeOrderInWorkButton);
            this.Controls.Add(this.CreateOrderButton);
            this.Controls.Add(this.ordersDataGridView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "Компьютерный магазин";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ordersDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem справочникиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ComponentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ComputersToolStripMenuItem;
        private System.Windows.Forms.DataGridView ordersDataGridView;
        private System.Windows.Forms.Button CreateOrderButton;
        private System.Windows.Forms.Button TakeOrderInWorkButton;
        private System.Windows.Forms.Button FinishOrderButton;
        private System.Windows.Forms.Button PayOrderButton;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.ToolStripMenuItem отчетToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ComponentsListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ComputerComponentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OrdersListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClientsToolStripMenuItem;
    }
}