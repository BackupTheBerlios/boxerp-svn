using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

namespace Boxerp.Client
{
	/// <summary>
	/// Boxerp internal interface
	/// </summary>
	public interface ICustomNotifyPropertyChanged :  INotifyPropertyChanged
	{
		bool HasSubscribers();
		void ThrowPropertyChangedEvent(string propertyName);
		Delegate[] GetSubscribersList();
	}
}
