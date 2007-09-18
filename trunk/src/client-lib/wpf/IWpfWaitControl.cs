using System;
using System.Collections.Generic;
using System.Text;

using Boxerp.Client;

namespace Boxerp.Client.WPF
{
	public interface IWpfWaitControl : IWaitControl
	{
		System.Windows.Threading.Dispatcher Dispatcher { get; }
	}
}
