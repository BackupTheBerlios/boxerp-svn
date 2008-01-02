using System;
using System.Collections.Generic;
using System.Text;

namespace mvcSample1WinForms
{
    public class WinFormsSharedData : AbstractUsersListData<Form1>
    {
        public WinFormsSharedData(Form1 view)
            : base(view)
        { }
    }
}
