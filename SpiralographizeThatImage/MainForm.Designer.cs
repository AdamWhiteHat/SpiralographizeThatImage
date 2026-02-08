
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
            components = new System.ComponentModel.Container();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            saveImageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            loadImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            byThicknessMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            byRadiusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            constantSpiralsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            constantSpiralMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            goldenRatioMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            orderbySpiralArms = new System.Windows.Forms.ToolStripMenuItem();
            orderbyTopToBottom = new System.Windows.Forms.ToolStripMenuItem();
            statusStripInfo = new System.Windows.Forms.StatusStrip();
            toolStripStatusSize = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatuxZoom = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripSettings = new System.Windows.Forms.ToolStrip();
            toolStripLabelTickness = new System.Windows.Forms.ToolStripLabel();
            toolStripTextBoxThickness = new System.Windows.Forms.ToolStripTextBox();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabelTurns = new System.Windows.Forms.ToolStripLabel();
            toolStripTextBoxTurns = new System.Windows.Forms.ToolStripTextBox();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            contextMenuStrip1.SuspendLayout();
            menuStrip.SuspendLayout();
            statusStripInfo.SuspendLayout();
            toolStripSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pictureBox1.ContextMenuStrip = contextMenuStrip1;
            pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBox1.Location = new System.Drawing.Point(0, 25);
            pictureBox1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            pictureBox1.Size = new System.Drawing.Size(750, 749);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Resize += pictureBox1_Resize;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { saveImageToolStripMenuItem1 });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(153, 26);
            // 
            // saveImageToolStripMenuItem1
            // 
            saveImageToolStripMenuItem1.Name = "saveImageToolStripMenuItem1";
            saveImageToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            saveImageToolStripMenuItem1.Text = "&Save Image...";
            saveImageToolStripMenuItem1.Click += saveImageMenuItem_Click;
            // 
            // menuStrip
            // 
            menuStrip.BackColor = System.Drawing.SystemColors.Control;
            menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, optionsToolStripMenuItem, constantSpiralsToolStripMenuItem });
            menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new System.Drawing.Size(750, 25);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { saveImageToolStripMenuItem, toolStripSeparator2, closeToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            fileToolStripMenuItem.Text = "&File";
            // 
            // saveImageToolStripMenuItem
            // 
            saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
            saveImageToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            saveImageToolStripMenuItem.Text = "&Save Image...";
            saveImageToolStripMenuItem.Click += saveImageMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            closeToolStripMenuItem.Text = "&Close";
            closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { loadImageToolStripMenuItem, byThicknessMenuItem, byRadiusMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new System.Drawing.Size(99, 21);
            optionsToolStripMenuItem.Text = "&Image Spirals";
            // 
            // loadImageToolStripMenuItem
            // 
            loadImageToolStripMenuItem.Name = "loadImageToolStripMenuItem";
            loadImageToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            loadImageToolStripMenuItem.Text = "&Load Image...";
            loadImageToolStripMenuItem.Click += loadImageToolStripMenuItem_Click;
            // 
            // byThicknessMenuItem
            // 
            byThicknessMenuItem.Checked = true;
            byThicknessMenuItem.CheckOnClick = true;
            byThicknessMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            byThicknessMenuItem.Enabled = false;
            byThicknessMenuItem.Name = "byThicknessMenuItem";
            byThicknessMenuItem.Size = new System.Drawing.Size(287, 22);
            byThicknessMenuItem.Text = "Image by Line Thickness Modulation";
            byThicknessMenuItem.CheckedChanged += byThicknessMenuItem_CheckedChanged;
            // 
            // byRadiusMenuItem
            // 
            byRadiusMenuItem.CheckOnClick = true;
            byRadiusMenuItem.Enabled = false;
            byRadiusMenuItem.Name = "byRadiusMenuItem";
            byRadiusMenuItem.Size = new System.Drawing.Size(287, 22);
            byRadiusMenuItem.Text = "Image by Line Radius Modulating";
            byRadiusMenuItem.CheckedChanged += byRadiusMenuItem_CheckedChanged;
            // 
            // constantSpiralsToolStripMenuItem
            // 
            constantSpiralsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { constantSpiralMenuItem, goldenRatioMenuItem });
            constantSpiralsToolStripMenuItem.Name = "constantSpiralsToolStripMenuItem";
            constantSpiralsToolStripMenuItem.Size = new System.Drawing.Size(114, 21);
            constantSpiralsToolStripMenuItem.Text = "Constant Spirals";
            // 
            // constantSpiralMenuItem
            // 
            constantSpiralMenuItem.CheckOnClick = true;
            constantSpiralMenuItem.Name = "constantSpiralMenuItem";
            constantSpiralMenuItem.Size = new System.Drawing.Size(189, 22);
            constantSpiralMenuItem.Text = "Common Spiral";
            constantSpiralMenuItem.CheckedChanged += constantSpiralMenuItem_CheckedChanged;
            // 
            // goldenRatioMenuItem
            // 
            goldenRatioMenuItem.CheckOnClick = true;
            goldenRatioMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { orderbySpiralArms, orderbyTopToBottom });
            goldenRatioMenuItem.Name = "goldenRatioMenuItem";
            goldenRatioMenuItem.Size = new System.Drawing.Size(189, 22);
            goldenRatioMenuItem.Text = "Golden Ratio Spiral";
            goldenRatioMenuItem.CheckStateChanged += goldenRatioMenuItem_CheckStateChanged;
            // 
            // orderbySpiralArms
            // 
            orderbySpiralArms.CheckOnClick = true;
            orderbySpiralArms.Name = "orderbySpiralArms";
            orderbySpiralArms.Size = new System.Drawing.Size(191, 22);
            orderbySpiralArms.Text = "Spiral Arm Ordered";
            orderbySpiralArms.CheckStateChanged += orderbySpiralArms_CheckedChanged;
            // 
            // orderbyTopToBottom
            // 
            orderbyTopToBottom.CheckOnClick = true;
            orderbyTopToBottom.Name = "orderbyTopToBottom";
            orderbyTopToBottom.Size = new System.Drawing.Size(191, 22);
            orderbyTopToBottom.Text = "Top Down Ordered";
            orderbyTopToBottom.CheckStateChanged += orderbyTopToBottom_CheckedChanged;
            // 
            // statusStripInfo
            // 
            statusStripInfo.BackColor = System.Drawing.SystemColors.Control;
            statusStripInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            statusStripInfo.GripMargin = new System.Windows.Forms.Padding(0);
            statusStripInfo.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStripInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusSize, toolStripStatuxZoom });
            statusStripInfo.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            statusStripInfo.Location = new System.Drawing.Point(0, 0);
            statusStripInfo.Name = "statusStripInfo";
            statusStripInfo.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            statusStripInfo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            statusStripInfo.ShowItemToolTips = true;
            statusStripInfo.Size = new System.Drawing.Size(429, 25);
            statusStripInfo.SizingGrip = false;
            statusStripInfo.TabIndex = 2;
            statusStripInfo.Text = "statusStrip1";
            // 
            // toolStripStatusSize
            // 
            toolStripStatusSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            toolStripStatusSize.Name = "toolStripStatusSize";
            toolStripStatusSize.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            toolStripStatusSize.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            toolStripStatusSize.RightToLeft = System.Windows.Forms.RightToLeft.No;
            toolStripStatusSize.Size = new System.Drawing.Size(122, 25);
            toolStripStatusSize.Text = "750 x 750 px";
            toolStripStatusSize.ToolTipText = "Image Dimensions";
            // 
            // toolStripStatuxZoom
            // 
            toolStripStatuxZoom.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            toolStripStatuxZoom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            toolStripStatuxZoom.Name = "toolStripStatuxZoom";
            toolStripStatuxZoom.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            toolStripStatuxZoom.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            toolStripStatuxZoom.RightToLeft = System.Windows.Forms.RightToLeft.No;
            toolStripStatuxZoom.Size = new System.Drawing.Size(84, 25);
            toolStripStatuxZoom.Text = "100%";
            toolStripStatuxZoom.ToolTipText = "Image Zoom";
            // 
            // toolStripSettings
            // 
            toolStripSettings.BackColor = System.Drawing.SystemColors.Control;
            toolStripSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripSettings.GripMargin = new System.Windows.Forms.Padding(0);
            toolStripSettings.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripSettings.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStripSettings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripLabelTickness, toolStripTextBoxThickness, toolStripSeparator1, toolStripLabelTurns, toolStripTextBoxTurns });
            toolStripSettings.Location = new System.Drawing.Point(0, 0);
            toolStripSettings.Name = "toolStripSettings";
            toolStripSettings.Padding = new System.Windows.Forms.Padding(0);
            toolStripSettings.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            toolStripSettings.Size = new System.Drawing.Size(312, 25);
            toolStripSettings.TabIndex = 3;
            toolStripSettings.Text = "toolStrip1";
            // 
            // toolStripLabelTickness
            // 
            toolStripLabelTickness.Margin = new System.Windows.Forms.Padding(10, 0, 2, 0);
            toolStripLabelTickness.Name = "toolStripLabelTickness";
            toolStripLabelTickness.Size = new System.Drawing.Size(90, 25);
            toolStripLabelTickness.Text = "Line thickness:";
            toolStripLabelTickness.ToolTipText = "Pen/Stroke Thickness";
            // 
            // toolStripTextBoxThickness
            // 
            toolStripTextBoxThickness.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            toolStripTextBoxThickness.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            toolStripTextBoxThickness.Name = "toolStripTextBoxThickness";
            toolStripTextBoxThickness.Size = new System.Drawing.Size(50, 25);
            toolStripTextBoxThickness.Text = "2.0";
            toolStripTextBoxThickness.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            toolStripTextBoxThickness.ToolTipText = "Pen/Stroke Thickness";
            toolStripTextBoxThickness.TextChanged += toolStripTextBoxThickness_TextChanged;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabelTurns
            // 
            toolStripLabelTurns.ActiveLinkColor = System.Drawing.Color.RosyBrown;
            toolStripLabelTurns.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            toolStripLabelTurns.Name = "toolStripLabelTurns";
            toolStripLabelTurns.Size = new System.Drawing.Size(68, 25);
            toolStripLabelTurns.Text = "# of turns:";
            toolStripLabelTurns.ToolTipText = "# of Spiral Turns";
            // 
            // toolStripTextBoxTurns
            // 
            toolStripTextBoxTurns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            toolStripTextBoxTurns.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            toolStripTextBoxTurns.Name = "toolStripTextBoxTurns";
            toolStripTextBoxTurns.Size = new System.Drawing.Size(50, 25);
            toolStripTextBoxTurns.Text = "75";
            toolStripTextBoxTurns.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            toolStripTextBoxTurns.ToolTipText = "# of Spiral Turns";
            toolStripTextBoxTurns.TextChanged += toolStripTextBoxTurns_TextChanged;
            // 
            // splitContainer1
            // 
            splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer1.Location = new System.Drawing.Point(0, 747);
            splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            splitContainer1.MinimumSize = new System.Drawing.Size(532, 27);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(toolStripSettings);
            splitContainer1.Panel1MinSize = 310;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(statusStripInfo);
            splitContainer1.Panel2MinSize = 220;
            splitContainer1.Size = new System.Drawing.Size(750, 27);
            splitContainer1.SplitterDistance = 314;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 4;
            splitContainer1.SizeChanged += splitContainer1_SizeChanged;
            // 
            // MainForm
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(750, 774);
            Controls.Add(splitContainer1);
            Controls.Add(pictureBox1);
            Controls.Add(menuStrip);
            DoubleBuffered = true;
            MainMenuStrip = menuStrip;
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MinimumSize = new System.Drawing.Size(551, 331);
            Name = "MainForm";
            Text = "Spiralographize";
            Shown += MainForm_Shown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            statusStripInfo.ResumeLayout(false);
            statusStripInfo.PerformLayout();
            toolStripSettings.ResumeLayout(false);
            toolStripSettings.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byThicknessMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byRadiusMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem constantSpiralsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem constantSpiralMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goldenRatioMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orderbySpiralArms;
        private System.Windows.Forms.ToolStripMenuItem orderbyTopToBottom;
        private System.Windows.Forms.ToolStripMenuItem loadImageToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStripInfo;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusSize;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatuxZoom;
        private System.Windows.Forms.ToolStrip toolStripSettings;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxTurns;
        private System.Windows.Forms.ToolStripLabel toolStripLabelTurns;
        private System.Windows.Forms.ToolStripLabel toolStripLabelTickness;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxThickness;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveImageToolStripMenuItem1;
    }
}