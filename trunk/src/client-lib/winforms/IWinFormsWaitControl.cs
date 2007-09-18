using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Boxerp.Client;

namespace Boxerp.Client.WindowsForms
{
	public interface IWinFormsWaitControl : IWaitControl
	{
		object Invoke(Delegate method, params object[] args);
		object Invoke(Delegate method);
		IAsyncResult BeginInvoke(Delegate method, params object[] args);
	}
}
