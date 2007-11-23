using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Boxerp.Client
{
	public abstract class AbstractData<BaseView, C, FinalView> : IUIData
		where BaseView : IView
		where C : AbstractController
		where FinalView : IView<BaseView, C>
	{
		private FinalView _view;
		private Hashtable _propertyBag = new Hashtable();

		public FinalView View
		{
			get { return _view; }
			set { _view = value; }
		}

		public Hashtable PropertyBag
		{
			get 
			{
				return _propertyBag;
			}
		}
	}
}
