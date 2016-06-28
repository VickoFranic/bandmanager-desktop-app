using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BandManagerProject
{
    public partial class Gallery : Form
    {
        public Gallery(List<string> photosList)
        {
            InitializeComponent();

            foreach(string url in photosList)
            {
                PictureBox pic = new PictureBox();
                pic.Load(url);
            }
        }
    }
}
