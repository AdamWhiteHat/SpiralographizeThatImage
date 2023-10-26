using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SpiralographizeThatImage
{
	public partial class MainForm : Form
	{
		private Bitmap _bitmap = null;
		private string _mostRecentBrowseFolder = @"C:\Temp";

		public MainForm()
		{
			InitializeComponent();
		}

		private void Style2_Shown(object sender, EventArgs e)
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
			pictureBox1.Image = _bitmap;
			pictureBox1.Enabled = true;
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs paintEventArgs)
		{
			PictureBox source = (PictureBox)sender;
			if (source.Enabled && _bitmap != null)
			{
				int revolutionCount = ((_bitmap.Width + _bitmap.Height) / 2) / 10;

				LineSegment[] lineSegments = Spiralographize.GetLineSegments(_bitmap, revolutionCount);
				Spiralographize.DrawSpiral(paintEventArgs.Graphics, lineSegments, Color.Black);

				source.Image = null;
			}
		}
	}
}
