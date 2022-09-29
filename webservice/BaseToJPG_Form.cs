using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace webservice
{
    public partial class BaseToJPG_Form : Form
    {
        public BaseToJPG_Form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filetype = textBox2.Text.Trim();
            base64.output((Convert.FromBase64String(textBox1.Text.Trim())), "图片", filetype);
            MessageBox.Show("生成" + "图片报告成功", "提示");  
        }

        private void BaseToJPG_Form_Load(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            textBox1.Text = f1.str;
        }
    }
}
