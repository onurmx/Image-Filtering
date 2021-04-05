using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml.Serialization;
using System.IO;

namespace Image_Filtering
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Load Image";
            openFileDialog.Filter = "JPG Image File|*.jpg|JPEG Image File|*.jpeg|PNG Image File|*.png";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog.FileName;
                pictureBox2.ImageLocation = openFileDialog.FileName;
            }
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Result Image";
            saveFileDialog.Filter = "BITMAP Image File|*.bmp|JPEG Image File|*.jpeg";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!saveFileDialog.FileName.Equals(""))
                {
                    System.IO.FileStream fileStream = null;
                    try
                    {
                        using (fileStream = (System.IO.FileStream)saveFileDialog.OpenFile())
                        {
                            if (saveFileDialog.FilterIndex == 1)
                            {
                                pictureBox2.Image.Save(fileStream, ImageFormat.Bmp);
                            }
                            if (saveFileDialog.FilterIndex == 2)
                            {
                                pictureBox2.Image.Save(fileStream, ImageFormat.Jpeg);
                            }
                        }
                    }
                    finally
                    {
                        if (fileStream != null)
                        {
                            fileStream.Dispose();
                        }
                    }
                }
            }
        }

        private void usageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add Point - Left Click\nRemove Point - Right Click\nMove Point - Middle Click and Drag");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(".", "*.xml");
            foreach (var file in files)
            {
                listBox1.Items.Add(file.Split('\\').ElementAt(1).Split('.').ElementAt(0));
            }
            panel1.AutoScroll = true;
            panel2.AutoScroll = true;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
            comboBox1.Items.Add("2");
            comboBox1.Items.Add("3");
            comboBox1.Items.Add("4");
            comboBox1.Items.Add("6");
            comboBox1.SelectedIndex = 0;
            comboBox2.Items.Add("2");
            comboBox2.Items.Add("4");
            comboBox2.Items.Add("8");
            comboBox2.Items.Add("16");
            comboBox2.SelectedIndex = 0;
            textBox2.TextAlign = HorizontalAlignment.Center;
            textBox3.TextAlign = HorizontalAlignment.Center;
            textBox4.TextAlign = HorizontalAlignment.Center;
        }

        private void resetChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdditionalFunctionality.Points = new List<Point>() { new Point(0, 255), new Point(255, 0) };
            AdditionalFunctionality.FilterPoints = new List<int>();
            pictureBox2.Image = pictureBox1.Image;
            pictureBox3.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image = FunctionalFilters.Inversion(pictureBox2.Image);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image = FunctionalFilters.BrightnessCorrection(pictureBox2.Image);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image = FunctionalFilters.ContrastEnhancement(pictureBox2.Image);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image = FunctionalFilters.GammaCorrection(pictureBox2.Image);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image = ConvolutionFilters.Blur(pictureBox2.Image);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image = ConvolutionFilters.GaussianBlur(pictureBox2.Image);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image = ConvolutionFilters.Sharpen(pictureBox2.Image);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image = ConvolutionFilters.EdgeDetection(pictureBox2.Image);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image = ConvolutionFilters.Emboss(pictureBox2.Image);
            }
        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            for (int i = 0; i < AdditionalFunctionality.Points.Count - 1; i++)
            {
                g.DrawLine(Pens.Black, AdditionalFunctionality.Points[i], AdditionalFunctionality.Points[i + 1]);
                Rectangle rect = new Rectangle(AdditionalFunctionality.Points[i].X - 4, AdditionalFunctionality.Points[i].Y - 4, 4 * 2, 4 * 2);
                g.DrawEllipse(Pens.Black, rect);
                rect = new Rectangle(AdditionalFunctionality.Points[i + 1].X - 4, AdditionalFunctionality.Points[i + 1].Y - 4, 4 * 2, 4 * 2);
                g.DrawEllipse(Pens.Black, rect);
            }
        }

        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {
            if (!AdditionalFunctionality.Points.Any(p => p.X == e.X) && e.Button == MouseButtons.Left)
            {
                AdditionalFunctionality.Points.Add(new Point(e.X, e.Y));
                AdditionalFunctionality.Points = AdditionalFunctionality.Points.OrderBy(p => p.X).ToList();
            }
            if (AdditionalFunctionality.Points.Any(p => p.X == e.X) && e.Button == MouseButtons.Left)
            {
                AdditionalFunctionality.Points[AdditionalFunctionality.Points.FindIndex(p => p.X == e.X)] = new Point(e.X, e.Y);
            }
            if (AdditionalFunctionality.Points.Any(p => p.X == e.X) && e.Button == MouseButtons.Right && e.X != 0 && e.X != 255)
            {
                AdditionalFunctionality.Points.RemoveAt(AdditionalFunctionality.Points.FindIndex(p => p.X == e.X));
            }
            pictureBox3.Invalidate();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            AdditionalFunctionality.Points = new List<Point>() { new Point(0, 255), new Point(255, 0) };
            AdditionalFunctionality.FilterPoints = new List<int>();
            pictureBox2.Image = pictureBox1.Image;
            pictureBox3.Invalidate();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                AdditionalFunctionality.CalculateFilterPoints();
                pictureBox2.Image = AdditionalFunctionality.AdditionalFilter(pictureBox2.Image);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox1.Text);
            FileStream fs = new FileStream($@".\{textBox1.Text}.xml", FileMode.OpenOrCreate);
            XmlSerializer s = new XmlSerializer(typeof(List<Point>));
            s.Serialize(fs, AdditionalFunctionality.Points);
            textBox1.Clear();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                if (listBox1.SelectedItem != null)
                {
                    pictureBox2.Image = pictureBox1.Image;
                    FileStream fs = new FileStream($@"{listBox1.SelectedItem.ToString()}.xml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    XmlSerializer s = new XmlSerializer(typeof(List<Point>));
                    AdditionalFunctionality.Points = (List<Point>)s.Deserialize(fs);
                    AdditionalFunctionality.CalculateFilterPoints();
                    pictureBox2.Image = AdditionalFunctionality.AdditionalFilter(pictureBox2.Image);
                    pictureBox3.Invalidate();
                }
            }
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (AdditionalFunctionality.Points.Any(p => p.X == e.X) && e.Button == MouseButtons.Middle)
            {
                MouseMovementAnimation.Index = AdditionalFunctionality.Points.FindIndex(p => p.X == e.X);
                MouseMovementAnimation.OriginalX = AdditionalFunctionality.Points[MouseMovementAnimation.Index].X;
                if (MouseMovementAnimation.Index == 0 || MouseMovementAnimation.Index == (AdditionalFunctionality.Points.Count - 1))
                {
                    MouseMovementAnimation.VerticalEditable = true;
                }
                else
                {
                    MouseMovementAnimation.HorizontalEditable = true;
                    MouseMovementAnimation.VerticalEditable = true;
                }
            }
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                MouseMovementAnimation.HorizontalEditable = false;
                MouseMovementAnimation.VerticalEditable = false;
            }
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseMovementAnimation.HorizontalEditable && MouseMovementAnimation.VerticalEditable)
            {
                if ((AdditionalFunctionality.Points[MouseMovementAnimation.Index - 1].X < e.X && e.X < AdditionalFunctionality.Points[MouseMovementAnimation.Index + 1].X) && (0 <= e.Y && e.Y <= 255))
                {
                    AdditionalFunctionality.Points[MouseMovementAnimation.Index] = new Point(e.X, e.Y);
                    pictureBox3.Invalidate();
                }
            }
            if (MouseMovementAnimation.VerticalEditable && MouseMovementAnimation.HorizontalEditable == false)
            {
                if ((0 <= e.Y && e.Y <= 255))
                {
                    AdditionalFunctionality.Points[MouseMovementAnimation.Index] = new Point(MouseMovementAnimation.OriginalX, e.Y);
                    pictureBox3.Invalidate();
                }
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image = LaboratoryFilter.MedianFilter(pictureBox2.Image);
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image = OrderedDitheringFilter.GrayScale(pictureBox2.Image);
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                OrderedDitheringFilter.MatrixType = Int32.Parse(comboBox1.SelectedItem.ToString());
                OrderedDitheringFilter.GrayLevel = Int32.Parse(comboBox2.SelectedItem.ToString());
                pictureBox2.Image = OrderedDitheringFilter.OrderedDithering(pictureBox2.Image);
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            QuantizationFilter.DivisorR = QuantizationFilter.DivisorG = QuantizationFilter.DivisorB = 0;
            if (pictureBox2.Image != null &&
                (textBox2.TextLength > 0 && (QuantizationFilter.DivisorR = Int32.Parse(textBox2.Text.ToString())) >= 1) &&
                (textBox3.TextLength > 0 && (QuantizationFilter.DivisorG = Int32.Parse(textBox3.Text.ToString())) >= 1) &&
                (textBox4.TextLength > 0 && (QuantizationFilter.DivisorB = Int32.Parse(textBox4.Text.ToString())) >= 1))
            {
                pictureBox2.Image = QuantizationFilter.UniformQuantization(pictureBox2.Image);
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image = LaboratoryFilter2.ConvertToYCbCr(pictureBox2.Image);
            }
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image = LaboratoryFilter2.ConvertToRGB(pictureBox2.Image);
            }
        }
    }
}
