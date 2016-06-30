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
    public partial class EditUserForm : Form
    {
        User _user;
        string _newName;

        public EditUserForm(User user)
        {
            InitializeComponent();
            
            _user = user;

            setUserData();
        }

        void setUserData()
        {
            pictureBox1.Load(_user.picture);
            textBox1.Text = _user.name;
        }

        public string getNewName()
        {
            return _newName;
        }

        /**
         * Call BandManager API and save new username to database
         */
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string newName = textBox1.Text;

                if (String.IsNullOrEmpty(newName))
                    throw new Exception("Your username can`t be empty");

                string response = BandManagerService.updateUserName(newName, _user.facebook_id);
                MessageBox.Show(response);

                _newName = newName;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

    }
}
