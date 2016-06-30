using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Facebook;
using Newtonsoft.Json;
using BandManagerProject.lib.models;
using BandManagerProject.lib.services;

namespace BandManagerProject
{
    public partial class BandProfileForm : Form
    {
        Page _page;
        User _user;
        private FacebookService Facebook = new FacebookService();

        public BandProfileForm(Page page, User user)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            _page = page;
            _user = user;

            getPageProfileFromFacebook();
        }

        /**
         * Call Facebook API and get page profile details
         */
        private void getPageProfileFromFacebook()
        {
            JsonObject response = (JsonObject)Facebook.getPageProfile(_page, _user.access_token);

            if (! String.IsNullOrEmpty(response.ToString()))
            {
                try
                {
                    string about = response["about"].ToString();
                    string bio = response["bio"].ToString();
                    string members = response["band_members"].ToString();
                    string hometown = response["hometown"].ToString();
                    string url = response["link"].ToString();

                    label5.Text = _page.name;
                    label6.Text = _page.genre;
                    label7.Text = hometown;
                    label9.Text = members;
                    label3.Text = about;
                    linkLabel1.Tag = url;
                    pictureBox2.Load(_page.picture);
                }
                catch (Exception)
                {
                    // Missing some field for Facebook page, ignore
                }
            }

        }

        /**
         * Open page link in default browser
         */
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Tag.ToString());
        }

        /**
         * Show all events for band
         */
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                JsonObject response = (JsonObject)Facebook.getPageEvents(_page, _user.access_token);
                dynamic data = JsonConvert.DeserializeObject(response.ToString());
                dynamic events = data.events["data"];

                foreach (var ev in events)
                {
                    Event eventItem = new Event();

                    eventItem.description = ev.description;
                    eventItem.name = ev.name;
                    eventItem.start_time = ev.start_time;

                    string[] item = new string[2];
                    item[0] = eventItem.name;
                    item[1] = eventItem.start_time;

                    var lvi = new ListViewItem(item);
                    lvi.Tag = eventItem;
                    listView1.Items.Add(lvi);
                }

                listView1.Visible = true;
            }
            catch(Exception)
            {
               // Some data missing for Facebook event, ignore
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            JsonObject response = (JsonObject)Facebook.getPagePhotos(_page, _user.access_token);
            dynamic data = JsonConvert.DeserializeObject(response.ToString());
            dynamic photos = data.photos["data"];

            List<Picture> photosList = new List<Picture>();

            foreach (var photo in photos)
            {
                Picture pic = new Picture();

                pic.id = photo.id;
                pic.created_ts = photo.created_time;

                photosList.Add(pic);
            }

            GalleryForm gallery = new GalleryForm(photosList, _user);
            gallery.Show();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            Event ev = item.Tag as Event;

            EventDetailsForm edf = new EventDetailsForm(ev);
            edf.Show();
        }

    }
}
