////
//// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
////
//// Redistribution and use in source and binary forms, with or
//// without modification, are permitted provided that the following
//// conditions are met:
//// Redistributions of source code must retain the above
//// copyright notice, this list of conditions and the following
//// disclaimer.
//// Redistributions in binary form must reproduce the above
//// copyright notice, this list of conditions and the following
//// disclaimer in the documentation and/or other materials
//// provided with the distribution.
////
//// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
//// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
//// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
//// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
//// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
//// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
//// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
//// THE POSSIBILITY OF SUCH DAMAGE.
//
//

using System;

namespace Boxerp.Client
{
	
	
	public class BindingMetadata
	{
		private string _widgetName;
		private string _widgetBindingProperty;
		private BindingOptions _bindingOptions;
		private string _bindingPath;
		
		public virtual string WidgetName 
		{
			get 
			{
				return _widgetName;
			}
			set
			{
				_widgetName = value;
			}
		}

		public virtual string WidgetBindingProperty 
		{
			get 
			{
				return _widgetBindingProperty;
			}
			set
			{
				_widgetBindingProperty = value;
			}
		}

		public virtual BindingOptions BindingOptions 
		{
			get 
			{
				return _bindingOptions;
			}
			set
			{
				_bindingOptions = value;
			}
		}

		public virtual string BindingPath 
		{
			get 
			{
				return _bindingPath;
			}
			set
			{
				_bindingPath = value;
			}
		}
		
		public BindingMetadata(){}
		
		public BindingMetadata(string widgetName, string bindingProperty, BindingOptions options, string bindingPath)
		{
			_widgetName = widgetName;
			_widgetBindingProperty = bindingProperty;
			_bindingOptions = options;
			_bindingPath = bindingPath;
		}
	}
}
