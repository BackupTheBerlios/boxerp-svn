using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Boxerp.Client;
using Boxerp.Client.WindowsForms;

namespace mvcSample1WinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Program.Start();
        }

        public static void Start()
        {
            Form1 usersListView = new Form1();
            UsersListController controller = new UsersListController(
                new WinFormsResponsiveHelper(ConcurrencyMode.Modal), usersListView);
            controller.RetrieveGroups();
            Application.Run(usersListView);
        }
    }
}