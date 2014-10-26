using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Svg;
using Svg.Transforms;

namespace BatchSVG
{
    public partial class Form1 : Form
    {
        List<string> svgFileList;

        public Form1()
        {
            InitializeComponent();
        }

        public void GetSvgFiles(string path)
        {
            foreach (string dir in Directory.GetDirectories(path))
            {
                GetSvgFiles(dir);
            }

            foreach (string file in Directory.GetFiles(path, "*.svg"))
            {
                svgFileList.Add(file);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;
                textBox1.Text = path;

                if (string.IsNullOrEmpty(textBox2.Text))
                {
                    string outPath = Path.Combine(path, "converted");
                    textBox2.Text = outPath;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            svgFileList = new List<string>();
            string source = textBox1.Text;
            string output = textBox2.Text;

            if (!Directory.Exists(output))
                Directory.CreateDirectory(output);

            GetSvgFiles(source);
            int index = 0;

            foreach (string sf in svgFileList)
            {
                string fileName = Path.GetFileNameWithoutExtension(sf);
                using (Bitmap image = new Bitmap((int)numericUpDown1.Value, (int)numericUpDown2.Value))
                {
                    label8.Text = string.Format("Processing image {0} | {1}%", Path.GetFileName(sf), Math.Ceiling(((double)((double)index / (double)svgFileList.Count) * 100)));
                    this.Refresh();

                    SvgDocument doc = SvgDocument.Open(sf);
                    doc.Width = image.Width;
                    doc.Height = image.Height;
                    doc.Draw(image);

                    string of = Path.Combine(output, fileName);

                    if (comboBox1.Text == "PNG")
                        image.Save(of + ".png", ImageFormat.Png);
                    else if (comboBox1.Text == "JPEG")
                        image.Save(of + ".jpg", ImageFormat.Jpeg);
                    else if (comboBox1.Text == "GIF")
                        image.Save(of + ".gif", ImageFormat.Gif);
                    else if (comboBox1.Text == "TIFF")
                        image.Save(of + ".tif", ImageFormat.Tiff);
                    else if (comboBox1.Text == "BMP")
                        image.Save(of + ".bmp", ImageFormat.Bmp);
                    else
                        image.Save(of + ".png", ImageFormat.Png);

                    index++;
                }
            }

            label8.Text = "Finished";
        }
    }
}
