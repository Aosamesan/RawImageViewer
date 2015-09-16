using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RawImageViewer
{
    public partial class ImageTabItem : UserControl, INotifyPropertyChanged
    {
        OpenFileDialog ofd;
        Image rawImage;
        Image RawImage
        {
            get { return rawImage; }
            set
            {
                rawImage?.Dispose();
                rawImage = value;
                OnPropertyChanged(nameof(RawImage));
            }
        }
        string imageName;
        public string ImageName
        {
            get { return imageName; }
            set
            {
                imageName = value;
                OnPropertyChanged(nameof(ImageName));
            }
        }
        byte[] imageBytes;
        public int ImageWidth { get; private set; }

        public event Action<object, EventArgs> UpdateTooltip;

        int fileSize;
        public int FileSize
        {
            get { return fileSize; }
            private set
            {
                fileSize = value;
                OnPropertyChanged(nameof(FileSize));
            }
        }

        public int ImageHeight
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(FileSize) / ImageWidth));
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string n)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
        public Action removePage;

        public ImageTabItem()
        {
            InitializeComponent();

            Dock = DockStyle.Fill;
            ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Raw Image File|*.raw";
            ImageName = "Untitled";
            FileSize = 0;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(ofd.FileName))
                {
                    using(var fs = File.OpenRead(ofd.FileName))
                    {
                        long length = fs.Length;
                        imageBytes = new byte[fs.Length];
                        fs.Read(imageBytes, 0, imageBytes.Length);
                        setWidth();
                        ImageName = ofd.SafeFileName;
                        FileSize = Convert.ToInt32(length);
                        UpdateTooltip?.Invoke(null, null);
                    }
                }
            }
        }

        private void setWidth()
        {
            WidthSetter ws = new WidthSetter(ImageWidth);
            if (ws.ShowDialog() == DialogResult.OK)
            {
                ImageWidth = ws.MyWidth;

                RawImage = GetBitmapFromByteArray(imageBytes, 1, ImageWidth);
                pictureBox.Image = RawImage;
                UpdateTooltip?.Invoke(null, null);
            }
        }

        public static Bitmap GetBitmapFromByteArray(byte[] bytes, int pixelSize = 1, int width = 250)
        {
            if (bytes == null)
                return null;
            if (width < 1)
                return null;

            int height = bytes.Length / width;
            Bitmap b = new Bitmap(width * pixelSize, height * pixelSize);
            Pen p = new Pen(Brushes.Transparent);
            using (Graphics g = Graphics.FromImage(b))
            {
                for (int r = 0; r < height; r++)
                {
                    for (int c = 0; c < width; c++)
                    {
                        p.Color = GetGrayColorFromByte(bytes[c + r * width]);
                        g.DrawRectangle(p, c * pixelSize, r * pixelSize, pixelSize, pixelSize);
                    }
                }
            }

            return b;
        }

        public static Color GetGrayColorFromByte(byte b)
        {
            return Color.FromArgb(b, b, b);
        }

        private void changeWidthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setWidth();
        }

        private void removeThisTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Sure?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Invoke(removePage);
            }
        }
    }
}
