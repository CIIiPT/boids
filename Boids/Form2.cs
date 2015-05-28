using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Boids
{
    public partial class Form2 : Form
    {
        
        public int count = 0;
        public int count1 = 0;
        public Form2()
        {

            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            count = Convert.ToInt32(textBox1.Text);
            count1 = Convert.ToInt32(textBox2.Text);
            Form1 form = new Form1(count,count1);
            form.ShowDialog();
            Close();
            
        }
    }
}
