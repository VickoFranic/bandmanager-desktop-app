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
    // Get all users from API
    //List<User> users = BandManagerService.getAllUsers();
    //User loggedInUser = users.Find(item => item.facebook_id == userFacebookID);

    public partial class UserProfileForm : Form
    {
        private User _user;
        private FacebookService Facebook = new FacebookService();

        public UserProfileForm(User user)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            _user = user;
            pictureBox1.Load(_user.picture);
            label4.Text = _user.facebook_id;
            label3.Text = _user.name;

            createUserPagesPanel();
        }

        public void createUserPagesPanel()
        {
            List<Page> pages = BandManagerService.getUserPages(_user);

            if (pages.Count != 0)
            {
                foreach (Page page in pages)
                {
                    string[] item = new string[2];
                    item[0] = page.name;
                    item[1] = page.genre;

                    var lvi = new ListViewItem(item);
                    lvi.Tag = page;
                    listView1.Items.Add(lvi);
                }
            }
        }

        /**
         * Get band details from API and show them in band profile form
         */
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            if (item != null)
            {
                Page page = item.Tag as Page;

                BandProfileForm bpf = new BandProfileForm(page, _user);

                bpf.ShowDialog();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EditUserForm euf = new EditUserForm(_user);

            euf.ShowDialog();

            if (euf.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string name = euf.getNewName();
                _user.name = name;
                label3.Text = name;
            }

        }
    }
}
