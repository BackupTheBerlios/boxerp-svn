using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Boxerp.Client.WindowsForms
{
    public partial class QuestionDialog : Form
    {
        protected readonly bool _isModal;
        private bool _answer;

        public QuestionDialog(bool isModal)
        {
            InitializeComponent();

            if (isModal)
            {
                this._isModal = true;
                this.Modal = true;
            }
            else
            {
                _isModal = false;
            }
        }

        private void QuestionDialog_Load(object sender, EventArgs e)
        {

        }

        public string Message
        {
            get
            {
                return MessageLbl.Text;
            }
            set
            {
                MessageLbl.Text = value;
            }
        }

        public bool IsModal
        {
            get
            {
                return _isModal;
            }
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            _answer = true;
            this.Hide();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            _answer = false;
            this.Hide();
        }

    }
}