using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	/// <summary>
	/// Interface for any question dialog
	/// </summary>
	public interface IQuestionWindow
	{
		string Msg { get; set; }
		string AfirmativeOption { get; set; }
		string NegativeOption { get; set; }
		void ShowDialog();
		bool IsAfirmative { get; set; }
		void Close();
	}
}
