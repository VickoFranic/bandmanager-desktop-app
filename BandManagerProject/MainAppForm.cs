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
using CefSharp.WinForms;
using CefSharp.WinForms.Internals;
using CefSharp;

namespace BandManagerProject
{
    public partial class MainAppForm : Form
    {
        FacebookService Facebook;
        ChromiumWebBrowser Browser;
        UserProfileForm userProfileForm;

        public MainAppForm()
        {
            InitializeComponent();
            Facebook = new FacebookService();

            Uri loginUrl = Facebook.generateLoginUrl();

            WindowState = FormWindowState.Maximized;
            Browser = new ChromiumWebBrowser(loginUrl.AbsoluteUri)
            {
                Dock = DockStyle.Fill,
            };

            this.panel1.Controls.Add(Browser);

            Browser.AddressChanged += OnBrowserAddressChanged;
        }


        /**
         * Show user profile form
         */
        private void showUserProfile(string accessToken)
        {
            User user = Facebook.getFacebookUser(accessToken);

            userProfileForm = new UserProfileForm(user);
            
     //       this.Visible = false;
            userProfileForm.ShowDialog();

            if (userProfileForm.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                this.Close();
            }
        }

        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args)
        {
            Uri callback = new Uri(args.Address);
            string accessToken = Facebook.getAccessTokenFromCallback(callback);

            if (accessToken != String.Empty)
            {
               this.InvokeOnUiThreadIfRequired(() => showUserProfile(accessToken));
            }
        }
    }
}