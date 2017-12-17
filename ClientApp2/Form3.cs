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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        public Form1 father;
        private async void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            if (username == "" || password == "")
            {
                MessageBox.Show("Please input Username and Password");
                return;
            }
            bool result = await ClientAPI.ClientAPI.SignIn(username, password);
            //label1.Text = result.ToString();
            MessageBox.Show(result ? "Success" : "Failed");
            if(result)
            {
                father.username = username;
                Close();
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            if (username == "" || password == "")
            {
                MessageBox.Show("Please input Username and Password");
                return;
            }

            bool result = await ClientAPI.ClientAPI.SignUp(username, password);
            //label1.Text = result.ToString();
            MessageBox.Show(result ? "Success" : "Failed");
        }
    }
}
