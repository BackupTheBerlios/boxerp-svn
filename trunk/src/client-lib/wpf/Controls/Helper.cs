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
using System.Windows.Input;

namespace Boxerp.Client.WPF.Controls
{
	public class Helper
	{
		public static bool ControlKeyIsPressed()
		{
			return (Keyboard.Modifiers == ModifierKeys.Control);
		}

		public static bool IsValidKey(Key key)
		{
			if ((key != Key.Tab) && (key != Key.Left) && (key != Key.Right) &&
				(key != Key.Enter) && (key != Key.End) &&
				(key != Key.Home) && (key != Key.Clear)
				&& (key != Key.RightShift) && (key != Key.LeftShift) && (key != Key.CapsLock)
				&& (key != Key.LeftCtrl) && (key != Key.RightCtrl))
			{
				return true;
			}

			return false;
		}

		public static bool IsNumber(Key key, char character)
		{
			if ((key != Key.Delete) && (key != Key.Back) && (!Char.IsNumber(character)))
			{
				return false;
			}

			return true;
		}

		public static bool IsLetterOrDigit(Key key, char character)
		{
			if ((key != Key.Delete) && (key != Key.Back) && (!Char.IsLetterOrDigit(character)))
			{
				return false;
			}

			return true;
		}

		public static bool IsDeleteOrBack(Key key)
		{
			return ((key == Key.Delete) || (key == Key.Back));
		}

	}
}
