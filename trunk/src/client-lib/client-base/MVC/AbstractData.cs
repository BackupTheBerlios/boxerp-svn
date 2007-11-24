using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Boxerp.Client
{
	public abstract class AbstractData<TBaseView, C, TFinalView> : IUIData
		where TBaseView : IView
		where C : AbstractController
		where TFinalView : TBaseView
	{
		private TFinalView _view;
		private Hashtable _propertyBag = new Hashtable();

		public TFinalView View
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
