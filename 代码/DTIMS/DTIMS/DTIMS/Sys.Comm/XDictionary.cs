/*
 * @(#)XDictionary.cs    1.0, Mar 31, 2005
 * @author feng xianwen.
 * �ֵ伯�ϣ������������ֵ�Զ��󣬼�Ҳ���Ƕ���ֵҲ��Ϊ���󱣴�
 * Dictionary
 *
 */
using System;
using System.Collections;

namespace BJ.WebTools
{
	/// <summary>
	/// Dictionary����ֵ�Ե����򼯺ϡ�
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
            throw new Exception("�����������ԣ�Ӧ�óɶԣ�key, value�����ò�����");
         }

         for (int i=0; i<args.Length; ) 
         {
            mKeys.Add(args[i++]);
            mValues.Add(args[i++]);
         }
      }

	}
}
