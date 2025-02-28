using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Linq.Expressions;
using static SpiralographizeThatImage.Spiralographize;
using static SpiralographizeThatImage.Spiralographize.GoldenRatioSpiral;
using System.Security.Policy;
using System.Diagnostics;

namespace SpiralographizeThatImage
{
    public partial class MainForm : Form
    {
        private Bitmap _bitmap = null;
        private bool _isDirty = false;
        private ImageSpirals _lastSelected_ImageSpiral = ImageSpirals.ByThickness;
        private bool disableEvents = false;
        private string _mostRecentBrowseFolder = @"C:\Temp";

        private static readonly string Title = "Spiralographize";
        private static readonly string TitleFormat = "Spiralographize - [{0}]";

        public MainForm()
        {
            InitializeComponent();
            AddDebugOptionToMenu();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Bitmap blank = Renderer.ABlankImage(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = blank;
            _isDirty = false;
        }

        private void SetPictureBox(Bitmap bitmap)
        {
            pictureBox1.Image = bitmap;
            _isDirty = true;
        }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = null;
            using (OpenFileDialog browseDialog = new OpenFileDialog())
            {
                browseDialog.Filter = "Image files (*.bmp;*.png;*.jpg;*.jpeg;*.gif)|*.bmp;*.png;*.jpg;*.jpeg;*.gif|All files (*.*)|*.*";
                browseDialog.InitialDirectory = _mostRecentBrowseFolder;
                if (browseDialog.ShowDialog() == DialogResult.OK)
                {
                    filename = browseDialog.FileName;
                    _mostRecentBrowseFolder = Path.GetDirectoryName(browseDialog.FileName);
                }
            }
            if (filename == null || !File.Exists(filename))
            {
                return;
            }

            SetLoadedImage(filename);

            if (!byRadiusMenuItem.Checked && !byThicknessMenuItem.Checked)
            {
                if (_lastSelected_ImageSpiral == ImageSpirals.ByRadius)
                {
                    byRadiusMenuItem.Checked = true;
                }
                else if (_lastSelected_ImageSpiral == ImageSpirals.ByThickness)
                {
                    byThicknessMenuItem.Checked = true;
                }
            }
            else
            {
                DrawSpiral_FromImage(_bitmap);
            }
        }

        private void SetLoadedImage(string filename)
        {
            _bitmap = new Bitmap(Image.FromFile(filename));

            byThicknessMenuItem.Enabled = true;
            byRadiusMenuItem.Enabled = true;

            this.Text = string.Format(TitleFormat, Path.GetFileName(filename));
        }

        private void ClearLoadedImage()
        {
            _bitmap = null;

            byThicknessMenuItem.Enabled = false;
            byRadiusMenuItem.Enabled = false;

            this.Text = Title;
        }

        private void DrawSpiral_FromImage(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return;
            }

            Size size = new Size(pictureBox1.Width, pictureBox1.Height);
            int revolutionCount = ((bitmap.Width + bitmap.Height) / 2) / 10;
            LineSegment[] lineSegments = new LineSegment[0];

            if (byRadiusMenuItem.Checked)
            {
                lineSegments = Spiralographize.ModulatingRadius.GetGeometrySegments(bitmap, revolutionCount);
            }
            else if (byThicknessMenuItem.Checked)
            {
                lineSegments = Spiralographize.ModulatingThickness.GetGeometrySegments(bitmap, revolutionCount);
            }

            Bitmap buffer = Renderer.ABlankImage(size.Width, size.Height);
            using (Graphics graphics = Graphics.FromImage(buffer))
            {
                Spiralographize.Renderer.ALine.DrawSpiral(graphics, lineSegments, Color.Black);
            }

            SetPictureBox(buffer);
        }

        private void DrawSpiral_Golden(DrawOrder drawOrder)
        {
            if (!orderbySpiralArms.Checked && !orderbyTopToBottom.Checked)
            {
                return;
            }

            ClearLoadedImage();

            Bitmap buffer = Renderer.ABlankImage(pictureBox1.Width, pictureBox1.Height);
            SetPictureBox(buffer);

            Spiralographize.GoldenRatioSpiral.Render(pictureBox1, drawOrder);
        }

        private void DrawSpiral_Common()
        {
            if (!constantSpiralMenuItem.Checked)
            {
                return;
            }

            ClearLoadedImage();

            Size size = new Size(pictureBox1.Width, pictureBox1.Height);
            int revolutionCount = ((size.Width + size.Height) / 2) / 10;
            LineSegment[] lineSegments = Spiralographize.ConstantSpiral.GetGeometrySegments(size, revolutionCount);

            Bitmap buffer = Renderer.ABlankImage(size.Width, size.Height);
            using (Graphics graphics = Graphics.FromImage(buffer))
            {
                Spiralographize.Renderer.ALine.DrawSpiral(graphics, lineSegments, Color.Black);
            }

            SetPictureBox(buffer);
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveWork();
        }

        private bool SaveWork()
        {
            string filename = null;
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Image files (*.bmp;*.png;*.jpg;*.jpeg;*.gif)|*.bmp;*.png;*.jpg;*.jpeg;*.gif|All files (*.*)|*.*";
                saveDialog.FilterIndex = 0;
                saveDialog.InitialDirectory = _mostRecentBrowseFolder;
                saveDialog.DefaultExt = "png";
                saveDialog.AddExtension = true;
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    filename = saveDialog.FileName;
                    _mostRecentBrowseFolder = Path.GetDirectoryName(saveDialog.FileName);
                }
            }
            if (filename == null)
            {
                return false; // User canceled, return false.
            }

            ImageFormat format = ImageFormat.Jpeg;
            string ext = Path.GetExtension(filename).Trim(new char[] { '.' }).ToLower();

            if (ext == "bmp")
            {
                format = ImageFormat.Bmp;
            }
            if (ext == "png")
            {
                format = ImageFormat.Png;
            }
            if (ext == "jpg" || ext == "jpeg")
            {
                format = ImageFormat.Jpeg;
            }
            if (ext == "gif")
            {
                format = ImageFormat.Gif;
            }

            if (format == ImageFormat.Png)
            {
                pictureBox1.Image.Save(filename, format);
            }
            else
            {
                Bitmap copy = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppRgb);
                using (Graphics g = Graphics.FromImage(copy))
                {
                    g.Clear(Color.White);
                    g.DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
                    g.Flush();
                }
                copy.Save(filename, format);
            }


            _isDirty = false;
            return true;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isDirty)
            {
                if (MessageBox.Show("Your image has not been saved. Would you like to save it first?", "Unsaved work", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (!SaveWork())
                    {
                        return; // If user cancels the save dialog, dont exit.
                    }
                }
            }

            this.Close();
        }

        private void byThicknessMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (disableEvents) return;

            _lastSelected_ImageSpiral = ImageSpirals.ByThickness;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                byThicknessMenuItem.Checked = true;
                byRadiusMenuItem.Checked = false;
                orderbySpiralArms.Checked = false;
                orderbyTopToBottom.Checked = false;
                constantSpiralMenuItem.Checked = false;

                DrawSpiral_FromImage(_bitmap);
            }
        }

        private void byRadiusMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (disableEvents) return;

            _lastSelected_ImageSpiral = ImageSpirals.ByRadius;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                byRadiusMenuItem.Checked = true;
                byThicknessMenuItem.Checked = false;
                orderbySpiralArms.Checked = false;
                orderbyTopToBottom.Checked = false;
                constantSpiralMenuItem.Checked = false;

                DrawSpiral_FromImage(_bitmap);
            }
        }

        private void constantSpiralMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (disableEvents) return;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                constantSpiralMenuItem.Checked = true;
                byThicknessMenuItem.Checked = false;
                byRadiusMenuItem.Checked = false;
                orderbySpiralArms.Checked = false;
                orderbyTopToBottom.Checked = false;

                DrawSpiral_Common();
            }
        }

        private void orderbySpiralArms_CheckedChanged(object sender, EventArgs e)
        {
            if (disableEvents) return;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                orderbySpiralArms.Checked = true;
                orderbyTopToBottom.Checked = false;
                byThicknessMenuItem.Checked = false;
                byRadiusMenuItem.Checked = false;
                constantSpiralMenuItem.Checked = false;

                DrawSpiral_Golden(DrawOrder.BySpiralArm);
            }
        }

        private void orderbyTopToBottom_CheckedChanged(object sender, EventArgs e)
        {
            if (disableEvents) return;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                orderbyTopToBottom.Checked = true;
                orderbySpiralArms.Checked = false;
                byThicknessMenuItem.Checked = false;
                byRadiusMenuItem.Checked = false;
                constantSpiralMenuItem.Checked = false;

                DrawSpiral_Golden(DrawOrder.FromTopToBottom);
            }
        }

        [Conditional("DEBUG")]
        private void AddDebugOptionToMenu()
        {
            ToolStripMenuItem debugMenuItem = new ToolStripMenuItem();
            debugMenuItem.Name = "debugMenuItem";
            debugMenuItem.Size = new System.Drawing.Size(332, 26);
            debugMenuItem.Text = "Draw Calibration Lines";
            debugMenuItem.Click += debugMenuItem_Clicked;
            constantSpiralsToolStripMenuItem.DropDownItems.Add(debugMenuItem);
        }

        private void debugMenuItem_Clicked(object sender, EventArgs e)
        {
            if (disableEvents) return;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                byRadiusMenuItem.Checked = false;
                byThicknessMenuItem.Checked = false;
                orderbySpiralArms.Checked = false;
                orderbyTopToBottom.Checked = false;
                constantSpiralMenuItem.Checked = false;
            }

            Bitmap buffer = Renderer.ABlankImage(pictureBox1.Width, pictureBox1.Height);



            LineSegment[] lineSegments1 = Spiralographize.Debug.Calibration.GetGeometrySegments(buffer, 1);
            LineSegment[] lineSegments2 = Spiralographize.Debug.Calibration.GetGeometrySegments(buffer, 2);
            LineSegment[] lineSegments3 = Spiralographize.Debug.Calibration.GetGeometrySegments(buffer, 4);
            LineSegment[] lineSegments4 = Spiralographize.Debug.Calibration.GetGeometrySegments(buffer, 8);

            using (Graphics graphics = Graphics.FromImage(buffer))
            {
                Spiralographize.Renderer.ALine.DrawSpiral(graphics, lineSegments1.ToArray(), Color.Black);
                Spiralographize.Renderer.ALine.DrawSpiral(graphics, lineSegments2.ToArray(), Color.Black);
                Spiralographize.Renderer.ALine.DrawSpiral(graphics, lineSegments3.ToArray(), Color.Black);
                Spiralographize.Renderer.ALine.DrawSpiral(graphics, lineSegments4.ToArray(), Color.Black);
            }

            SetPictureBox(buffer);
        }


    }
}
