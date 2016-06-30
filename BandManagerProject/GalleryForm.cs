using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BandManagerProject.lib.models;
using BandManagerProject.lib.services;

namespace BandManagerProject
{
    public partial class GalleryForm : Form
    {
        List<Picture> _photosList;
        User _user;
        private FacebookService Facebook = new FacebookService();

        public GalleryForm(List<Picture> photosList, User user)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            _photosList = photosList;
            _user = user;

            loadImages();
        }

        private void loadImages()
        {
            foreach (Picture photo in _photosList)
            {
                string[] item = new string[2];
                item[0] = photo.id;
                item[1] = photo.created_ts;

                ListViewItem lvi = new ListViewItem(item);
                lvi.Tag = photo;

                listView1.Items.Add(lvi);
            }
        }

        /**
         * Fetch full picture url from Facebook API and show it in picture box
         */
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            Picture pic = item.Tag as Picture;

            string url = Facebook.getPictureUrl(pic.id, _user.access_token);
            pictureBox1.Load(url);
        }

    }
}
