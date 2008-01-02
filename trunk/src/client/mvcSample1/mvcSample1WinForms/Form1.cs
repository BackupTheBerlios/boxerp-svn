using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Boxerp.Client;
using Boxerp.Client.WindowsForms;

namespace mvcSample1WinForms
{
    public partial class Form1 : Form, IUsersListView
    {
        UsersListController _controller;
        WinFormsSharedData _sharedData;

        public Form1()
        {
            InitializeComponent();
            _sharedData = new WinFormsSharedData(this);
        }

        public int ActiveUsers
        {
            set
            {
                _activeUsers.Text = value.ToString();
            }
        }

        private void OnAdd(object sender, EventArgs e)
        {
            OnAddUser();
        }

        private void OnEdit(object sender, EventArgs e)
        {
            OnEditUser();
        }

        private void OnDelete(object sender, EventArgs e)
        {
            OnDeleteUser();
        }

        #region IUsersListView Members

        public void OnSelectionChanged(object sender, EventArgs e)
        {
            OnSelectionChanged();
        }

        public void OnDeleteUser()
        {
            Controller.DeleteUser(_users.SelectedItem as User);
        }

        public void OnEditUser()
        {
            Controller.EditUser(_users.SelectedItem as User);
        }

        public void OnAddUser()
        {
            Controller.AddUser(new User(), _groups.SelectedItem as Group);
        }

        public void UpdateGroups()
        {
            _groups.Items.Clear();
            foreach (Group group in SharedData.Groups)
            {
                _groups.Items.Add(group);
            }
        }

        public void UpdateUsers()
        {
            _users.Items.Clear();
            foreach (User user in SharedData.Users)
            {
                _users.Items.Add(user);
            }
        }

        public IUserEditView GetUserEditView()
        {
            Form2 userEditView = new Form2();
            return userEditView;
        }

        #endregion

        #region IView<UsersListController,IUsersListData> Members

        public IUsersListData SharedData
        {
            get 
            {
                return _sharedData;
            }
        }

        #endregion

        #region IView<UsersListController> Members

        public UsersListController Controller
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

        public void OnSelectionChanged()
        {
            Group selectedGroup = _groups.SelectedItem as Group;
            Controller.OnGroupSelectionChanged(selectedGroup);
        }
    }
}