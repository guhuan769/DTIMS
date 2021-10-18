using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Inphase.CTQS.Query
{
   //Ä£°åÁÐ

   public class ImageButtonTemplate : ITemplate
   {
      public ImageButtonTemplate()
      {

      }

      public void InstantiateIn(Control container)
      {
         System.Web.UI.WebControls.Image lb = new System.Web.UI.WebControls.Image();
         lb.ImageUrl = "~/Images/Common/view.gif";
         container.Controls.Add(lb);
      }

   }

   public class CheckBoxTemplate : ITemplate
   {
      public CheckBoxTemplate()
      {


      }

      public void InstantiateIn(Control container)
      {
         System.Web.UI.WebControls.CheckBox cb = new CheckBox();
         cb.ID = new Random().Next().ToString();
         container.Controls.Add(cb);
      }

   }
}
