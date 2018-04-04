using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Implementasi
{
    public partial class FormTesting : Form
    {
        public FormTesting()
        {
            InitializeComponent();
            CropImage.CroppingImages.CroppingImages.perX = 5;
            CropImage.CroppingImages.CroppingImages.perY = 10;
        }
        CropImage.CroppingImages.CroppingImages mainForm;
        Image _originalImage;
        private void Crop_Click(object sender, EventArgs e)
        {
            mainForm = new CropImage.CroppingImages.CroppingImages(@"D:\logs\pesawat.jpg");
            DialogResult dr = mainForm.ShowDialog();
            if(dr == DialogResult.OK)
            {
                MessageBox.Show("Sudah diedit");
                pictureBox1.Image = mainForm.getCropImage();
                pictureBox1.Refresh();
            }
            else
            {
                MessageBox.Show("Belum diedit");
            }
            /* pengecekan image sudah diedit atau belum bisa juga menggunakan ini
            if (CropImage.CroppingImages.MainForm.checkedEdited)
            {

            }
            */
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = _originalImage.Clone() as Image;
        }

        private void FormTesting_Load(object sender, EventArgs e)
        {
            _originalImage = pictureBox1.Image.Clone() as Image;
        }
       
        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you want to save this image?","Message",MessageBoxButtons.OKCancel,MessageBoxIcon.Question,MessageBoxDefaultButton.Button1);
            if(dr == DialogResult.OK)
            {
                CropImage.CroppingImages.CroppingImages._saveTo = @"D:\logs\1.jpg";
                mainForm.resizeAndSave();

                Bitmap image = pictureBox1.Image as Bitmap;
                Bitmap imgOutput = mainForm.ResizeImage(image, 4, 3); 
                imgOutput.Save(@"D:\logs\temp.jpg");
                MessageBox.Show("Successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                image.Dispose();
                imgOutput.Dispose();
            }
        }
    }
}
