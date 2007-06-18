using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	public abstract class AbstractBindableWrapper<T, Y> : IBindableWrapper<T>
		where Y : AbstractBindableWrapper<T, Y>.BindableFields<T>
	{
		private Y _bindableFields;
		private Stack<Y> _undoStack = new Stack<Y>();
		private Stack<Y> _redoStack = new Stack<Y>();

		public Y Data
		{
			get
			{
				return _bindableFields;
			}

			set
			{
				_undoStack.Push(_bindableFields);
				_bindableFields = value;
			}
		}

		public void UnDo()
		{
			if (_undoStack.Count > 0)
			{
				_redoStack.Push(_bindableFields);
				_bindableFields = _undoStack.Pop();
			}
		}

		public void ReDo()
		{
			if (_redoStack.Count > 0)
			{
				_undoStack.Push(_bindableFields);
				_bindableFields = _redoStack.Pop();
			}
		}

		public override string ToString()
		{
			return Data.BusinessObj.ToString();
		}

		public abstract class BindableFields<D>
		{
			private D _businessObj;

			public BindableFields(D businessObj)
			{
				_businessObj = businessObj;
			}
	
			public D BusinessObj
			{
				get
				{
					return _businessObj;
				}

				set
				{
					_businessObj = value;
				}
			}
		}
	}
}
