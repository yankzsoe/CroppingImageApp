using System;
using System.Drawing;
using System.Windows.Forms;

namespace CropImage.CroppingImages
{
    public partial class frmMessageBox : Form
    {
        public Image img { get; set; }        
        public frmMessageBox(Image _img)
        {
            InitializeComponent();
                pictureBox1.Image = _img;           
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            CroppingImages.checkedEdited = false;
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
               DialogResult dr = MessageBox.Show("Are you sure?", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question,MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.OK)
                {
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Refresh();
                        img = pictureBox1.Image;
                        this.DialogResult = DialogResult.OK;
                        CroppingImages.checkedEdited = true;
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.Cancel;
                        CroppingImages.checkedEdited = false;
                    }
                }       
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmMessageBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
        }
    }
}
