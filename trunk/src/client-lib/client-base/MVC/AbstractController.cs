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
	/// This class shoulb be used by the framework only
	/// </summary>
	public abstract class InternalAbstractController<V, TData, TSelf> : InternalAbstractController<V, TSelf>
		where TSelf: InternalAbstractController<V, TData, TSelf>
		where V : IView<TSelf, TData> 
		where TData : IUIData
	{
		protected InternalAbstractController(IResponsiveClient helper, V view)
			: base(helper, view)
		{ }

		protected InternalAbstractController(IResponsiveClient helper)
			: base(helper)
		{ }
	}

	
	public abstract class AbstractController<V, TData> : InternalAbstractController<V, TData, AbstractController<V, TData>>
		where V : IView<AbstractController<V, TData>, TData>
		where TData : IUIData
	{
		private TData _data;

		protected AbstractController(IResponsiveClient helper, V view)
			: base(helper, view)
		{ 
			_data = view.SharedData;		
		}

		protected AbstractController(IResponsiveClient helper)
			: base(helper)
		{ }

		public TData SharedData
		{
			get
			{
				return _data;
			}
		}
	}

	/// <summary>
	/// This should be used only by the framework
	/// </summary>
	public abstract class InternalAbstractController<V, TSelf> : AbstractController
		where TSelf: InternalAbstractController<V, TSelf>
		where V : IView<TSelf>
	{
		protected InternalAbstractController(IResponsiveClient helper, V view)
			: base(helper)
		{}

		protected InternalAbstractController(IResponsiveClient helper)
			: base(helper)
		{}

	}
	
	public abstract class AbstractController<V> : InternalAbstractController<V, AbstractController<V>>
		where V : IView<AbstractController<V>>
	{
		private V _view = default(V);

		protected AbstractController(IResponsiveClient helper, V view)
			: base(helper)
		{
			_view = view;
			_view.Controller = this;
		}

		protected AbstractController(IResponsiveClient helper)
			: base(helper)
		{}

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
	/// All the controllers should extend this class, which enforce the responsive helper to be protected
	/// and binds the complete event to the operation finish method
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

		protected MethodBase MethodBase(SimpleDelegate del)
		{
			return (MethodBase)del.Method;
		}
	}
}
