using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

namespace Boxerp.Client
{
	public interface ICustomNotifyPropertyChanged :  INotifyPropertyChanged
	{
		bool HasSubscribers();
		void ThrowPropertyChangedEvent(string propertyName);
	}
}
