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
	/// The business object T becomes bindable. A collection of Y elements is made bindable as well and
	/// such elements are also wrapper with a BindablwWrapper so they don't have to be bindable before.
	/// Collection will be of type BindableCollection<BindableWrapper<Y>>
	/// </summary>
	/// <param name="T">The business object</param>
	/// 
	/// <param name="Y">The item type of the collection</param>
	[Serializable]
	public class BindableWithCollection<T, Y> : AbstractBindableWrapper<T, BindableWithCollection<T, Y>.WrapObject<T, Y>>
	{
		public BindableWithCollection(T businessObj)
			: base (businessObj)
		{}

		public Type GetRelatedObjectType()
		{
			return typeof(Y);
		}

		public virtual Type GetCollectionType()
		{
			return typeof(BindableCollection<BindableWrapper<Y>>);
		}
		
		[Serializable]
		public class WrapObject<D, Z> : AbstractBindableWrapper<D, BindableWithCollection<D, Z>.WrapObject<D, Z>>.BindableFields<D>
		{
			[field: NonSerialized]
			private ProxyGenerator _proxyGenerator = new ProxyGenerator();

			private BindableCollection<BindableWrapper<Z>> _list
				= new BindableCollection<BindableWrapper<Z>>();
			
			public virtual BindableCollection<BindableWrapper<Z>> Collection     // virtual to intercept the get and set
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

			/// <summary>
			/// This constructor is needed by the base class
			/// </summary>
			/// <param name="interceptor"></param>
			public WrapObject(IInterceptor interceptor)
				: base(interceptor)
			{
			}

			public WrapObject(IInterceptor interceptor, List<Z> sourceList)
				: base(interceptor)
			{
				_list.AddingNew += OnAddingItem;
				foreach (Z item in sourceList)
				{
					Collection.Add(new BindableWrapper<Z>(item));
				}
			}

			/// <summary>
			/// Wrap the item to make it bindable before adding to the collection
			/// </summary>
			/// <param name="sender"></param>
			/// <param name="args"></param>
			private void OnAddingItem(Object sender, EventArgs args)
			{
				Z newItem = (Z)sender;
				Collection.Add(new BindableWrapper<Z>(newItem));
			}
		}
	}
}
