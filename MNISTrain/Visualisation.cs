using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace MNISTrain
{
    /// <summary>
    /// Хранение изображения в пиксельной форме
    /// </summary>
    public class DigitImage
    {
        // an MNIST image of a '0' thru '9' digit
        public int width {get; protected set;} // 28
        public int height {get; protected set;} // 28
        public byte[][] pixels {get; protected set;} // 0(white) - 255(black)
        public byte label { get; protected set; } // '0' - '9'

        /// <summary>
        /// Конструктор - сеттер полей класса хранения изображения
        /// <param name="pixels"> Матрица шестнадцатеричных значений пикселов изображения </param>
        /// <param name="label"> Хз </param>
        /// </summary>
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

    /// <summary>
    /// Получение и визуализация изображений из бд MNIST
    /// </summary>
    public class Visualisation
    {
        private int ReverseBytes(int v)
        {
            byte[] intAsBytes = BitConverter.GetBytes(v);
            Array.Reverse(intAsBytes);
            return BitConverter.ToInt32(intAsBytes, 0);
        }

        private string IntToBinaryString(int v)
        {
            // to pretty print an int as binary
            string s = Convert.ToString(v, 2); // base 2 but without leading 0s
            string t = s.PadLeft(32, '0'); // add leading 0s
            string res = "";
            for (int i = 0; i < t.Length; ++i)
            {
                if (i > 0 && i % 8 == 0)
                    res += " "; // add a space every 8 chars
                res += t[i];
            }
            return res;
        }

        /// <summary>
        /// Конструктор - сеттер полей класса хранения изображения
        /// <param name="labelFile"> Хз </param>
        /// <param name="pixelFile"> Изображения, заданные шестнадцатеричными пикселями </param>
        /// </summary>
        public DigitImage[] LoadData(string pixelFile, string labelFile)
        {
            // Load MNIST training set of 60,000 images into memory
            // remove static to access listBox1
            int numImages = 60000;
            DigitImage[] result = new DigitImage[numImages];

            byte[][] pixels = new byte[28][];
            for (int i = 0; i < pixels.Length; ++i)
                pixels[i] = new byte[28];

            FileStream ifsPixels = new FileStream(pixelFile, FileMode.Open);
            FileStream ifsLabels = new FileStream(labelFile, FileMode.Open);

            BinaryReader brImages = new BinaryReader(ifsPixels);
            BinaryReader brLabels = new BinaryReader(ifsLabels);

            int magic1 = brImages.ReadInt32(); // stored as Big Endian
            magic1 = ReverseBytes(magic1); // convert to Intel format

            int imageCount = brImages.ReadInt32();
            imageCount = ReverseBytes(imageCount);

            int numRows = brImages.ReadInt32();
            numRows = ReverseBytes(numRows);
            int numCols = brImages.ReadInt32();
            numCols = ReverseBytes(numCols);

            int magic2 = brLabels.ReadInt32();
            magic2 = ReverseBytes(magic2);

            int numLabels = brLabels.ReadInt32();
            numLabels = ReverseBytes(numLabels);

            // each image
            for (int di = 0; di < numImages; ++di)
            {
                for (int i = 0; i < 28; ++i) // get 28x28 pixel values
                {
                    for (int j = 0; j < 28; ++j)
                    {
                        byte b = brImages.ReadByte();
                        pixels[i][j] = b;
                    }
                }

                byte lbl = brLabels.ReadByte(); // get the label
                DigitImage dImage = new DigitImage(28, 28, pixels, lbl);
                result[di] = dImage;
            } // each image

            ifsPixels.Close(); brImages.Close();
            ifsLabels.Close(); brLabels.Close();

            return result;
        } // LoadData

        /// <summary>
        /// Конвертирование изображения в набор битов с увеличением
        /// <param name="dImage"> Изображение </param>
        /// <param name="mag"> Коэффициент увеличения изображения </param>
        /// </summary>
        public Bitmap MakeBitmap(DigitImage dImage, int mag)
        {
            // create a C# Bitmap suitable for display in a PictureBox control
            int width = dImage.width * mag;
            int height = dImage.height * mag;
            Bitmap result = new Bitmap(width, height);
            Graphics gr = Graphics.FromImage(result);
            for (int i = 0; i < dImage.height; ++i)
            {
                for (int j = 0; j < dImage.width; ++j)
                {
                    int pixelColor = 255 - dImage.pixels[i][j]; // white background, black digits
                    Color c = Color.FromArgb(pixelColor, pixelColor, pixelColor); // gray scale
                    SolidBrush sb = new SolidBrush(c);
                    gr.FillRectangle(sb, j * mag, i * mag, mag, mag); // fills bitmap via Graphics object
                }
            }
            return result;
        }

        /// <summary>
        /// Вывод изображения в виде набора пикселов, заданных шестнадцатеричными числами
        /// <param name="dImage"> Изображение </param>
        /// </summary>
        public string PixelValues(DigitImage dImage)
        {
            // create a string, with embedded newlines, suitable 
            // for display in a multi-line TextBox control
            string s = "";
            for (int i = 0; i < dImage.height; ++i)
            {
                for (int j = 0; j < dImage.width; ++j)
                {
                    s += dImage.pixels[i][j].ToString("X2") + " ";
                }
                s += Environment.NewLine;
            }
            return s;
        }
    }
}
