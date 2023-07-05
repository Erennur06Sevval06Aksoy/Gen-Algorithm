using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gen_Algoritması
{
    public partial class Form2 : Form
    {
        static List<string> sequences = new List<string>();
        private int incident_count;
        public Form2(int count)
        {
            InitializeComponent();
            incident_count = count;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(Convert.ToString(incident_value));
            string incident_seq = textBox1.Text;
            string[] words = incident_seq.Split(' ');
            foreach (var word in words)
            {
                listBox1.Items.Add(word);
                sequences.Add(word);
            }
            if (listBox1.Items.Count >= incident_count)
            {
                button1.Enabled = false;
                textBox1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(sequences);
            form3.Show();
        }
    }
}
