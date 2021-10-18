/*
 * @(#)XDictionary.cs    1.0, Mar 31, 2005
 * @author feng xianwen.
 * 字典集合，可用作保存键值对对象，键也可是对象，值也可为对象保存
 * Dictionary
 *
 */
using System;
using System.Collections;

namespace BJ.WebTools
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
