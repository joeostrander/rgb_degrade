using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace rgb_degrade
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select an image file to degrade";
            openFileDialog1.Filter = "Image Files|*.jpg;*.png;*.bmp|All Files|*.*";
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            label1.Text = openFileDialog1.FileName;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName;
            openFileDialog1.FileName = "";
            saveFileDialog1.FileName = "";
            label1.Text = "";
            comboBox1.SelectedIndex = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp";
            ImageFormat format = ImageFormat.Png;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(saveFileDialog1.FileName);
                switch (fi.Extension.ToUpper())
                {
                    case "BMP":
                        format = ImageFormat.Bmp;
                        break;
                    case "JPG":
                        format = ImageFormat.Jpeg;
                        break;
                    default:
                        break;
                }
                string filepath = fi.Directory.FullName + "\\" + Path.GetFileNameWithoutExtension(fi.Name) + fi.Extension;
                Console.WriteLine(filepath);
                pictureBox1.Image.Save(filepath);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(pictureBox1.Image);
        }

        private void button4_Click(object sender, EventArgs e)
        {


            if (!File.Exists(openFileDialog1.FileName))
                return;

            byte mask_red = 0xC0;
            byte mask_green = 0xC0;
            byte mask_blue = 0xC0;

            switch (comboBox1.Text)
            {

                case "RGB332":
                    mask_red = 0xFC;
                    mask_green = 0xFC;
                    break;
                case "RGB565":
                    mask_red = 0xF8;
                    mask_green = 0xFC;
                    mask_blue = 0xF8;
                    break;
                case "RGB666":
                    mask_red = 0xFC;
                    mask_green = 0xFC;
                    mask_blue = 0xFC;
                    break;
                case "RGB222":
                    break;
                default:
                    break;
            }

            string file = openFileDialog1.FileName;
            Bitmap original = (Bitmap)Image.FromFile(file);
            Bitmap bitmap = new Bitmap(original.Width, original.Height);

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color color = original.GetPixel(x, y);
                    //Color color_new = Color.FromArgb(color.R & mask_red, color.G & mask_green, color.B & mask_blue);
                    Color color_new = Color.FromArgb(
                        (int)((color.R & mask_red) | (~mask_red)) & 0xFF,
                        (int)((color.G & mask_green) | (~mask_green)) & 0xFF,
                        (int)((color.B & mask_blue) | (~mask_blue)) & 0xFF
                        );
                    bitmap.SetPixel(x, y, color_new);
                }
            }


            pictureBox1.Image = bitmap;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
