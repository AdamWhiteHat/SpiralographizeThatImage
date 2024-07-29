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

namespace SpiralographizeThatImage
{
    public partial class MainForm : Form
    {
        private Bitmap _bitmap = null;
        private bool _isDirty = false;
        private bool disableEvents = false;
        private string _mostRecentBrowseFolder = @"C:\Temp";

        public MainForm()
        {
            InitializeComponent();
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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
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

            _bitmap = new Bitmap(Image.FromFile(filename));
            DrawSpiralFromImage(_bitmap);
        }

        private void DrawSpiralFromImage(Bitmap bitmap)
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
            else if (constantSpiralMenuItem.Checked)
            {
                lineSegments = Spiralographize.ConstantSpiral.GetGeometrySegments(size, revolutionCount);
            }
            else if (goldenRatioMenuItem.Checked)
            {
                DrawSpiral();
                return;
            }

            Bitmap buffer = Renderer.ABlankImage(pictureBox1.Width, pictureBox1.Height);
            using (Graphics graphics = Graphics.FromImage(buffer))
            {
                Spiralographize.Renderer.ALine.DrawSpiral(graphics, lineSegments, Color.Black);
            }

            SetPictureBox(buffer);
        }

        private void DrawSpiral()
        {
            if (!goldenRatioMenuItem.Checked)
            {
                return;
            }

            Bitmap buffer = Renderer.ABlankImage(pictureBox1.Width, pictureBox1.Height);
            SetPictureBox(buffer);

            Spiralographize.GoldenRatioSpiral.Render(pictureBox1);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
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
                saveDialog.DefaultExt = "jpg";
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

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                byThicknessMenuItem.Checked = true;
                byRadiusMenuItem.Checked = false;
                goldenRatioMenuItem.Checked = false;
                constantSpiralMenuItem.Checked = false;

                DrawSpiralFromImage(_bitmap);
            }
        }

        private void byRadiusMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (disableEvents) return;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                byRadiusMenuItem.Checked = true;
                byThicknessMenuItem.Checked = false;
                goldenRatioMenuItem.Checked = false;
                constantSpiralMenuItem.Checked = false;

                DrawSpiralFromImage(_bitmap);
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
                goldenRatioMenuItem.Checked = false;

                if (_bitmap == null)
                {
                    _bitmap = Renderer.ABlankImage(pictureBox1.Width, pictureBox1.Height);
                }

                DrawSpiralFromImage(_bitmap);
            }
        }

        private void goldenRatioMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (disableEvents) return;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                goldenRatioMenuItem.Checked = true;
                byThicknessMenuItem.Checked = false;
                byRadiusMenuItem.Checked = false;
                constantSpiralMenuItem.Checked = false;

                DrawSpiral();
            }
        }

    }
}
