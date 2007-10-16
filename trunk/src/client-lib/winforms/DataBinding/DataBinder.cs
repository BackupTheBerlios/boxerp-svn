using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Boxerp.Client;
using System.Reflection;

namespace Boxerp.Client.WindowsForms
{
	public class DataBinder<T, Y> : IWinFormsDataBinder<T, Y>
		where T : IBindableWrapper<T>
		where Y : IContainerControl
	{
		private Dictionary<string, string> boundObjectPropertyStringCases = new Dictionary<string, string>();
		private T _bindableWrapper;
		private Y _control;

		public DataBinder(T bindableObj, Y control)
		{
			_bindableWrapper = bindableObj;
			_control = control;
			buildPropertyInfoDictionary();
		}

		private void buildPropertyInfoDictionary()
		{
			foreach (PropertyInfo pInfo in typeof(T).GetProperties())
			{
				boundObjectPropertyStringCases.Add(pInfo.Name, pInfo.Name);
				boundObjectPropertyStringCases.Add("_" + pInfo.Name, pInfo.Name);
				string lPInfoName = lcaseWord(pInfo.Name);
				if (pInfo.Name != lPInfoName)
				{
					boundObjectPropertyStringCases.Add(lPInfoName, pInfo.Name);
					boundObjectPropertyStringCases.Add("_"+lPInfoName, pInfo.Name);
				}
			}
		}

		#region IWinFormsDataBinder<T> Members

		public Y Control
		{
			get
			{
				throw new Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		#endregion

		#region IDataBinder<T> Members

		public void BindWithXml()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void BindWithReflection()
		{
			Type formType = typeof(Y);
			System.Diagnostics.Debug.Assert(formType != null);
			FieldInfo[] fieldInfos = formType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			foreach (FieldInfo lFieldInfo in fieldInfos)
			{
				System.Diagnostics.Debug.WriteLine("bind " + lFieldInfo.Name + " type:" + lFieldInfo.FieldType);
				if (lFieldInfo.FieldType.IsSubclassOf(typeof(Control)))
				{
					if (boundObjectPropertyStringCases.ContainsKey(lFieldInfo.Name))
					{
						System.Diagnostics.Debug.WriteLine("binding " + lFieldInfo.Name + " type:" + lFieldInfo.FieldType + " to:" + boundObjectPropertyStringCases[lFieldInfo.Name]);
						Console.WriteLine("binding " + lFieldInfo.Name + " type:" + lFieldInfo.FieldType + " to:" + boundObjectPropertyStringCases[lFieldInfo.Name]);

						(lFieldInfo.GetValue(this) as Control).DataBindings.Add(
							"Text",
							_bindableWrapper.Data.BusinessObj,
							boundObjectPropertyStringCases[lFieldInfo.Name],
							false,
							DataSourceUpdateMode.OnPropertyChanged);
					}
				}
			}
		}

		public void BindWithXml(string xml)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public T BindableWrapper
		{
			get
			{
				return _bindableWrapper;
			}
			set
			{
				_bindableWrapper = value;
			}
		}

		#endregion

		private static string lcaseWord(string word)
		{
			return word.Substring(0, 1).ToLowerInvariant() + word.Substring(1);
		}
	}
}
