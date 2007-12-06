using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Migrator
{
	public class CodeGenerator
	{
		public static string CreateViewIfaceName(string name)
		{
			return "I" + name;
		}

		public static string CreateControllerName(string name)
		{
			return name + "Controller";
		}

		public static string CreateTestViewName(string name)
		{
			return "Test" + name;
		}

		public static string CreateSharedDataIfaceName(string name)
		{
			return "I" + name + "Data";
		}

		public static string CreateSharedDataAbstractName(string name)
		{
			return "Abstract" + name + "Data";
		}

		public static string CreateSharedDataConcreteName(string name, string guiToolkit)
		{
			return guiToolkit + name + "Data";
		}

		public static string CreateSharedDataTestName(string name)
		{
			return "Test" + name + "Data";
		}

		private static void generateHeader(StringBuilder writer)
		{
			writer.Append(Singleton.Instance.SourcesHeader);
			writer.AppendLine();
			writer.AppendLine("namespace " + Singleton.Instance.Namespc);
			writer.AppendLine("{");
		}

		private static void generateFooter(StringBuilder writer)
		{
			writer.AppendLine("}");
		}

		public static void GenerateViewInterfaceDeclaration(string name, StringBuilder writer, bool createSharedData)
		{
			generateHeader(writer);
			writer.Append("\tpublic interface " + CreateViewIfaceName(name) + " : ");
			if (createSharedData)
			{
				writer.Append("IView<" + CreateControllerName(name) + ", " + CreateSharedDataIfaceName(name) + ">");
			}
			else
			{
				writer.Append("IView<" + CreateControllerName(name) + ">");
			}
			writer.AppendLine();
			writer.AppendLine("\t{");
			writer.AppendLine();
		}

		public static void CloseViewInterface(StringBuilder writer)
		{
			writer.AppendLine("\t}");
			generateFooter(writer);
		}

		public static void CreateProperty(string typeName, string propName, string field, StringBuilder writer, bool hasSetter)
		{
			writer.AppendLine("\t\tpublic " + typeName + " " + propName);
			writer.AppendLine("\t\t{");
			writer.AppendLine("\t\t\tget");
			writer.AppendLine("\t\t\t{");
			writer.AppendLine("\t\t\t\treturn " + field + ";");
			writer.AppendLine("\t\t\t}");
			if (hasSetter)
			{
				writer.AppendLine("\t\t\tset");
				writer.AppendLine("\t\t\t{");
				writer.AppendLine("\t\t\t\t" + field + " = value;");
				writer.AppendLine("\t\t\t}");
			}
			writer.AppendLine("\t\t}");
		}

		public static void AddMethodToInterface(string line, StringBuilder writer)
		{
			string substr = "\t\t\t" + line.Substring(line.IndexOf("public") + 6);
			substr = substr.Substring(0, substr.IndexOf(')') +1) + ";";
			writer.AppendLine(substr);
		}

		public static void AddControllerFieldToView(string name, StringBuilder writer)
		{
			writer.AppendLine("\t\tprivate " + CreateControllerName(name) + " _controller;");
			CreateProperty(CreateControllerName(name), "Controller", " _controller", writer, true);
		}

		public static void AddDataFieldToView(string name, StringBuilder writer, string guiToolkit)
		{
			writer.AppendLine("\t\tprivate " + CreateSharedDataConcreteName(name, guiToolkit) + " _data;");
			CreateProperty(CreateSharedDataIfaceName(name), "SharedData", " _data", writer, false);
		}

		public static void GenerateController(string name, StringBuilder writer, bool createSharedData)
		{
			generateHeader(writer);
			writer.Append("\tpublic class " + CreateControllerName(name) + " : ");
			if (createSharedData)
			{
				writer.AppendLine(String.Format("AbstractController<{0},{1},{2}>",
						CreateViewIfaceName(name),
						CreateSharedDataIfaceName(name),
						CreateControllerName(name)));
			}
			else
			{
				writer.AppendLine(String.Format("AbstractController<{0},{1}>",
						CreateViewIfaceName(name),
						CreateControllerName(name)));
			}
			writer.AppendLine("\t{");
			writer.AppendLine("\t\tpublic " + CreateControllerName(name) + "(IResponsiveClient helper, " + CreateViewIfaceName(name) + " view)");
			writer.AppendLine("\t\t\t: base (helper, view)");
			writer.AppendLine("\t\t{}");
			writer.AppendLine();
			writer.AppendLine("\t\tprotected override void OnAsyncOperationFinish(object sender, ThreadEventArgs args)");
			writer.AppendLine("\t\t{");
			writer.AppendLine("\t\t\tthrow new Exception(\"The method or operation is not implemented.\");");
			writer.AppendLine("\t\t}");

			writer.AppendLine("\t}");
			generateFooter(writer);
		}

		public static void GenerateTestView(string name, StringBuilder writer, bool createSharedData, List<string> methods)
		{
			generateHeader(writer);
			writer.AppendLine(String.Format("\tpublic class {0} : AbstractTestView<{1}, {2}, {3}>, {4}",
				CreateTestViewName(name),
				CreateControllerName(name),
				CreateSharedDataConcreteName(name, "Test"),
				CreateSharedDataIfaceName(name),
				CreateViewIfaceName(name)));

			writer.AppendLine("\t{");
			writer.AppendLine("\t\tpublic " + CreateTestViewName(name) + "()");
			//writer.AppendLine("\t\t\tbase (helper, view)");
			writer.AppendLine("\t\t{");
			writer.AppendLine("\t\t\tCreateData();");
			writer.AppendLine("\t\t}");
			writer.AppendLine();
		
			writer.AppendLine("\t\tprotected override void CreateData()");
			writer.AppendLine("\t\t{");
			writer.AppendLine("\t\t\t_data = new " + CreateSharedDataTestName(name) + "(this);");
			writer.AppendLine("\t\t}");
			writer.AppendLine();

			foreach (string method in methods)
			{
				writer.AppendLine(method);
				writer.AppendLine("\t\t{");
				writer.AppendLine("\t\t\tthrow new Exception(\"The method or operation is not implemented.\");");
				writer.AppendLine("\t\t}");
			}

			writer.AppendLine("\t}");
			generateFooter(writer);
		}

		public static void GenerateSharedDataIface(string name, StringBuilder writer)
		{
			generateHeader(writer);
			writer.AppendLine("\tpublic interface " + CreateSharedDataIfaceName(name) + " : IUIData");
			writer.AppendLine("\t{");
			writer.AppendLine();
			writer.AppendLine("\t}");
			generateFooter(writer);
		}

		public static void GenerateSharedDataAbstract(string name, StringBuilder writer)
		{
			generateHeader(writer);
			writer.AppendLine(String.Format("\tpublic abstract class {0}<T> : AbstractData<{1}, {2}, T>, {3}",
					CreateSharedDataAbstractName(name),
					CreateViewIfaceName(name),
					CreateControllerName(name),
					CreateSharedDataIfaceName(name)));
			writer.AppendLine("\t\twhere T : " + CreateViewIfaceName(name));
			writer.AppendLine("\t{");
			writer.AppendLine("\t\tpublic " + CreateSharedDataAbstractName(name) + "(T view)");
			writer.AppendLine("\t\t\t: base(view)");
			writer.AppendLine("\t\t{}");
			writer.AppendLine();
			writer.AppendLine("\t}");
			generateFooter(writer);
		}

		private static void generateSharedDataConcrete(string concreteName, string name, string viewName, StringBuilder writer)
		{
			generateHeader(writer);
			writer.AppendLine(String.Format("\tpublic class {0} : {1}<{2}>",
				  concreteName,
				  CreateSharedDataAbstractName(name),
				  viewName));
			writer.AppendLine("\t{");
			writer.AppendLine(String.Format("\t\tpublic {0}({1} view)",
					concreteName,
					viewName));
			writer.AppendLine("\t\t\t: base(view)");
			writer.AppendLine("\t\t{}");
			writer.AppendLine("\t}");
			generateFooter(writer);
		}

		public static void GenerateSharedDataConcrete(string name, StringBuilder writer, string guiToolkit)
		{
			generateSharedDataConcrete(CreateSharedDataConcreteName(name, guiToolkit), name, name, writer);
		}

		public static void GenerateSharedDataTest(string name, StringBuilder writer)
		{
			generateSharedDataConcrete(CreateSharedDataTestName(name), name, CreateTestViewName(name), writer);
		}
	}
}
