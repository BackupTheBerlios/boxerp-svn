using System;

namespace Boxerp.Facade
{
	public class FacadeCacheTest
	{
		public FacadeCacheTest(){}
		public static void Main()
		{
            FacadeCache cache = new FacadeCache(4);
            Object obj1 = new Object();
            cache.SetCacheData(obj1, "obj1");
            Object obj2 = new Object();
            cache.SetCacheData(obj2, "obj2");
            Object obj3 = new Object();
            cache.SetCacheData(obj3, "obj3");
            Object obj4 = new Object();
            cache.SetCacheData(obj4, "obj4");
            Object obj5 = new Object();
            cache.SetCacheData(obj5, "obj5");
            Object obj6 = new Object();
            cache.SetCacheData(obj6, "obj6");

            bool aux = cache.IsInCache("obj3");

            Console.WriteLine ("-{0}-",cache.Capacity);
            //Console.WriteLine ("1:-{0}-",cache.GetCacheData("obj1"));
            Console.WriteLine ("2:-{0}-",cache.GetCacheData("obj2"));
            Console.WriteLine ("3:-{0}-",cache.GetCacheData("obj3"));
            Console.WriteLine ("4:-{0}-",cache.GetCacheData("obj4"));
            //Console.WriteLine ("5:-{0}-",cache.GetCacheData("obj5"));
            Console.WriteLine ("6:-{0}-",cache.GetCacheData("obj6"));
        }
    }
}
