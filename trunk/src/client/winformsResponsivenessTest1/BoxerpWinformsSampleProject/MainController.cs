using System;
using System.Collections.Generic;
using System.Text;
using Boxerp.Client;
using Boxerp.Client.WindowsForms;
using System.Net;

namespace BoxerpWinformsSampleProject
{
    public class MainController : AbstractController
    {

        //http://download.microsoft.com/vwdsetup.exe
        private IMain _control;

        protected override void  OnAsyncOperationFinish(object sender, ThreadEventArgs args)
        {
			_control.ShowMessage("OK!!!");
        }
    
        public MainController(IResponsiveClient helper, IMain control)
	    : base(helper)
       {
            _control = control;
       }

        public void DoLongFileDownload()
        {
            ResponsiveHelper.StartAsyncCall(doLongFileDownload);
        }

        private void doLongFileDownload()
        {
            try
            {
                //yeah i know there is a download file async  ;)
                //WebClient client = new WebClient();
                //client.DownloadFile(_control.File, "C:\test.exe");
				System.Threading.Thread.Sleep(1000);
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                ResponsiveHelper.OnAbortAsyncCall(ex);
            }
            catch (Exception ex)
            {
                ResponsiveHelper.OnAsyncException(ex);
            }
            finally
            {
                StopAsyncCall();
            }
        }
    }
}
