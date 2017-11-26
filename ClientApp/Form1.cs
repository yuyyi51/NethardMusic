using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworkAPI;
using ClientAPI;

namespace ClientApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            bool result = await ClientAPI.ClientAPI.SignUp(username,password);
            label1.Text = result.ToString();
            //MessageBox.Show("finished");
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            bool result = await ClientAPI.ClientAPI.SignIn(username, password);
            label1.Text = result.ToString();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dia = new OpenFileDialog();
            string path;
            if (dia.ShowDialog() == DialogResult.OK)
            {
                path = dia.FileName;
            }
            else
                return;
            await ClientAPI.ClientAPI.CheckMD5(path);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            string name = textBox3.Text;
            string singer = textBox4.Text;
            OpenFileDialog dia = new OpenFileDialog();
            string path;
            if (dia.ShowDialog() == DialogResult.OK)
            {
                path = dia.FileName;
            }
            else
                return;
            await ClientAPI.ClientAPI.UploadMusic(path, name, singer);
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            MusicInfo[] infos = await ClientAPI.ClientAPI.GetMusicList();
            foreach(MusicInfo info in infos)
            {
                ListViewItem item = new ListViewItem();
                item.Text = info.name;
                item.SubItems.Add(info.singer);
                item.SubItems.Add(info.url);
                listView1.Items.Add(item);
            }
        }
    }
}
