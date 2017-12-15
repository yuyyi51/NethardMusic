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

namespace ClientApp2
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
            if (username == "" || password == "")
            {
                MessageBox.Show("请输入用户名和密码");
                return;
            }

            bool result = await ClientAPI.ClientAPI.SignUp(username, password);
            //label1.Text = result.ToString();
            MessageBox.Show(result?"注册成功":"注册失败");
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            if (username == "" || password == "")
            {
                MessageBox.Show("请输入用户名和密码");
                return;
            }
            bool result = await ClientAPI.ClientAPI.SignIn(username, password);
            //label1.Text = result.ToString();
            MessageBox.Show(result ? "登录成功" : "登录失败");
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string path = textBox5.Text;
            if (path == "")
            {
                MessageBox.Show("请先选择文件");
                return;
            }
            bool result = await ClientAPI.ClientAPI.CheckMD5(path);
            MessageBox.Show(result ? "没有相同文件" : "已有相同文件");
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            string name = textBox3.Text;
            string singer = textBox4.Text;
            if(name == "" || singer == "")
            {
                MessageBox.Show("请输入歌名和歌手");
                return;
            }
            string path = textBox5.Text;
            if (path == "")
            {
                MessageBox.Show("请先选择文件");
                return;
            }
            button4.Enabled = false;
            button4.Text = "上传中";
            bool result = await ClientAPI.ClientAPI.UploadMusic(path, name, singer);
            button4.Text = "上传";
            button4.Enabled = true;
            MessageBox.Show(result ? "上传成功" : "上传失败");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog dia = new OpenFileDialog();
            string path;
            if (dia.ShowDialog() == DialogResult.OK)
            {
                path = dia.FileName;
            }
            else
                return;
            textBox5.Text = path;
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            MusicInfo[] infos = await ClientAPI.ClientAPI.GetMusicList();
            foreach (MusicInfo info in infos)
            {
                ListViewItem item = new ListViewItem();
                item.Text = info.name;
                item.SubItems.Add(info.singer);
                item.SubItems.Add(info.url);
                listView1.Items.Add(item);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
                return;
            ListViewItem item = listView1.SelectedItems[0];
            string url = item.SubItems[2].Text;
            //MessageBox.Show(url);
            axWindowsMediaPlayer1.URL = url;
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = 20;
        }
    }
}
