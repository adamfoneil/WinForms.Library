namespace WinForms.SampleApp
{
	partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnFileType = new System.Windows.Forms.ToolStripButton();
            this.cbBelt = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cbItem = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revealInFileExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bldPath = new WinForms.Library.Controls.BuilderTextBox();
            this.builderTextBox1 = new WinForms.Library.Controls.BuilderTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbKeyedItem = new System.Windows.Forms.ComboBox();
            this.tbToolStrip = new System.Windows.Forms.ToolStripTextBox();
            this.cbToolStrip = new System.Windows.Forms.ToolStripComboBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbFirstName
            // 
            this.tbFirstName.Location = new System.Drawing.Point(85, 46);
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new System.Drawing.Size(230, 20);
            this.tbFirstName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "First Name:";
            // 
            // tbLastName
            // 
            this.tbLastName.Location = new System.Drawing.Point(85, 72);
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(230, 20);
            this.tbLastName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Last Name:";
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Location = new System.Drawing.Point(85, 98);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(80, 17);
            this.chkIsActive.TabIndex = 4;
            this.chkIsActive.Text = "checkBox1";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(85, 160);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Date:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.toolStripButton1,
            this.btnSave,
            this.btnFileType,
            this.tbToolStrip,
            this.cbToolStrip});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(847, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(35, 22);
            this.btnNew.Text = "New";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(49, 22);
            this.toolStripButton1.Text = "Open...";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(35, 22);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnFileType
            // 
            this.btnFileType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileType.Image = ((System.Drawing.Image)(resources.GetObject("btnFileType.Image")));
            this.btnFileType.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileType.Name = "btnFileType";
            this.btnFileType.Size = new System.Drawing.Size(23, 22);
            this.btnFileType.Text = "Get File Type";
            this.btnFileType.Click += new System.EventHandler(this.BtnFileType_Click);
            // 
            // cbBelt
            // 
            this.cbBelt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBelt.FormattingEnabled = true;
            this.cbBelt.Location = new System.Drawing.Point(85, 121);
            this.cbBelt.Name = "cbBelt";
            this.cbBelt.Size = new System.Drawing.Size(230, 21);
            this.cbBelt.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Belt:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(85, 218);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(43, 220);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Level:";
            // 
            // cbItem
            // 
            this.cbItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbItem.FormattingEnabled = true;
            this.cbItem.Location = new System.Drawing.Point(84, 244);
            this.cbItem.Name = "cbItem";
            this.cbItem.Size = new System.Drawing.Size(231, 21);
            this.cbItem.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(46, 247);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Item:";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Location = new System.Drawing.Point(396, 60);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(439, 297);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 15;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.SmallIcon;
            this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.revealInFileExplorerToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(188, 48);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // revealInFileExplorerToolStripMenuItem
            // 
            this.revealInFileExplorerToolStripMenuItem.Name = "revealInFileExplorerToolStripMenuItem";
            this.revealInFileExplorerToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.revealInFileExplorerToolStripMenuItem.Text = "Reveal in File Explorer";
            this.revealInFileExplorerToolStripMenuItem.Click += new System.EventHandler(this.revealInFileExplorerToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // bldPath
            // 
            this.bldPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bldPath.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bldPath.Location = new System.Drawing.Point(396, 28);
            this.bldPath.Name = "bldPath";
            this.bldPath.Size = new System.Drawing.Size(439, 26);
            this.bldPath.Suggestions = null;
            this.bldPath.TabIndex = 16;
            this.bldPath.Value = "";
            this.bldPath.BuilderClicked += new WinForms.Library.Controls.BuilderEventHandler(this.bldPath_BuilderClicked);
            // 
            // builderTextBox1
            // 
            this.builderTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.builderTextBox1.Location = new System.Drawing.Point(85, 186);
            this.builderTextBox1.Name = "builderTextBox1";
            this.builderTextBox1.Size = new System.Drawing.Size(230, 26);
            this.builderTextBox1.Suggestions = null;
            this.builderTextBox1.TabIndex = 8;
            this.builderTextBox1.Value = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 274);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Keyed Item:";
            // 
            // cbKeyedItem
            // 
            this.cbKeyedItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKeyedItem.FormattingEnabled = true;
            this.cbKeyedItem.Location = new System.Drawing.Point(85, 271);
            this.cbKeyedItem.Name = "cbKeyedItem";
            this.cbKeyedItem.Size = new System.Drawing.Size(230, 21);
            this.cbKeyedItem.TabIndex = 18;
            // 
            // tbToolStrip
            // 
            this.tbToolStrip.Name = "tbToolStrip";
            this.tbToolStrip.Size = new System.Drawing.Size(100, 25);
            // 
            // cbToolStrip
            // 
            this.cbToolStrip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbToolStrip.Name = "cbToolStrip";
            this.cbToolStrip.Size = new System.Drawing.Size(121, 25);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 370);
            this.Controls.Add(this.cbKeyedItem);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.bldPath);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbItem);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbBelt);
            this.Controls.Add(this.builderTextBox1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.chkIsActive);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbLastName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFirstName);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbFirstName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbLastName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkIsActive;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btnNew;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripButton btnSave;
		private WinForms.Library.Controls.BuilderTextBox builderTextBox1;
		private System.Windows.Forms.ComboBox cbBelt;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cbItem;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ListView listView1;
		private Library.Controls.BuilderTextBox bldPath;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem revealInFileExplorerToolStripMenuItem;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cbKeyedItem;
        private System.Windows.Forms.ToolStripButton btnFileType;
        private System.Windows.Forms.ToolStripTextBox tbToolStrip;
        private System.Windows.Forms.ToolStripComboBox cbToolStrip;
    }
}

