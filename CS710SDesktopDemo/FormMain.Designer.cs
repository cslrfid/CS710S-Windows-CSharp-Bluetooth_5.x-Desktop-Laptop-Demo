namespace CS710SDesktopDemo
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
            this.button1 = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonInventory = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.buttonStopInventory = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderDeviceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.dataGridView_EPC = new System.Windows.Forms.DataGridView();
            this.EPC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RSSI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button7 = new System.Windows.Forms.Button();
            this.checkBoxMacAddressFiltering = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_EPC)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 49);
            this.button1.TabIndex = 0;
            this.button1.Text = "Scan";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(12, 134);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(116, 49);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonInventory
            // 
            this.buttonInventory.Enabled = false;
            this.buttonInventory.Location = new System.Drawing.Point(12, 253);
            this.buttonInventory.Name = "buttonInventory";
            this.buttonInventory.Size = new System.Drawing.Size(116, 49);
            this.buttonInventory.TabIndex = 3;
            this.buttonInventory.Text = "Inventory";
            this.buttonInventory.UseVisualStyleBackColor = true;
            this.buttonInventory.Click += new System.EventHandler(this.buttonInventory_Click);
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.Location = new System.Drawing.Point(144, 134);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(718, 104);
            this.textBox3.TabIndex = 5;
            // 
            // buttonStopInventory
            // 
            this.buttonStopInventory.Enabled = false;
            this.buttonStopInventory.Location = new System.Drawing.Point(12, 319);
            this.buttonStopInventory.Name = "buttonStopInventory";
            this.buttonStopInventory.Size = new System.Drawing.Size(116, 49);
            this.buttonStopInventory.TabIndex = 6;
            this.buttonStopInventory.Text = "Stop Inventory";
            this.buttonStopInventory.UseVisualStyleBackColor = true;
            this.buttonStopInventory.Click += new System.EventHandler(this.button4_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDeviceName});
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(144, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(718, 78);
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // columnHeaderDeviceName
            // 
            this.columnHeaderDeviceName.Text = "Device Name";
            this.columnHeaderDeviceName.Width = 350;
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Enabled = false;
            this.buttonDisconnect.Location = new System.Drawing.Point(12, 189);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(116, 49);
            this.buttonDisconnect.TabIndex = 8;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(12, 387);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(116, 49);
            this.button6.TabIndex = 9;
            this.button6.Text = "Clear Tag records";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // dataGridView_EPC
            // 
            this.dataGridView_EPC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_EPC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EPC,
            this.RSSI});
            this.dataGridView_EPC.Location = new System.Drawing.Point(144, 253);
            this.dataGridView_EPC.Name = "dataGridView_EPC";
            this.dataGridView_EPC.RowHeadersVisible = false;
            this.dataGridView_EPC.Size = new System.Drawing.Size(718, 281);
            this.dataGridView_EPC.TabIndex = 10;
            // 
            // EPC
            // 
            this.EPC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EPC.HeaderText = "EPC (with duplicate filter)";
            this.EPC.Name = "EPC";
            // 
            // RSSI
            // 
            this.RSSI.HeaderText = "RSSI (dBuV)";
            this.RSSI.Name = "RSSI";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(12, 453);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(116, 49);
            this.button7.TabIndex = 11;
            this.button7.Text = "Exit";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // checkBoxMacAddressFiltering
            // 
            this.checkBoxMacAddressFiltering.AutoSize = true;
            this.checkBoxMacAddressFiltering.Checked = true;
            this.checkBoxMacAddressFiltering.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMacAddressFiltering.Location = new System.Drawing.Point(12, 18);
            this.checkBoxMacAddressFiltering.Name = "checkBoxMacAddressFiltering";
            this.checkBoxMacAddressFiltering.Size = new System.Drawing.Size(127, 17);
            this.checkBoxMacAddressFiltering.TabIndex = 12;
            this.checkBoxMacAddressFiltering.Text = "Mac Address Filtering";
            this.checkBoxMacAddressFiltering.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 546);
            this.Controls.Add(this.checkBoxMacAddressFiltering);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.dataGridView_EPC);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.buttonStopInventory);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.buttonInventory);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.button1);
            this.Name = "FormMain";
            this.Text = "CS710S Windows C# Desktop demo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_EPC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonInventory;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button buttonStopInventory;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeaderDeviceName;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.DataGridView dataGridView_EPC;
        private System.Windows.Forms.DataGridViewTextBoxColumn EPC;
        private System.Windows.Forms.DataGridViewTextBoxColumn RSSI;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.CheckBox checkBoxMacAddressFiltering;
    }
}

