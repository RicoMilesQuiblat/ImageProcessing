using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        Bitmap image, image2, processedImage;
        Device selectedDevice;

        public Form1()
        {
            InitializeComponent();
           
        }
        
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(image.Width, image.Height);
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    processedImage.SetPixel(i, j, image.GetPixel(i, j));
                }
            }
            pictureBox3.Image = processedImage;
        }

        private void greyScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(image.Width, image.Height);
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color color = image.GetPixel(i, j);
                    int gray = (color.R + color.G + color.B) / 3;
                    processedImage.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            pictureBox3.Image = processedImage;

        }

        private void inverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(image.Width, image.Height);
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color color = image.GetPixel(i, j);
                    processedImage.SetPixel(i, j, Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B));
                }
            }
            pictureBox3.Image = processedImage;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(image.Width, image.Height);

            for (int i = 0; i < processedImage.Width; i++)
            {
                for (int j = 0; j < processedImage.Height; j++)
                {
                    Color changeColor = image.GetPixel(i, j);
                    int gray = (int)(changeColor.R + changeColor.G + changeColor.B) / 3;
                    processedImage.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }

            int[] histogramData = new int[256];
            for (int i = 0; i < processedImage.Width; i++)
            {
                for (int j = 0; j < processedImage.Height; j++)
                {
                    Color greyscaleData = processedImage.GetPixel(i, j);
                    histogramData[greyscaleData.R]++;
                }
            }

            Bitmap histogramGraph = new Bitmap(256, 800);
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 800; j++)
                {
                    histogramGraph.SetPixel(i, j, Color.White);
                }
            }

            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < Math.Min(histogramData[i] / 5, 800); j++)
                {
                    histogramGraph.SetPixel(i, j, Color.Black);
                }
            }

            pictureBox3.Image = histogramGraph;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(image.Width, image.Height);

            for (int i = 0; i < processedImage.Width; i++)
            {
                for (int j = 0; j < processedImage.Height; j++)
                {
                    Color changeColor = image.GetPixel(i, j);
                    int red = (int)(changeColor.R);
                    int green = (int)(changeColor.G);
                    int blue = (int)(changeColor.B);
                    int sepiaRed = (int)((0.393 * red) + (0.769 * green) + (0.189 * blue));
                    int sepiaGreen = (int)((0.349 * red) + (0.686 * green) + (0.168 * blue));
                    int sepiaBlue = (int)((0.272 * red) + (0.534 * green) + (0.131 * blue));

                    if (sepiaRed > 255)
                    {
                        red = 255;
                    }
                    else
                    {
                        red = sepiaRed;
                    }

                    if (sepiaGreen > 255)
                    {
                        green = 255;
                    }
                    else
                    {
                        green = sepiaGreen;
                    }

                    if (sepiaBlue > 255)
                    {
                        blue = 255;
                    }
                    else
                    {
                        blue = sepiaBlue;
                    }

                    processedImage.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }
            }

            pictureBox3.Image = processedImage;
            image = processedImage;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox3.Image != null)
            {
                saveFileDialog1.ShowDialog();
            }
            else
            {
                MessageBox.Show("Picture Box 2 is empty, cannot save.", "Saving Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ImageFormat format = ImageFormat.Jpeg;
            pictureBox2.Image.Save(saveFileDialog1.FileName, format);
        }

        private void fToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            image2 = new Bitmap(openFileDialog1.FileName);
            pictureBox2.Image = image2;
        }

        private void originalPicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            image = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = image;
        }

        private void subtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(image2.Width, image.Height);
            Color mygreen = Color.FromArgb(0, 0, 255);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5;

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    Color backpixel = image2.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractvalue = Math.Abs(grey - greygreen);
                    if (subtractvalue < threshold)
                    {
                        processedImage.SetPixel(x, y, backpixel);
                    }
                    else
                    {
                        processedImage.SetPixel(x, y, pixel);
                    }
                }
            }
            pictureBox3.Image = processedImage;
        }

        private void webcamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Get all available devices
                Device[] allDevices = DeviceManager.GetAllDevices();

                if (allDevices.Length > 0)
                {
                    string deviceInfo = "Available Devices:\n\n";

                    for (int i = 0; i < allDevices.Length; i++)
                    {
                        deviceInfo += $"Device {i + 1}: {allDevices[i].Name} - Version: {allDevices[i].Version}\n";
                    }
                    MessageBox.Show(deviceInfo, "Available Devices");
                    // Assuming you want to use the first available device
                    Device firstDevice = allDevices[0];

                    // Start the webcam and display it in the PictureBox
                    firstDevice.ShowWindow(pictureBox1);
                }
                else
                {
                    MessageBox.Show("No webcam devices found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing webcam: {ex.Message}");
            }

        }
       
    }
    }
