using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApp2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public string uname;
        private async void button2_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string singer = textBox2.Text;
            if (name == "" || singer == "")
            {
                MessageBox.Show("Please input Title and Author");
                return;
            }
            string path = textBox3.Text;
            if (path == "")
            {
                MessageBox.Show("Please select the file");
                return;
            }
            bool result = await ClientAPI.ClientAPI.UploadMusic(path, name, singer,uname);
            MessageBox.Show(result ? "Success" : "Failed");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dia = new OpenFileDialog();
            string path;
            if (dia.ShowDialog() == DialogResult.OK)
            {
                path = dia.FileName;
            }
            else
                return;
            textBox3.Text = path;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
