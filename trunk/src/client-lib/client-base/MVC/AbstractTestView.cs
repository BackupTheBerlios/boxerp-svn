using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{ 
	/// <summary>
	/// C = controller
	/// D = First IUIData interface
	/// Z = Concrete implementation of D
	/// I = IView interface
	/// </summary>
	/// <typeparam name="C"></typeparam>
	/// <typeparam name="D"></typeparam>
	/// <typeparam name="Z"></typeparam>
	/// <typeparam name="I"></typeparam>
	public class AbstractTestView<C, TFinalDataIface, TBaseDataIface> : IView<C, TBaseDataIface>
		where C : AbstractController
		where TBaseDataIface : IUIData
		where TFinalDataIface : TBaseDataIface, new()
	{
		private TFinalDataIface _data ;
		private C _controller;

		protected AbstractTestView()
		{
			_data = new TFinalDataIface();
		}

		TBaseDataIface IView<C, TBaseDataIface>.Data
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
}
