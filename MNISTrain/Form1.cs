using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace MNISTrain
{
    public partial class Form1 : Form
    {
        private string pixelFile = "train-images";
        private string labelFile = "train-labels";
        private DigitImage[] trainImages = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int nextIndex = int.Parse(textBox2.Text);
            DigitImage currImage = trainImages[nextIndex];

            int mag = int.Parse(comboBox1.SelectedItem.ToString()); // magnification
            Bitmap bitMap = Visualisation.MakeBitmap(currImage, mag);
            pictureBox1.Image = bitMap;

            string pixelVals = Visualisation.PixelValues(currImage);
            textBox1.Text = pixelVals;

            textBox1.Text = textBox2.Text; // update curr idx from old next idz
            textBox2.Text = (nextIndex + 1).ToString(); // ++next index

            listBox1.Items.Add("Curr image index = " + textBox3.Text + " label = " + currImage.label);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    public class DigitImage
    {
        // an MNIST image of a '0' thru '9' digit
        public int width; // 28
        public int height; // 28
        public byte[][] pixels; // 0(white) - 255(black)
        public byte label; // '0' - '9'

        public DigitImage(int width, int height, byte[][] pixels, byte label)
        {
            this.width = width; this.height = height;
            this.pixels = new byte[height][];
            for (int i = 0; i < this.pixels.Length; ++i)
                this.pixels[i] = new byte[width];

            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                    this.pixels[i][j] = pixels[i][j];

            this.label = label;
        }
    }
}
