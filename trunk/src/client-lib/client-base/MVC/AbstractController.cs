//
// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
//
// Redistribution and use in source and binary forms, with or
// without modification, are permitted provided that the following
// conditions are met:
// Redistributions of source code must retain the above
// copyright notice, this list of conditions and the following
// disclaimer.
// Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials
// provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Boxerp.Client
{
	/// <summary>
	/// Extend this class when you need a controller with SharedData in addition to the reference to the View.
	/// Read about the base class (AbstractController) for more information.
	/// </summary>
	/// <param name="V">The View Interface</param>
	/// <param name="TData">The SharedData Interface</param>
	/// <param name="C">The name of Controller itself</param>
    public abstract class AbstractController<V, TData, C> : AbstractController<V, C>
		where V : IView<C, TData>
		where C : AbstractController<V, TData, C>
		where TData : IUIData
	{
		private TData _data;

		protected AbstractController(IResponsiveClient helper, V view)
			: base(helper, view)
		{ 
			Console.Out.WriteLine ("assigning data:" + typeof(TData));
			_data = view.SharedData;
		}

		protected AbstractController(IResponsiveClient helper)
			: base(helper)
		{ }

		/// <summary>
		/// Access to the SharedData instance
		/// </summary>
		public TData SharedData
		{
			get
			{
				return _data;
			}
		}
	}

	/// <summary>
	/// Extend this class when you are going to create a Controller that does not need SharedData but 
	/// just a View:
	/// public class MyController : AbstractController<MyView, MyController>
	/// 
	/// Read about the base class (AbstractController) for more information.
	/// </summary>
	/// <param name="V">The View Interface</param>
	/// <param name="C">The name of the Controller itself</param>
	public abstract class AbstractController<V, C> : AbstractController
		where C : AbstractController<V, C>
		where V : IView<C>
	{
		private V _view = default(V);

		/// <summary>
		/// Use this constructor to get the controller fully initialized
		/// </summary>
		/// <param name="helper">One of the implementations of the IResponsiveClient: WpfResponsiveHelper, GtkResponsiveHelper, WinFormsResponsiveHelper</param>
		/// <param name="view">The instance of the view</param>
		protected AbstractController(IResponsiveClient helper, V view)
			: base(helper)
		{
			_view = view;
			_view.Controller = (C)this;
		}

		protected AbstractController(IResponsiveClient helper)
			: base(helper)
		{}

		/// <summary>
		/// Access to the View instance
		/// </summary>
		public V View
		{
			get
			{
				return _view;
			}
			set
			{
				_view = value;
			}
		}
	}

	/// <summary>
	/// Base class for Controllers. Although a Controller can extend this class, you should 
	/// extend one of the generic versions, AbstractController<V,C> or 
	/// AbstractController<V,TData,C>. 
	/// 
	/// Read more about the Controller in the boxerp.org wiki
	/// </summary>
	public abstract class AbstractController : IController
	{
		protected IResponsiveClient _responsiveHelper;

		protected AbstractController(IResponsiveClient helper)
		{
			_responsiveHelper = helper;
			_responsiveHelper.TransferCompleteEvent += OnAsyncOperationFinish;
		}

		protected IResponsiveClient ResponsiveHelper
		{
			get 
			{ 
				return _responsiveHelper; 
			}
			set 
			{ 
				_responsiveHelper = value; 
			}
		}

		internal void setResponsiveHelper(IResponsiveClient helper)
		{
			_responsiveHelper = helper;
		}

		public int CurrentThread
		{
			get
			{
				return System.Threading.Thread.CurrentThread.ManagedThreadId;
			}
		}

		/// <summary>
		/// This method is called whenever a background operation finishes.
		/// 
		/// Implementation example: .doc/Examples/OnAsyncOperationFinish.cs
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		protected abstract void OnAsyncOperationFinish(Object sender, ThreadEventArgs args);

		/// <summary>
		/// This method is a short version for calling StopAsyncMethod in the responsive helper. Use it when you don't have to pass 
		/// any extra information on why the method is being stopped
		/// </summary>
		protected void StopAsyncCall()
		{
			System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1);
			System.Reflection.MethodBase mb = sf.GetMethod();
			_responsiveHelper.StopAsyncMethod(System.Threading.Thread.CurrentThread.ManagedThreadId, mb, null);
		}

		/// <summary>
		/// Use this to compare the ThreadEventArgs.MethodBase property with a method so that you figure out 
		/// which background method has finished. See the implementation of the OnAsyncOperationFinish
		/// </summary>
		/// <param name="del"></param>
		/// <returns></returns>
		protected MethodBase MethodBase(SimpleDelegate del)
		{
			return (MethodBase)del.Method;
		}

		/** \example OnAsyncOperationFinish.cs
		* 
		*/
	}
}
