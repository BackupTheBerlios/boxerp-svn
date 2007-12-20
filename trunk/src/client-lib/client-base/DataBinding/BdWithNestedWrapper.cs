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
	/// The business object T becomes bindable and its nested object Y too.
	/// </summary>
	/// <param name="T">The business object</param>
	/// 
	/// <param name="Y">The nested business object</param>
	[Serializable]
	public class BdWithNestedWrapper<T, Y> : AbstractBindableWrapper<T, BdWithNestedWrapper<T, Y>.WrapObject<T, Y>>
		where Y : IBindableWrapper
	{
		public BdWithNestedWrapper(T businessObj, Y nestedWrapper)
			: base(businessObj, new object[] { nestedWrapper })
		{ }

		public BdWithNestedWrapper(T businessObj)
			: base (businessObj)
		{}
			
		public Type GetNestedObjectType()
		{
			return typeof(Y);
		}

		public override void Undo()
		{
			base.Undo();
			Data.NestedWrapper.Undo();	
		}
			
		public override void Redo()
		{
			base.Redo();
			Data.NestedWrapper.Redo();
		}

		[Serializable]
		public class WrapObject<D, Z> : AbstractBindableWrapper<D, BdWithNestedWrapper<D, Z>.WrapObject<D, Z>>.BindableFields<D>
				where Z : IBindableWrapper
		{
			private Z _nestedWrapper;
			
			public virtual Z NestedWrapper     // virtual to intercept the get and set
			{
				get 
				{ 
					return _nestedWrapper; 
				}
				
				set 
				{ 
					_nestedWrapper = value; 
				}
			}

			
			public WrapObject(IInterceptor interceptor)
				: base(interceptor)
			{
				
			}
					
			public WrapObject(IInterceptor interceptor, Z nestedWrapper)
				: base(interceptor)
			{
				_nestedWrapper = nestedWrapper;
			}
		}
	}
}
