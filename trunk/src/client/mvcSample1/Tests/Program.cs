using System;
using System.Collections.Generic;
using System.Text;
using mvcSample1WinForms;
using Boxerp.Client;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Program
    {
        [Test]
        public void RetrieveUsers()
        {
            TestUsersListView usersListView = new TestUsersListView();
            UsersListController controller = new UsersListController(
                new ConsoleResponsiveHelper(ConcurrencyMode.Modal), usersListView);
            controller.RetrieveGroups();
            Assert.IsTrue(true, "No Exceptions");
        }

        [Test]
        public void SelectGroup()
        {
            TestUsersListView usersListView = new TestUsersListView();
            UsersListController controller = new UsersListController(
                new ConsoleResponsiveHelper(ConcurrencyMode.Modal), usersListView);
            controller.RetrieveGroups();
            usersListView.SelectedGroup = usersListView.SharedData.Groups[1];
            controller.OnGroupSelectionChanged(usersListView.SelectedGroup);
            Assert.IsTrue(true, "No Exceptions");
        }
    }
}
