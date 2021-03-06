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
using Castle.DynamicProxy;
using Castle.Core.Interceptor;

namespace Boxerp.Client
{
	/// <summary>
	/// It is the most used wrapper among the Bindable Wrappers. It does not add any extra 
	/// property to the business object but just wraps it and makes it bindable.
	/// </summary>
	/// <param name="T"></param>
	[Serializable]
	public class BindableWrapper<T> : AbstractBindableWrapper<T, BindableWrapper<T>.WrapObject<T>>
	{
		public BindableWrapper(T businessObj)
			: base(businessObj, true, false, false)
		{ }

		public BindableWrapper(T businessObj, bool disableInterception)
			: base(businessObj, disableInterception, disableInterception)
		{ }

		public BindableWrapper(T businessObj, bool disableInterception, bool disableUndoRedo)
			: base(businessObj, true, disableInterception, disableUndoRedo)
		{ }

		[Serializable]
		public class WrapObject<D> : AbstractBindableWrapper<D, BindableWrapper<D>.WrapObject<D>>.BindableFields<D>
		{
			public WrapObject(IInterceptor interceptor)
				: base(interceptor)
			{ 
			
			}
		}
	}
}
