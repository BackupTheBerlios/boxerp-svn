using System;
using System.Collections.Generic;
using System.Text;

using Boxerp.Client;

namespace Boxerp.Client.WPF
{
	/// <summary>
	/// Defines the interface that every wait control has to have in a Windows Presentation Foundation environment
	/// </summary>
	public interface IWpfWaitControl : IWaitControl
	{
		System.Windows.Threading.Dispatcher Dispatcher { get; }
	}
}
