

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Resources;

namespace Sys.Project.Common
{

   /// <summary>
   /// 将对aspx的请求定位到约定的类
   /// </summary>
   public class AspxMapping : System.Web.UI.Page, System.Web.SessionState.IRequiresSessionState 
   {
      /// <summary>
      /// 入口
      /// </summary>
      protected override void FrameworkInitialize() {
         MappingAspx(Request.Path.Substring(Request.ApplicationPath.Length));
      }

      /// <summary>
      /// 处理用户请求的ASPX页
      /// </summary>
      /// <param name="path">表示相对Application的URL</param>
      private void MappingAspx(String path) 
      {
         String[] aname = GetAName(path);

         try 
         {
            Object o = Activator.CreateInstance(aname[0], aname[1]);
            o = ((System.Runtime.Remoting.ObjectHandle)o).Unwrap();
         
            // 处理用户的请求
            ((System.Web.UI.Page)o).ProcessRequest(Context);
         } 
         catch (Exception e) 
         {
            Response.Write(e.StackTrace);
         }
      }

      /// <summary>
      /// 获取指定URL对应的程序集名和类名
      /// </summary>
      /// <param name="path">相对Application的URL</param>
      /// <returns>程序集名:类名</returns>
      private String[] GetAName(String path) 
      {
         String[] rt = null;

         String res = Request.PhysicalApplicationPath + @"bin\aspx.resources";

         try 
         {
            ResourceSet reader = new ResourceSet(res);
            String v = reader.GetString(path.ToLower());
            reader.Close();

            int idx = v.IndexOf(":");
            if (idx < 0) return rt;

            rt = new String[2];

            rt[0] = v.Substring(0, idx);
            rt[1] = v.Substring(idx + 1);
         } 
         catch (Exception e) 
         {
            Response.Write(e.StackTrace);
         }

         return rt;
      }

   }
}
