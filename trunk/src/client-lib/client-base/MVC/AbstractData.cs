using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Boxerp.Client
{
	/// <summary>
	/// Use this class to implement your SharedData interface, leaving the third type parameter generic.
	/// So the concrete implementations will need less code to work. 
	/// 
	/// Implementation example: .doc/Examples/AbstractData.cs
	/// </summary>
	/// <param name="TBaseView">The View Interface</param>
	/// <param name="C">The Controller</param>
	/// <param name="TFinalView">The Final implementation of the View. Leave it as T</param>
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

		/// <summary>
		/// This copies the idea of the CastleProject.MonoRail engine to pass data from the controller 
		/// to the view. You can just store and retrieve objects to shared then between view and 
		/// controller, although a strongly typed definition is encouraged. Define the properties
		/// you need in the class that extends this one to have strongly typed shared data.
		/// </summary>
		public Hashtable PropertyBag
		{
			get 
			{
				return _propertyBag;
			}
		}
		
		protected AbstractData(TFinalView view)
		{
			View = view;
		}

		/** \example AbstractData.cs
		* 
		*/
	}
}
