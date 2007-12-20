//
// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
//
// Copyright (C) 2005,2006 Shidix Technologies (www.shidix.com)
//
// Redistribution and use in source and binary forms, with or
// without modification, are permitted provided that the following
// conditions are met:
// Redistributions of source code must retain the above
// copyright notice, this list of conditions and the following
// disclaimer.
// Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials
// provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.
//
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Boxerp.Client.WindowsForms
{
    public enum ResponseType
    {
        Ok,
        Cancel
    }

	/// <summary>
	/// Dialog with two buttons (Accept, Cancel). Although it would be possible to use a MessageBox,
	/// the ResponsiveHelper hierarchy need an instance of an IQuestionWindow for a generic implementation
	/// </summary>
    public partial class QuestionDialog : Form, IQuestionWindow
    {
        protected readonly bool _isModal;
		private string _afirmativeOption;
		private string _negativeOption;
        private ResponseType _answer;

		public QuestionDialog() 
			: this (true)
		{ }

        public QuestionDialog(bool isModal)
        {
            InitializeComponent();

            if (isModal)
            {
                this._isModal = true;
            }
            else
            {
                _isModal = false;
            }
        }

        private void QuestionDialog_Load(object sender, EventArgs e)
        {

        }

        public ResponseType Run()
        {
			BringToFront();
			base.ShowDialog();
            return _answer;
        }

        public string Msg
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
            _answer = ResponseType.Ok;
            this.Hide();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            _answer = ResponseType.Cancel;
            this.Hide();
        }

		#region IQuestionWindow Members


		public string AfirmativeOption
		{
			get
			{
				return _afirmativeOption;
			}
			set
			{
				_afirmativeOption = value;
				OkBtn.Text = value;
			}
		}

		public string NegativeOption
		{
			get
			{
				return _negativeOption;
			}
			set
			{
				_negativeOption = value;
				CancelBtn.Text = value;
			}
		}

		public new void ShowDialog()
		{
			Run();
		}

		public bool IsAfirmative
		{
			get
			{
				return _answer == ResponseType.Ok;
			}
			set
			{
				_answer = value ? ResponseType.Ok : ResponseType.Cancel;
			}
		}

		#endregion
	}
}