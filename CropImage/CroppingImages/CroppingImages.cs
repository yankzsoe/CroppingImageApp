using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace CropImage.CroppingImages
{
    public partial class CroppingImages : Form
    {
        public static bool checkedEdited { get; set; }
        Boolean mouseClicked;
        Point startPoint = new Point();
        Point endPoint = new Point();
        Rectangle rectCropArea;
        Bitmap sourceBitmap;
        public static string _saveTo { get; set; }
        public CroppingImages(string _PathImage)
        {
            InitializeComponent();
            pictureBox1.Image = Image.FromFile(_PathImage);
        }

        public static double perX { get; set; }
        public static double perY { get; set; }
        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpTemp = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpTemp;
        }

        private void showMessage()
        {
            sourceBitmap = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);
            frmMessageBox frmMessage = new frmMessageBox(cropImage(sourceBitmap, rectCropArea));
            DialogResult dr = frmMessage.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        public void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseClicked = true;
            startPoint.X = e.X;
            startPoint.Y = e.Y;
            endPoint.X = -1;
            endPoint.Y = -1;
            rectCropArea = new Rectangle(new Point(e.X, e.Y), new Size());
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int _x;
            int _y;
            Point ptCurrent = new Point(e.X, e.Y);

            if (mouseClicked)
            {
                endPoint = ptCurrent;
                if (e.X > startPoint.X && e.Y > startPoint.Y)
                { //ditarik ke kanan bawah
                    _x = e.X - startPoint.X;
                    _y = e.Y - startPoint.Y;
                    if (_x > _y)
                    {
                        rectCropArea.Width = _x;
                        rectCropArea.Height = (int)(perY / perX) * _x;
                    }
                    else
                    {
                        rectCropArea.Height = _y;
                        rectCropArea.Width = (int)(perX / perY) * _y;
                    }
                    rectCropArea.X = startPoint.X;
                    rectCropArea.Y = startPoint.Y;
                }
                else if (e.X < startPoint.X && e.Y > startPoint.Y)
                { //ditarik ke kiri bawah
                    _x = startPoint.X - e.X;
                    _y = e.Y - startPoint.Y;
                    if (_x > _y)
                    {
                        rectCropArea.Width = _x;
                        rectCropArea.Height = (int)(perY / perX) * _x;
                    }
                    else
                    {
                        rectCropArea.Height = _y;
                        rectCropArea.Width = (int)(perX / perY) * _y;
                    }
                    rectCropArea.X = e.X;
                    rectCropArea.Y = startPoint.Y;
                }
                else if (e.X > startPoint.X && e.Y < startPoint.Y)
                { //ditarik ke kanan atas
                    _x = e.X - startPoint.X;
                    _y = startPoint.Y - e.Y;
                    if (_x > _y)
                    {
                        rectCropArea.Width = _x;
                        rectCropArea.Height = (int)(perY / perX) * _x;
                    }
                    else
                    {
                        rectCropArea.Height = _y;
                        rectCropArea.Width = (int)(perX / perY) * _y;
                    }
                    rectCropArea.X = startPoint.X;
                    rectCropArea.Y = e.Y;
                }
                else
                {                                            //ditarik ke kiri atas
                    _x = startPoint.X - e.X;
                    _y = startPoint.Y - e.Y;
                    if (_x > _y)
                    {
                        rectCropArea.Width = _x;
                        rectCropArea.Height = (int)(perY / perX) * _x;
                    }
                    else
                    {
                        rectCropArea.Height = _y;
                        rectCropArea.Width = (int)(perX / perY) * _y;
                    }
                    rectCropArea.X = e.X;
                    rectCropArea.Y = e.Y;
                }
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseClicked = false;

            if (endPoint.X != -1)
            {
                Point currentPoint = new Point(e.X, e.Y);
            }
            endPoint.X = -1;
            endPoint.Y = -1;
            startPoint.X = -1;
            startPoint.Y = -1;
            if (rectCropArea.Height != 0)
            {
                showMessage();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if(pictureBox1.Image != null)
            {
                using(Pen drawLine = new Pen(Color.SpringGreen))
                {
                    drawLine.DashStyle = DashStyle.Solid;
                    e.Graphics.DrawRectangle(drawLine, rectCropArea);
                }
            }
        }
        public Image getCropImage()
        {
            Image image = null;
            return image = cropImage(sourceBitmap, rectCropArea);
        }
        public Bitmap ResizeImage(Bitmap b, int nWidth, int nHeight)
        {
            int _pixel = 38;
            int x = nWidth * _pixel;
            int y = nHeight * _pixel;

            Bitmap result = new Bitmap(x, y);
            using (Graphics g = Graphics.FromImage((Image)result))
            {
                g.DrawImage(b, 0, 0, x, y);
            }
            return result;
        }
        private bool ThubnailCallback()
        {
            return false;
        }
        public void resizeAndSave()
        {
            int _pixel = 38;
            int x = Convert.ToInt32(perX) * _pixel;
            int y = Convert.ToInt32(perY) * _pixel;

            Image.GetThumbnailImageAbort callback = new Image.GetThumbnailImageAbort(ThubnailCallback);
            Bitmap bitmap = new Bitmap(getCropImage());
            Image myThubnail = bitmap.GetThumbnailImage(x, y, callback, IntPtr.Zero);
            myThubnail.Save(_saveTo);
            myThubnail.Dispose();
            bitmap.Dispose();
        }
        private void CroppingImages_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
        }

        /*
        byte[] convertImageToBinary (Image img) {
            using (MemoryStream ms = new MemoryStream ()) {
                img.Save (ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray ();
            }
        }
        Image convertBinaryToImage (byte[] data) {
            using (MemoryStream ms = new MemoryStream (data)) {
                return Image.FromStream (ms);
            }
        }

        public void SaveToLocal (string path) {
            Image img = cropImage (sourceBitmap, rectCropArea);
            Bitmap bitmap = img as Bitmap;
            bitmap.Save (path);
            bitmap.Dispose ();
        }
        public byte[] ImageAsByte () {
            byte[] bit = null;
            bit = convertImageToBinary (cropImage (sourceBitmap, rectCropArea));
            return bit;
        }
        private void CropImage () {
            sourceBitmap = new Bitmap (pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage (sourceBitmap);
            g.DrawImage (sourceBitmap, new Rectangle (0, 0, pictureBox1.Width, pictureBox1.Height), rectCropArea, GraphicsUnit.Pixel);
        }
        */
    }
}