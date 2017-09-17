using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace MNISTrain
{
    public partial class Form1 : Form
    {
        private Visualisation VL = new Visualisation();
        /// <summary>
        /// Имена файлов бд MNIST в ресурсах
        /// </summary>
        private string pixelFile = "train-images.idx3-ubyte";
        private string labelFile = "train-labels.idx1-ubyte";

        /// <summary>
        /// Массив изображений из бд
        /// </summary>
        private DigitImage[] trainImages = null;

        /// <summary>
        /// Автоматическая смена изображжений
        /// <param name="checkBox"> Значение чекбокса (включение/выключение автосмены изображений) </param>
        /// </summary>
        private void AutoChange(object sender, EventArgs e)
        {
            Timer tmrShow = new Timer { Interval = 1000 };

            if (checkBox1.Checked)
                tmrShow.Enabled = true;

            tmrShow.Tick += delegate
            {
                ChangeImage(sender, e);

                if (!checkBox1.Checked)
                    tmrShow.Enabled = false;
            };
        }

        /// <summary>
        /// Смена изображение на следующее (связано с полями индексов изображений)
        /// </summary>
        private void ChangeImage(object sender, EventArgs e)
        {
            // Получение индекса следующего изображения
            int nextIndex = (int)numericUpDown2.Value;
            DigitImage currImage = trainImages[nextIndex];

            // Получение коэффициента увеличения картинки
            int mag = int.Parse(comboBox1.SelectedItem.ToString());

            // Построение изображения попиксельно
            Bitmap bitMap = VL.MakeBitmap(currImage, mag);
            pictureBox1.Image = bitMap;

            // Вывод значений пикселов изоражений в текстовое поле
            string pixelVals = VL.PixelValues(currImage);
            textBox3.Text = pixelVals;

            // Вывод новых индексов изображений
            numericUpDown1.Value = numericUpDown2.Value;
            numericUpDown2.Value = nextIndex + 1;

            listBox1.Items.Add("Индекс текущего изображения = " + numericUpDown1.Value + " label = " + currImage.label);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeImage(sender, e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Загрузка изображений в память
            this.trainImages = VL.LoadData(pixelFile, labelFile);
            listBox1.Items.Add("Изображения загружены в память");
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            AutoChange(sender, e);
        }
    }
}
