using System;
using System.Collections.Generic;
using System.Text;

namespace BoxerpWinformsSampleProject
{
    public interface IMain
    {
        Uri File { get; }
        void DoLongFileDownload();
    }
}
