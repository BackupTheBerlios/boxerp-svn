using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{ 
	/// <summary>
	/// The GUI Views can't extend any Boxerp class as they have to inherit a form or control and they have to 
	/// implement the View Interface but the Test View can extend this class having less code to write.
	/// Extend this class in your TestViews
	/// </summary>
	/// <param name="C">The Controller</param>
	/// <param name="TFinalDataIface">TestData class</param>
	/// <param name="TBaseDataIface">SharedData Interface</param>
	public abstract class AbstractTestView<C, TFinalDataIface, TBaseDataIface> : IView<C, TBaseDataIface>
		where C : AbstractController
		where TBaseDataIface : IUIData
		where TFinalDataIface : TBaseDataIface
	{
		protected TFinalDataIface _data ;
		private C _controller;

		protected abstract void CreateData();
		
		TBaseDataIface IView<C, TBaseDataIface>.SharedData
		{
			get
			{
				return _data;
			}
		}

		public TFinalDataIface Data
		{
			get
			{
				return _data;
			}
			protected set
			{
				_data = value;
			}
		}

		public C Controller
		{
			get
			{
				return _controller;
			}
			set
			{
				_controller = value;
			}
		}
	}
	
	/// <summary>
	/// Extend this class in your TestViews when there is no SharedData involved, just View and Controller
	/// </summary>
	/// <param name="C">The Controller</param>
	public class AbstractTestView<C> : IView<C>
		where C : AbstractController
	{
		private C _controller;

		public C Controller
		{
			get
			{
				return _controller;
			}
			set
			{
				_controller = value;
			}
		}
	}
	
    	
}
