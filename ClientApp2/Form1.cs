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
        public string username = null;
        public Form1()
        {
            InitializeComponent();
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
                item.SubItems.Add(info.playedtimes.ToString());
                item.SubItems.Add(info.uname != null ? info.uname : "");
                item.SubItems.Add(info.url);
                listView1.Items.Add(item);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
                return;
            ListViewItem item = listView1.SelectedItems[0];
            string url = item.SubItems[4].Text;
            //MessageBox.Show(url);
            axWindowsMediaPlayer3.URL = url;
            axWindowsMediaPlayer3.Ctlcontrols.play();
            await ClientAPI.ClientAPI.MusicPlayed(url);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            axWindowsMediaPlayer3.settings.volume = 20;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form2 dia = new Form2();
            dia.uname = username;
            dia.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form3 dia = new Form3();
            dia.father = this;
            dia.ShowDialog();
            if(username != null)
            {
                label5.Text = username;
                button7.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                button7.Visible = true;
                button1.Visible = true;
                button2.Visible = true;
            }
        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            bool r = true;
            foreach(ListViewItem item in listView1.SelectedItems)
            {
                string url = item.SubItems[4].Text;
                bool re = await ClientAPI.ClientAPI.AddFavorite(username, url);
                r &= re;
            }
            if(r)
            {
                MessageBox.Show("Successed");
            }
            else
            {
                MessageBox.Show("Operation failed. Maybe you have already added some of them into favorite list.");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            MusicInfo[] infos = await ClientAPI.ClientAPI.GetFavoriteList(username);
            foreach (MusicInfo info in infos)
            {
                ListViewItem item = new ListViewItem();
                item.Text = info.name;
                item.SubItems.Add(info.singer);
                item.SubItems.Add(info.playedtimes.ToString());
                item.SubItems.Add(info.uname != null ? info.uname : "");
                item.SubItems.Add(info.url);
                listView1.Items.Add(item);
            }
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }
    }
}
