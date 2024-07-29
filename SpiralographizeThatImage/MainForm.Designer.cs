
namespace SpiralographizeThatImage
{
	partial class MainForm
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
            pictureBox1 = new System.Windows.Forms.PictureBox();
            menuStrip = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            constantSpiralMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            goldenRatioMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            byThicknessMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            byRadiusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            pictureBox1.Location = new System.Drawing.Point(0, 29);
            pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(750, 750);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // menuStrip
            // 
            menuStrip.BackColor = System.Drawing.SystemColors.Control;
            menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, optionsToolStripMenuItem });
            menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new System.Drawing.Size(750, 28);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { newImageToolStripMenuItem, saveToolStripMenuItem, toolStripSeparator2, closeToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            fileToolStripMenuItem.Text = "&File";
            // 
            // newImageToolStripMenuItem
            // 
            newImageToolStripMenuItem.Name = "newImageToolStripMenuItem";
            newImageToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            newImageToolStripMenuItem.Text = "&New";
            newImageToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            saveToolStripMenuItem.Text = "&Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(125, 6);
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            closeToolStripMenuItem.Text = "&Close";
            closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { constantSpiralMenuItem, goldenRatioMenuItem, byThicknessMenuItem, byRadiusMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            optionsToolStripMenuItem.Text = "&Options";
            // 
            // constantSpiralMenuItem
            // 
            constantSpiralMenuItem.CheckOnClick = true;
            constantSpiralMenuItem.Name = "constantSpiralMenuItem";
            constantSpiralMenuItem.Size = new System.Drawing.Size(332, 26);
            constantSpiralMenuItem.Text = "Constant Spiral";
            constantSpiralMenuItem.CheckedChanged += constantSpiralMenuItem_CheckedChanged;
            // 
            // goldenRatioMenuItem
            // 
            goldenRatioMenuItem.CheckOnClick = true;
            goldenRatioMenuItem.Name = "goldenRatioMenuItem";
            goldenRatioMenuItem.Size = new System.Drawing.Size(332, 26);
            goldenRatioMenuItem.Text = "Golden Ratio Spiral";
            goldenRatioMenuItem.CheckedChanged += goldenRatioMenuItem_CheckedChanged;
            // 
            // byThicknessMenuItem
            // 
            byThicknessMenuItem.Checked = true;
            byThicknessMenuItem.CheckOnClick = true;
            byThicknessMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            byThicknessMenuItem.Name = "byThicknessMenuItem";
            byThicknessMenuItem.Size = new System.Drawing.Size(332, 26);
            byThicknessMenuItem.Text = "Image by Line Thickness Modulation";
            byThicknessMenuItem.CheckedChanged += byThicknessMenuItem_CheckedChanged;
            // 
            // byRadiusMenuItem
            // 
            byRadiusMenuItem.CheckOnClick = true;
            byRadiusMenuItem.Name = "byRadiusMenuItem";
            byRadiusMenuItem.Size = new System.Drawing.Size(332, 26);
            byRadiusMenuItem.Text = "Image by Line Radius Modulating";
            byRadiusMenuItem.CheckedChanged += byRadiusMenuItem_CheckedChanged;
            // 
            // MainForm
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(750, 779);
            Controls.Add(pictureBox1);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "MainForm";
            Text = "Spiralographize";
            Shown += MainForm_Shown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byThicknessMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byRadiusMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem constantSpiralMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goldenRatioMenuItem;
    }
}