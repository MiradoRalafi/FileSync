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
    public partial class Start : UserControl
    {
        public Form form { get; set; }
        public Start()
        {
            InitializeComponent();
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            try
            {
                // Starts FTP server
                FTPServer server = new FTPServer();
                server.Start();
                // Starts File watcher
                FileManager.Initialize();
                FileManager.StartWatcher();
                this.form.Controls.Find("manager1", true)[0].BringToFront();
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
