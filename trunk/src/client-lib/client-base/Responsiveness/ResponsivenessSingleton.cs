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
using System.Threading;

namespace Boxerp.Client
{
	/// <summary>
	/// Configure this class at the beginnin of the application to let Boxerp know which is 
	/// the main thread:
	/// ResponsivenessSingleton.GetInstance().Initialize();
	/// </summary>
	public class ResponsivenessSingleton
	{
		private int _mainThreadId = -1;
		private int _currentThreadId;
		private static ResponsivenessSingleton _instance = null;

		public int CurrentThreadId
		{
			get { return _currentThreadId; }
			set { _currentThreadId = value; }
		}

		public int MainThreadId
		{
			get { return _mainThreadId; }
			set { _mainThreadId = value; }
		}

		public bool IsUIThread()
		{
			_currentThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
			if (_currentThreadId == _mainThreadId)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// The method to call at the beginning of the application.
		/// </summary>
		public void Initialize()
		{
			_mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
		}

		private ResponsivenessSingleton(){}

		/// <summary>
		/// As this class is a Singleton, this is the way to access it.
		/// </summary>
		/// <returns></returns>
		public static ResponsivenessSingleton GetInstance()
		{
			if (_instance == null)
			{
				_instance = new ResponsivenessSingleton();
			}

			return _instance;
		}

	}
}
