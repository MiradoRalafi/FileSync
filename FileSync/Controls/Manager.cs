using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileSync.ManagerObjects;

namespace FileSync
{
    public partial class Manager : UserControl
    {
        public Form form { get; set; }
        public Manager()
        {
            InitializeComponent();
        }

        private void btn_sync_Click(object sender, EventArgs e)
        {

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            FileManager.StopWatcher();
            this.form.Controls.Find("start1", true)[0].BringToFront();
        }
    }
}
