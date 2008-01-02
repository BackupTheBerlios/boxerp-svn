using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mvcSample1WinForms
{
    public partial class Form2 : Form, IUserEditView
    {
        UserEditController _controller;

        public Form2()
        {
            InitializeComponent();
        }

        #region IUserEditView Members

        public void DataBindUser(User user)
        {
            Console.Out.WriteLine("DataBindUser:" + user);
        }

        #endregion

        #region IView<UserEditController> Members

        public UserEditController Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                _controller = value;
            }
        }

        #endregion
    }
}