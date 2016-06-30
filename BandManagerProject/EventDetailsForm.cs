using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BandManagerProject.lib.models;

namespace BandManagerProject
{
    public partial class EventDetailsForm : Form
    {
        Event _event;

        public EventDetailsForm(Event ev)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            _event = ev;

            setEventData();
        }

        private void setEventData()
        {
            label2.Text = _event.description;
            label4.Text = _event.name;
            label6.Text = _event.start_time;
        }
    }
}
