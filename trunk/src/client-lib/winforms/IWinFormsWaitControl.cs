using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Boxerp.Client;

namespace Boxerp.Client.WindowsForms
{
	/// <summary>
	/// Defines the interface that every wait control has to have in a Windows.Forms environment
	/// </summary>
	public interface IWinFormsWaitControl : IWaitControl
	{
		object Invoke(Delegate method, params object[] args);
		object Invoke(Delegate method);
		IAsyncResult BeginInvoke(Delegate method, params object[] args);
	}
}
