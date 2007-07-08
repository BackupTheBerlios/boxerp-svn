// /home/carlos/boxerp_completo/trunk/src/client-lib/tests/bindableObjectsTests/bindableObjectsTests/NestedBusinessObject.cs created with MonoDevelop
// User: carlos at 4:45 PM 7/7/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace bindableObjectsTests
{
	
	
	public class NestedBusinessObject : ICloneable
	{
		private bool _someFlag;
		private SimpleBusinessObject _nestedObject;
		
		public virtual bool SomeFlag 
		{
			get 
			{
				return _someFlag;
			}
			set
			{
				_someFlag = value;
			}
		}

		public virtual bindableObjectsTests.SimpleBusinessObject NestedObject 
		{
			get 
			{
				return _nestedObject;
			}
			set
			{
				_nestedObject = value;
			}
		}
		
		public NestedBusinessObject()
		{
		}
		
		private NestedBusinessObject(bool flag, SimpleBusinessObject nested)
		{
			_someFlag = flag;
			_nestedObject = nested;
		}
		
		public object Clone()
		{
			return new NestedBusinessObject(_someFlag, _nestedObject);
		}
	}
}
