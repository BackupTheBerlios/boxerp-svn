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
	public class AbstractTestView<ViewIface, C, D, DataIface> : IView<ViewIface, C, DataIface>
		where C : AbstractController<ViewIface, DataIface>
		where DataIface : IUIData
		where D : DataIface, new()
		where ViewIface : IView<ViewIface, C, DataIface>
	{
		private D _data ;
		private C _controller;

		protected AbstractTestView()
		{
			_data = new D();
		}

		DataIface IView<ViewIface, C, DataIface>.Data
		{
			get
			{
				return _data;
			}
		}

		public D Data
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
