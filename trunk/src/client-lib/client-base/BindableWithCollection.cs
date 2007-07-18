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
	public class BindableWithCollection<T, Y> : AbstractBindableWrapper<T, BindableWithCollection<T, Y>.WrapObject<T, Y>>
	{
		public BindableWithCollection(T businessObj)
			: base (businessObj, typeof(BindableWithCollection<T, Y>.WrapObject<T, Y>))
		{}

		public Type GetRelatedObjectType()
		{
			return typeof(Y);
		}

		public virtual Type GetCollectionType()
		{
			return typeof(List<Y>);
		}
		
		public class WrapObject<D, Z> : AbstractBindableWrapper<D, BindableWithCollection<D, Z>.WrapObject<D, Z>>.BindableFields<D>
		{
			private ProxyGenerator _proxyGenerator = new ProxyGenerator();
			private List<Z> _list;
			
			public virtual List<Z> Collection     // virtual to intercept the get and set
			{
				get 
				{ 
					return _list; 
				}
				
				internal set 
				{ 
					_list = value; 
				}
			}

			public WrapObject(IInterceptor interceptor)
				: base(interceptor)
			{
				// to intercept changes in the list fields: add/remove items
				_list = (List<Z>)_proxyGenerator.CreateClassProxy(typeof(List<Z>), interceptor);
			}
		}
	}
}
