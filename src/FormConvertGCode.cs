using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class FormConvertGCode : Form
    {
        public FormConvertGCode()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "\\\\psf\\Home\\Downloads\\04191.gcode";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            ConvertGCodeMode1 mode1 = new ConvertGCodeMode1();
            mode1.XYThreshold = double.Parse(textBox2.Text);
            mode1.ZAdjust = double.Parse(textBox3.Text);
            mode1.EAdjust = double.Parse(textBox4.Text);
            mode1.EInfo = checkBox1.Checked;
            mode1.EInfoInterval = double.Parse(textBox5.Text);
            StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
            StreamWriter sw = new StreamWriter(textBox1.Text + "_new.gcode", false);
            StreamWriter swlog = new StreamWriter(textBox1.Text + ".log", false);
            mode1.ProcessStream(sr, sw, swlog);
            sr.Close();
            sw.Close();
            swlog.Close();

            button1.Enabled = true;
            MessageBox.Show("转换成功！");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            textBox1.Text = "";
            if (s.Length > 0) textBox1.Text = s[0];
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
               e.Effect = DragDropEffects.All;
           else
               e.Effect = DragDropEffects.None;
        }
    }
}
