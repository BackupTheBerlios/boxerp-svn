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

namespace Boxerp.Client
{
	public class DecimalTextBoxHelper
	{
		public static string CleanString(string val, int decimalDigits, char decimalSeparator)
		{
			string currentStr = val;
			string cleaned = String.Empty;
			if (currentStr.Length > 0)
			{
				bool readingDecimals = false;
				int decimals = 0;
				foreach (char c in currentStr)
				{
					if ((readingDecimals) && (decimals == decimalDigits))
					{
						break;
					}
					else if ((readingDecimals) && (c == decimalSeparator))
					{
						break;
					}
					else if (readingDecimals)
					{
						decimals++;
					}

					if (Char.IsNumber(c))
					{
						cleaned += c.ToString();
					}
					else if (c == decimalSeparator)
					{
						readingDecimals = true;
						cleaned += c.ToString();
					}
				}
				if (cleaned == ".")
				{
					cleaned = "0.0";
				}
				else if (cleaned.Length == 0)
				{
					return "0";
				}
			}
			return cleaned;
		}
	}
}
