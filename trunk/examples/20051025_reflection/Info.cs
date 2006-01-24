using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

public class Garbage
{
	public Garbage(){}

	public void BlaBlaBla() {}
}


public class Info
{

	public Info() {}

	public void OtherMethod ()
	{

	}

	public void WhoAmI (string arg1)
	{
		Assembly assembly = Assembly.GetExecutingAssembly();
		System.Type t = assembly.GetType(this.ToString());	// Get only this class
		Console.WriteLine("This class name is: {0}", t.ToString());
		MethodInfo[] mInfo = t.GetMethods();
		MemberInfo[] bInfo = t.GetMembers();
		FieldInfo[]  fInfo = t.GetFields();
		foreach (MethodInfo m in mInfo)
				Console.WriteLine("Method:  {0}", m.Name);
		foreach (MemberInfo b in bInfo)
				Console.WriteLine("Member:  {0}", b.Name);
		foreach (FieldInfo f in fInfo)
				Console.WriteLine("Field:  {0}", f.Name);

		StackFrame stackFrame = new StackFrame();
		MethodBase methodBase = stackFrame.GetMethod();
		Console.WriteLine("This method name is : {0}", methodBase.Name );

		/*System.Type[] types = assembly.GetTypes();
		foreach (System.Type t in types)
		{
			Console.WriteLine("Tipo: {0}", t.ToString());
			MethodInfo[] mInfo = t.GetMethods();
			MemberInfo[] bInfo = t.GetMembers();
			foreach (MethodInfo m in mInfo)
					Console.WriteLine("Modulo:  {0}", m.Name);
			foreach (MemberInfo b in bInfo)
					Console.WriteLine("Miembro:  {0}", b.Name);
		}*/
		
	}

	public static void Main ()
	{
		Info i = new Info();
		i.WhoAmI("hola");
	}
}
