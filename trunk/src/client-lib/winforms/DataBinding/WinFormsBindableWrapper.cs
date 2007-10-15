using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Boxerp.Client;

namespace Boxerp.Client.WindowsForms
{
	public class WinFormsBindableWrapper<T> : BindableWrapper<T>
	{
		Form form;
		Dictionary<string, string> boundObjectPropertyStringCases = new Dictionary<string, string>();

		public WinFormsBindableWrapper(T businessObj, Form form)
			: base(businessObj)
		{
			this.form = form;
			buildPropertyInfoDictionary();
			bind();
		}

		public WinFormsBindableWrapper(T businessObj, Form form, bool disableInterception)
			: base(businessObj, disableInterception)
		{
			this.form = form;
			buildPropertyInfoDictionary();
			bind();
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

		private void bind()
		{
			Type formType = form.GetType();
			System.Diagnostics.Debug.Assert(formType != null);
			FieldInfo[] fieldInfos = formType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			foreach (FieldInfo lFieldInfo in fieldInfos)
			{
				System.Diagnostics.Debug.WriteLine("bind " + lFieldInfo.Name+" type:"+lFieldInfo.FieldType);
				if (lFieldInfo.FieldType.IsSubclassOf(typeof(Control)))
				{
					if (boundObjectPropertyStringCases.ContainsKey(lFieldInfo.Name))
					{
						System.Diagnostics.Debug.WriteLine("binding " + lFieldInfo.Name + " type:" + lFieldInfo.FieldType +" to:"+boundObjectPropertyStringCases[lFieldInfo.Name]);
						Console.WriteLine("binding " + lFieldInfo.Name + " type:" + lFieldInfo.FieldType + " to:" + boundObjectPropertyStringCases[lFieldInfo.Name]);

						(lFieldInfo.GetValue(this) as Control).DataBindings.Add(
							"Text", 
							this.Data.BusinessObj, 
							boundObjectPropertyStringCases[lFieldInfo.Name], 
							false, 
							DataSourceUpdateMode.OnPropertyChanged);
					}
				}

			}
		}
		private static string lcaseWord(string word)
		{
			return word.Substring(0, 1).ToLowerInvariant() + word.Substring(1);
		}
	}
}
