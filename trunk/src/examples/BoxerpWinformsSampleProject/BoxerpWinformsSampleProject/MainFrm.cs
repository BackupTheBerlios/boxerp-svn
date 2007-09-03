using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Boxerp.Client.WindowsForms;
using Boxerp.Client;

namespace BoxerpWinformsSampleProject
{
    public partial class MainFrm : Form, IMain
    {

        private readonly MainController _controller;
        private Uri _file;

        public MainFrm()
        {
            _controller = new MainController(new WinFormsResponsiveHelper(ConcurrencyMode.Modal), this);
            InitializeComponent();
        }


        public MainController Controller
        {
            get
            {
                return _controller;
            }
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {

        }

        public Uri File
        {
            get
            {
                return new Uri(FileLocationTxt.Text);
            }
        }

        public void DoLongFileDownload()
        {
            _controller.DoLongFileDownload();
        }

        private void DownloadFileBtn_Click(object sender, EventArgs e)
        {
            _file = new Uri(FileLocationTxt.Text);
            DoLongFileDownload();
        }

		public void ShowMessage(string msg)
		{
			MessageBox.Show(msg);
		}
    }
}