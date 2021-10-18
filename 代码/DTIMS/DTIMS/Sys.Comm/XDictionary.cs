
using System;
using System.Collections;

namespace Sys.Comm.WebTools
{
	/// <summary>
	/// Dictionary，键值对的有序集合。
	/// </summary>
	public class XDictionary  
	{
      
      private ArrayList mKeys;
      private ArrayList mValues;

      public ArrayList Keys 
      {
         get 
         {
            return mKeys;
         }
      }

      public Int32 Count 
      {
         get 
         {
            return mKeys.Count;
         }
      }

      public void Add(Object k, Object v) 
      {
         mKeys.Add(k);
         mValues.Add(v);
      }

      public void Insert(Int32 idx, Object k, Object v) 
      {
         mKeys.Insert(idx, k);
         mValues.Insert(idx, v);
      }

      public Object this[Object key] 
      {
         get 
         {
            int idx = mKeys.IndexOf(key);
            if (idx < 0) return null;
            return mValues[idx];
         }
      }
      
      public XDictionary()
		{
         mKeys = new ArrayList();
         mValues = new ArrayList();
		}

      public XDictionary(params object[] args) : this()
      {
         if (args.Length%2 == 1) 
         {
            throw new Exception("参数个数不对，应该成对（key, value）设置参数！");
         }

         for (int i=0; i<args.Length; ) 
         {
            mKeys.Add(args[i++]);
            mValues.Add(args[i++]);
         }
      }

	}
}
