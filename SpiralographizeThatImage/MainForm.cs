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
using SpiralographizeThatImage.Factories;
using SpiralographizeThatImage.Renderers;
using System.Diagnostics;

namespace SpiralographizeThatImage
{
    public partial class MainForm : Form
    {
        private float Zoom = 1.0f;
        private Bitmap _bitmap = null;

        private Bitmap _unzoomedPictureboxImage = null;
        private Bitmap _zoomedPictureboxImage = null;
        private bool _isDirty = false;
        private ImageSpirals _lastSelected_ImageSpiral = ImageSpirals.ByThickness;
        private bool disableEvents = false;
        private string _mostRecentBrowseFolder = @"C:\Temp";
        private ToolStripMenuItem debugMenuItem;

        private static readonly string Title = "Spiralographize";
        private static readonly string TitleFormat = "Spiralographize - [{0}]";

        public MainForm()
        {
            InitializeComponent();
            AddDebugOptionToMenu();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Bitmap blank = Renderer.ABlankImage(pictureBox1.Size);
            pictureBox1.Image = blank;
            _zoomedPictureboxImage = null;
            _unzoomedPictureboxImage = blank;

            _isDirty = false;
            pictureBox1.MouseWheel += PictureBox1_MouseWheel;
            Zoom = 1.0f;

            splitContainer1.SplitterWidth = 1;
            splitContainer1.MinimumSize = new Size(splitContainer1.Panel1MinSize + splitContainer1.Panel2MinSize + 2, 27);
            splitContainer1.SplitterDistance = splitContainer1.Panel1MinSize;
        }

        #region High-Level, public methods

        public void DrawSpiral()
        {
            if ((byThicknessMenuItem.Checked || byRadiusMenuItem.Checked) && _bitmap != null)
            {
                DrawSpiral_FromImage(_bitmap);
            }
            else if (constantSpiralMenuItem.Checked)
            {
                DrawSpiral_Common();
            }
            else if (orderbySpiralArms.Checked || orderbyTopToBottom.Checked)
            {
                DrawSpiral_Golden();
            }
        }

        public bool SaveImage()
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

        public void LoadImage()
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

            LoadImage(filename);
        }

        public void LoadImage(string filename)
        {
            Zoom = 1.0f;
            _bitmap = new Bitmap(Image.FromFile(filename));

            byThicknessMenuItem.Enabled = true;
            byRadiusMenuItem.Enabled = true;

            this.Text = string.Format(TitleFormat, Path.GetFileName(filename));

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

        public void ClearImage()
        {
            _bitmap = null;

            byThicknessMenuItem.Enabled = false;
            byRadiusMenuItem.Enabled = false;

            this.Text = Title;
        }

        #endregion

        #region Image Loading

        private void SetPictureBox(Bitmap bitmap)
        {
            Zoom = 1.0f;
            _zoomedPictureboxImage = null;
            _unzoomedPictureboxImage = new Bitmap(bitmap);
            pictureBox1.Image = _unzoomedPictureboxImage;
            _isDirty = true;
            UpdateZoomStatus();
        }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadImage();
        }

        private void saveImageMenuItem_Click(object sender, EventArgs e)
        {
            SaveImage();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isDirty)
            {
                if (MessageBox.Show("Your image has not been saved. Would you like to save it first?", "Unsaved work", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (!SaveImage())
                    {
                        return; // If user cancels the save dialog, dont exit.
                    }
                }
            }

            this.Close();
        }

        #endregion

        #region Spiral Drawing

        private int? GetOption_NumberOfTurns()
        {
            int? result = null;//((size.Width + size.Height) / 2) / 10;
            if (int.TryParse(toolStripTextBoxTurns.Text, out int parsedTurns))
            {
                result = parsedTurns;
            }
            return result;
        }

        private float? GetOption_Thickness()
        {
            float? result = null;
            if (float.TryParse(toolStripTextBoxThickness.Text, out float parsedThickness))
            {
                result = parsedThickness;
            }
            return result;
        }

        private void DrawSpiral_FromImage(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return;
            }

            int? revolutionCount = GetOption_NumberOfTurns();
            if (!revolutionCount.HasValue)
            {
                return;
            }

            float? thickness = GetOption_Thickness();
            if (!thickness.HasValue)
            {
                return;
            }

            LineSegment[] lineSegments = new LineSegment[0];

            if (byRadiusMenuItem.Checked)
            {
                lineSegments = ModulateRadiusSpiral.GetGeometryElements(bitmap, revolutionCount.Value);
            }
            else if (byThicknessMenuItem.Checked)
            {
                lineSegments = ModulateThicknessSpiral.GetGeometryElements(bitmap, revolutionCount.Value, thickness.Value);
            }

            Size size = pictureBox1.Size;
            Bitmap buffer = Renderer.ABlankImage(size);
            using (Graphics graphics = Graphics.FromImage(buffer))
            {
                Renderer.ALine.DrawSpiral(graphics, lineSegments, Color.Black);
            }

            SetPictureBox(buffer);
        }

        private void DrawSpiral_Golden()
        {
            if (!orderbySpiralArms.Checked && !orderbyTopToBottom.Checked)
            {
                return;
            }

            ClearImage();

            float? thickness = GetOption_Thickness();
            if (!thickness.HasValue)
            {
                return;
            }

            int? revolutionCount = GetOption_NumberOfTurns();
            if (!revolutionCount.HasValue)
            {
                return;
            }

            double phiAngle = 360d - (360d * (GoldenRatioSpiral.Phi - 1d));

            int pointQuantity = (int)Math.Round(((double)revolutionCount.Value * Math.PI * 360d) / phiAngle);

            Size size = new Size(pictureBox1.Width, pictureBox1.Height);
            PointF[] points = GoldenRatioSpiral.GetGeometryElements(size, pointQuantity);

            Bitmap buffer = Renderer.ABlankImage(size);
            using (Graphics graphics = Graphics.FromImage(buffer))
            {
                Renderer.APoint.DrawSpiral(graphics, points, Color.Black, thickness.Value);
            }

            SetPictureBox(buffer);

            //Pen _pen = PenCache.GetPen(Color.Black, DefaultThickness + 1);
            //SizeF _size = new SizeF(DefaultThickness, DefaultThickness);
            //
            //Graphics _graphics = pictureBox.CreateGraphics();
            //int i = 0;
            //while (i < totalFrames)
            //{
            //    _graphics.DrawEllipse(_pen, new RectangleF(results[i++], _size));
            //}
            //_graphics.Flush()
        }

        private void DrawSpiral_Common()
        {
            if (!constantSpiralMenuItem.Checked)
            {
                return;
            }

            ClearImage();

            float? thickness = GetOption_Thickness();
            if (!thickness.HasValue)
            {
                return;
            }

            int? revolutionCount = GetOption_NumberOfTurns();
            if (!revolutionCount.HasValue)
            {
                return;
            }

            Size size = pictureBox1.Size;

            LineSegment[] lineSegments = ConstantSpiral.GetGeometryElements(size, revolutionCount.Value, thickness.Value);

            Bitmap buffer = Renderer.ABlankImage(size);
            using (Graphics graphics = Graphics.FromImage(buffer))
            {
                Renderer.ALine.DrawSpiral(graphics, lineSegments, Color.Black);
            }

            SetPictureBox(buffer);
        }

        private void byThicknessMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (disableEvents) return;

            _lastSelected_ImageSpiral = ImageSpirals.ByThickness;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                ClearMenuChecks();
                byThicknessMenuItem.Checked = true;

                DrawSpiral_FromImage(_bitmap);
            }
        }

        private void byRadiusMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (disableEvents) return;

            _lastSelected_ImageSpiral = ImageSpirals.ByRadius;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                ClearMenuChecks();
                byRadiusMenuItem.Checked = true;

                DrawSpiral_FromImage(_bitmap);
            }
        }

        private void constantSpiralMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (disableEvents) return;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                ClearMenuChecks();
                constantSpiralMenuItem.Checked = true;

                DrawSpiral_Common();
            }
        }

        private void goldenRatioMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (disableEvents) return;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                if (!orderbySpiralArms.Checked && !orderbyTopToBottom.Checked)
                {
                    goldenRatioMenuItem.Checked = false;
                }
                else
                {
                    goldenRatioMenuItem.Checked = true;
                }
            }
        }

        private void orderbySpiralArms_CheckedChanged(object sender, EventArgs e)
        {
            if (disableEvents) return;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                ClearMenuChecks();
                goldenRatioMenuItem.Checked = true;
                orderbySpiralArms.Checked = true;

                DrawSpiral_Golden();
            }
        }

        private void orderbyTopToBottom_CheckedChanged(object sender, EventArgs e)
        {
            if (disableEvents) return;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                ClearMenuChecks();
                goldenRatioMenuItem.Checked = true;
                orderbyTopToBottom.Checked = true;

                DrawSpiral_Golden();
            }
        }

        private void ClearMenuChecks()
        {
            byRadiusMenuItem.Checked = false;
            byThicknessMenuItem.Checked = false;
            orderbySpiralArms.Checked = false;
            orderbyTopToBottom.Checked = false;
            constantSpiralMenuItem.Checked = false;
            goldenRatioMenuItem.Checked = false;
            if (debugMenuItem != null)
            {
                debugMenuItem.Checked = false;
            }
        }

        [Conditional("DEBUG")]
        private void AddDebugOptionToMenu()
        {
            debugMenuItem = new ToolStripMenuItem();
            debugMenuItem.Name = "debugMenuItem";
            debugMenuItem.Size = new System.Drawing.Size(332, 26);
            debugMenuItem.Text = "Draw Calibration Lines";
            debugMenuItem.Click += debugMenuItem_Clicked;
            debugMenuItem.CheckOnClick = true;
            constantSpiralsToolStripMenuItem.DropDownItems.Add(debugMenuItem);
        }

        private void debugMenuItem_Clicked(object sender, EventArgs e)
        {
            if (disableEvents) return;

            using (new ToggleScope((state) => { disableEvents = state; }))
            {
                ClearMenuChecks();
                debugMenuItem.Checked = true;
            }

            Bitmap buffer = Renderer.ABlankImage(pictureBox1.Size);

            LineSegment[] lineSegments1 = CalibrationSpiral.GetGeometryElements(buffer, 1);
            LineSegment[] lineSegments2 = CalibrationSpiral.GetGeometryElements(buffer, 2);
            LineSegment[] lineSegments3 = CalibrationSpiral.GetGeometryElements(buffer, 4);
            LineSegment[] lineSegments4 = CalibrationSpiral.GetGeometryElements(buffer, 8);

            using (Graphics graphics = Graphics.FromImage(buffer))
            {
                Renderer.ALine.DrawSpiral(graphics, lineSegments1.ToArray(), Color.Black);
                Renderer.ALine.DrawSpiral(graphics, lineSegments2.ToArray(), Color.Black);
                Renderer.ALine.DrawSpiral(graphics, lineSegments3.ToArray(), Color.Black);
                Renderer.ALine.DrawSpiral(graphics, lineSegments4.ToArray(), Color.Black);
            }

            SetPictureBox(buffer);
        }

        #endregion

        #region PictureBox Zoom

        private void PictureBox1_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                int delta = e.Delta / 120;
                if (delta == 0)
                {
                    return;
                }

                Zoom += 0.2f * delta;

                UpdateZoomStatus();
                DrawZoomedImage();
            }
        }

        private void DrawZoomedImage()
        {
            if (_unzoomedPictureboxImage == null)
            {
                return;
            }

            if (Zoom == 1.0f)
            {
                pictureBox1.Image = _unzoomedPictureboxImage;
                return;
            }

            int sourceWidth = _unzoomedPictureboxImage.Width;
            int sourceHeight = _unzoomedPictureboxImage.Height;

            int targetWidth = (int)Math.Round(sourceWidth * Zoom);
            int targetHeight = (int)Math.Round(sourceHeight * Zoom);
            int targetTop = 0; //(sourceHeight - targetHeight) / 2;
            int targetLeft = 0;//(sourceWidth - targetWidth) / 2;

            _zoomedPictureboxImage = new Bitmap(sourceWidth, sourceHeight, _unzoomedPictureboxImage.PixelFormat);

            // Set the resolution of the bitmap to match the original resolution.
            _zoomedPictureboxImage.SetResolution(_unzoomedPictureboxImage.HorizontalResolution, _unzoomedPictureboxImage.VerticalResolution);

            using (Graphics bmGraphics = Graphics.FromImage(_zoomedPictureboxImage))
            {

                // First clear the image with the current BackColor
                bmGraphics.Clear(Color.Transparent);

                // Set the InterpolationMode since we are resizing an image here
                bmGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // Draw the original image on the temporary bitmap, resizing it using
                // the calculated values of targetWidth and targetHeight.
                bmGraphics.DrawImage(_unzoomedPictureboxImage,
                                     new Rectangle(targetLeft, targetTop, targetWidth, targetHeight),
                                     new Rectangle(0, 0, sourceWidth, sourceHeight),
                                    GraphicsUnit.Pixel);
            }

            pictureBox1.Image = _zoomedPictureboxImage;
        }

        #endregion

        #region Status Strip

        private static string _statusSizeFormat = "{0} × {1} px";
        private static string _statusZoomFormat = "{0:#0.#' %'}";

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            toolStripStatusSize.Text = string.Format(_statusSizeFormat, pictureBox1.Width, pictureBox1.Height);
        }

        private void splitContainer1_SizeChanged(object sender, EventArgs e)
        {
            int maxSplitterDistance = splitContainer1.Size.Width - splitContainer1.Panel2MinSize - splitContainer1.SplitterWidth - 2;

            if (splitContainer1.SplitterDistance > maxSplitterDistance)
            {
                int diff = splitContainer1.SplitterDistance - maxSplitterDistance;
                splitContainer1.SplitterDistance -= diff;
            }
        }

        private void UpdateZoomStatus()
        {
            float zoomPercent = 100f * Zoom;
            toolStripStatuxZoom.Text = string.Format(_statusZoomFormat, zoomPercent);
        }

        private void toolStripTextBoxTurns_TextChanged(object sender, EventArgs e)
        {
            DrawSpiral();
        }

        private void toolStripTextBoxThickness_TextChanged(object sender, EventArgs e)
        {
            DrawSpiral();
        }

        private void StatusStripCustomization()
        {
            ToolStripTextBox turns = new ToolStripTextBox();
            turns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            turns.Margin = new System.Windows.Forms.Padding(1, 0, 10, 0);
            turns.Name = "statusStrip_Turns";
            turns.Size = new System.Drawing.Size(50, 25);
            turns.Text = "75";
            turns.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            turns.TextChanged += toolStripTextBoxTurns_TextChanged;

            statusStripInfo.Items.Add(turns);

            ToolStripLabel turnsLabel = new ToolStripLabel();
            toolStripLabelTurns.Margin = new System.Windows.Forms.Padding(20, 1, 2, 2);
            toolStripLabelTurns.Name = "statusStrip_TurnsLabel";
            toolStripLabelTurns.Size = new System.Drawing.Size(75, 22);
            toolStripLabelTurns.Text = "# of turns:";
        }

        #endregion

    }
}
