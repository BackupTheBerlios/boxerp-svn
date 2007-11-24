using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace Boxerp.Client
{
	public class ControllerFactory<C, I>
		where C : AbstractController<I, C>
		where I : IView<C>
	{
		public static C CreateTestController(ConcurrencyMode mode)
		{
			ConstructorInfo cInfo = typeof(C).GetConstructor(new Type[] { typeof(IResponsiveClient), typeof(I) });
			C instance = (C)cInfo.Invoke(new object[] { new ConsoleResponsiveHelper(mode), null });
			return instance;
		}
	}
}
