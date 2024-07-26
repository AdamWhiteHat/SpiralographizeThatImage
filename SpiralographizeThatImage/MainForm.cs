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

namespace SpiralographizeThatImage
{
    public partial class MainForm : Form
    {
        private bool _isDirty = false;
        private Bitmap _bitmap = null;
        private string _mostRecentBrowseFolder = @"C:\Temp";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Bitmap blank = GetBlankImage();
            pictureBox1.Image = blank;
            pictureBox1.Enabled = false;
            _isDirty = false;
        }

        private Bitmap GetBlankImage()
        {
            Bitmap result = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.Clear(Color.Transparent);
            }
            return result;
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
            pictureBox1.Enabled = true;
            DrawSpiralFromImage(_bitmap);
        }

        private void DrawSpiralFromImage(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return;
            }

            int revolutionCount = ((bitmap.Width + bitmap.Height) / 2) / 10;
            LineSegment[] lineSegments = new LineSegment[0];

            if (byRadiusMenuItem.Checked)
            {
                lineSegments = Spiralographize.ModulatingRadius.GetLineSegments(_bitmap, revolutionCount);
            }
            else
            {
                lineSegments = Spiralographize.ModulatingThickness.GetLineSegments(bitmap, revolutionCount);
            }

            Bitmap buffer = GetBlankImage();
            using (Graphics graphics = Graphics.FromImage(buffer))
            {
                Spiralographize.DrawSpiral(graphics, lineSegments, Color.Black);
            }

            pictureBox1.Image = buffer;
            _isDirty = true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveWork();
        }

        private bool SaveWork()
        {
            if (!pictureBox1.Enabled || _bitmap == null)
            {
                return true;
            }

            string filename = null;
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Image files (*.bmp;*.png;*.jpg;*.jpeg;*.gif)|*.bmp;*.png;*.jpg;*.jpeg;*.gif|All files (*.*)|*.*";
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

            pictureBox1.Image.Save(filename, format);
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

        bool disableEvents = false;
        private void byThicknessMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (!disableEvents)
            {
                disableEvents = true;
                byRadiusMenuItem.Checked = !byThicknessMenuItem.Checked;
                disableEvents = false;
                DrawSpiralFromImage(_bitmap);
            }
        }

        private void byRadiusMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (!disableEvents)
            {
                disableEvents = true;
                byThicknessMenuItem.Checked = !byRadiusMenuItem.Checked;
                disableEvents = false;
                DrawSpiralFromImage(_bitmap);
            }
        }
    }
}
