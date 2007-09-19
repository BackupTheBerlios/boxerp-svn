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
	[Serializable]
	public class BdWithBindableCollection<T, Y> : AbstractBindableWrapper<T, BdWithBindableCollection<T, Y>.WrapObject<T, Y>>
		where Y : IBindableWrapper
	{
		public BdWithBindableCollection(T businessObj, List<Y> sourceList)
			: base(businessObj, new object[] { sourceList })
		{ }

		public BdWithBindableCollection(T businessObj)
			: base (businessObj)
		{}
			
		public Type GetRelatedObjectType()
		{
			return typeof(Y);
		}

		public virtual Type GetCollectionType()
		{
			return typeof(List<Y>);
		}
		
		public override void Undo()
		{
			base.Undo();
			foreach (Y listItem in Data.Collection)
			{
				listItem.Undo();
			}
		}
			
		public override void Redo()
		{
			base.Redo();
			foreach (Y listItem in Data.Collection)
			{
				listItem.Redo();
			}
		}

		[Serializable]
		public class WrapObject<D, Z> : AbstractBindableWrapper<D, BdWithBindableCollection<D, Z>.WrapObject<D, Z>>.BindableFields<D>
			where Z : IBindableWrapper
		{
			[NonSerialized]
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
					
			public WrapObject(IInterceptor interceptor, List<Z> sourceList)
				: base(interceptor)
			{
				// to intercept changes in the list fields: add/remove items
				_list = (List<Z>)_proxyGenerator.CreateClassProxy(typeof(List<Z>), interceptor);
				foreach (Z sourceItem in sourceList)
				{
						_list.Add(sourceItem);
				}
			}
		}
	}
}
