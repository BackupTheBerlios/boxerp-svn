//
// FacadeCache.cs
//
// Authors:
// 	Carlos Ble Jurado <carlosble@shidix.com>
// 	Zebenzui Perez Ramos <zebenperez@shidix.com>
//
// Copyright (C) 2005,2006 Shidix Technologies (www.shidix.com)
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
using System.Collections;

namespace Boxerp.Facade
{

	public struct CachePage					// Facade Cache is an ArrayList of CachePages.
	{
		public DateTime lastHit;	// Last time page was hit.
		public int hitsNumber;		
		public string key;			// Every page is identified by a key
		public int index;				// and has a position inside the ArrayList.
		public object data;			// Cache can allocate all kind of objects.
	}
	//////////////////////////////
	
	struct CacheStatus
	{
		
	}
	/////////////////////////////
	
	public enum CacheReplacement
	{
		LRU,      		// Least Recently used
		LFU				// Least Frequently used
	}
	////////////////////////////
			///<summary>
			///Exeption to raise when trying to get inexistent data
			///</summary>
	public class NullCachePageException : NullReferenceException
	{
		public NullCachePageException()
			: base ("Data doesnt exist at cache")
		{
		}

		public NullCachePageException(string Msg)
			: base (Msg)
		{
		}

		public NullCachePageException(string Msg, System.Exception e)
			: base (Msg, e)
		{
		}
	}
	/////////////////////////////
			///<summary>
			///FacadeCache is a class to store objects in memory to improve system performance.
			///</summary>
	public class FacadeCache
	{
		Hashtable dataIndex;				// key pointers to arraylist indexes. 
		ArrayList dataCache;				// Cache 
		int capacity;
		int count;
		CacheStatus status;
		CacheReplacement algorithm;	
		//////////////////////////////////////////////////////////////////////
				///<summary>
				///Number of pages in cache
				///</summary>
		public int Count 
		{
			get {return count;}
			set {count = value;}
		}
		//////////////////////////////////////////////////////////////////////
				//Maximun size of cache
		public int Capacity
		{
			get {return capacity;}
			set {capacity = value;}
		}
		//////////////////////////////////////////////////////////////////////
				//Define howto swap data from cache to database
		public CacheReplacement Algorithm
		{
			get {return algorithm;}
			set {algorithm = value;}
		}
		//////////////////////////////////////////////////////////////////////
				///<summary>
				///Constructor: Set the initial capacity (number of pages) of cache
				///</summary>
		public FacadeCache(int capacity)
		{
			try
			{
				dataIndex = new Hashtable(capacity);
				dataCache = new ArrayList(capacity);
				this.capacity = capacity;
				count = 0;
				algorithm = CacheReplacement.LFU;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//////////////////////////////////////////////////////////////////////

		private void Reallocate(CachePage page, string key, int index)
		{
			if (algorithm == CacheReplacement.LFU)
			{
				bool firstpos = true;
				CachePage tmp;
				while(index > 0)
				{
					tmp = (CachePage)dataCache[index];
					if (tmp.hitsNumber >= page.hitsNumber)
					{
						dataCache.Insert(index+1, page);
						dataIndex[key] = index +1;
						firstpos = false;
						break;
					}
					else
						index --;
				}
				if (firstpos)
				{
					dataCache.Insert(0, page);
					dataIndex[key] = 0;
				}
			}
			else if (algorithm == CacheReplacement.LRU)
			{
				dataCache.Insert(0, page);
				dataIndex[key] = 0;
			}
		}
		///////////////////////////////////////////////////////////////////////

		/*private void Insert(CachePage page, string key)
		{
			bool lastpos;
			if (algorithm == CacheReplacement.LFU)
			{
				IEnumerator cacheEnumerator = dataCache.GetEnumerator(); 
				CachePage iterPage;
				lastpos = true;
				while (cacheEnumerator.MoveNext())
				{
					iterPage = (CachePage)cacheEnumerator.Current;
					if (iterPage.hitsNumber < page.hitsNumber)
					{
						int index = dataCache.IndexOf(iterPage);
						dataCache.Insert(index, page);
						dataIndex[key] = index;
						lastpos = false;
						break;
					}
				}
				if (lastpos)
				{
					dataCache.Insert(this.Count, page);
					dataIndex[key] = this.Count;
				}
			}
			else
			{
				dataCache.Insert(0, page);
				dataIndex[key] = 0;
			}
			this.Count++;
		}*/
		///////////////////////////////////////////////////////////////////////

		private void Insert(CachePage page, string key)
        {
            // Si la cache esta llena. Tenemos que reemplazar una pagina
            // Se deberia avisar del FALLO DE PAGINA
		    if (this.Count >= this.Capacity)
            {
                CachePage pageToReplace = (CachePage) dataCache[0];
                foreach (CachePage currentPage in dataCache)
                {
                    // Buscamos la menos usada
			        if (algorithm == CacheReplacement.LFU)
                    {
                        if (pageToReplace.hitsNumber > currentPage.hitsNumber)
                            pageToReplace = currentPage;
                    }
                    // Buscamos la mas vieja
                    else if (algorithm == CacheReplacement.LRU)
                    {
                        if (pageToReplace.lastHit > currentPage.lastHit)
                            pageToReplace = currentPage;
                    }
                }    
			    // La reemplazamos	
				int index = dataCache.IndexOf(pageToReplace);
                dataCache.Insert(index, page);
				dataIndex[key] = index;
                // Eliminamos el indice 
                dataIndex.Remove(pageToReplace.key);
            }
            else
            {
				dataCache.Insert(this.Count, page);
				dataIndex[key] = this.Count++;
            }
        }
		///////////////////////////////////////////////////////////////////////
		
		public bool IsInCache(string key)
		{
			return dataIndex.ContainsKey(key);
		}
		//////////////////////////////////////////////////////////////////////
				///<summary>
				///Set data in cache. If data already exist in cache, only refresh hit date and hits number
				///and reallocate. If data doesnt exist, and cache is not full insert new data else if 
				///cache is full overwrite cache data, replacing  with apropiate algorithm
				///</summary>
		/*public void SetCacheData(object data, string key)
		{
			CachePage page;
			
			if (dataIndex.ContainsKey(key))			// Update cache hits
			{
				int index = (int) dataIndex[key];
				page = (CachePage) dataCache[index];
				dataCache.Remove(index);
				dataIndex.Remove(key);
				page.lastHit = DateTime.Now;
				page.hitsNumber ++;
				page.index = -1;
				page.data = data;
				Reallocate(page, key, index);
			}
			else												// Add to cache
			{
				if (this.Count <= this.Capacity)
				{
					page = new CachePage();
					page.lastHit = DateTime.Now;
					page.hitsNumber = 1;
					page.key = key;
					page.index = -1;
					page.data = data;
					Insert(page, key);
                    Console.WriteLine("a-{0}..{1}",this.Count,this.Capacity);
				}
				else 											// Cache is full, need to replace page
				{
					if (algorithm == CacheReplacement.LFU)
					{

					}
					else if (algorithm == CacheReplacement.LRU)
					{

					}
				}
			}
		}*/
		////////////////////////////////////////////////////////////////////////
		public bool SetCacheData(object data, string key)
		{
			CachePage page;
			
			if (dataIndex.ContainsKey(key))			// Update cache hits
			{
				int index = (int) dataIndex[key];
				page = (CachePage) dataCache[index];
				page.lastHit = DateTime.Now;
				page.hitsNumber ++;
				page.data = data;
				return true;
			}
			else									// Add to cache
			{
				page = new CachePage();
				page.lastHit = DateTime.Now;
				page.hitsNumber = 1;
				page.key = key;
				page.index = -1;
				page.data = data;
				Insert(page, key);
				return false;
			}
		}
		////////////////////////////////////////////////////////////////////////
				///<summary>
				///Gets data from cache page indexed by its key
				///</summary>
		public object GetCacheData(string key)
		{
			if (dataIndex.ContainsKey(key))
			{
            Console.WriteLine("GET:{0}",key);
				int index = (int) dataIndex[key];
				CachePage page = (CachePage) dataCache[index];
				page.lastHit = DateTime.Now;
				page.hitsNumber ++;
				dataIndex.Remove(key);
				dataCache.Remove(index);
				Reallocate(page, key, index);
				return page.data;
			}
			else
			{
				NullCachePageException ex = new NullCachePageException();
				throw ex;
			}
		}
		/////////////////////////////////////////////////////////////////////////
		
	}
}
