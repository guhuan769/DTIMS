/*
 * @(#)DataGridFilter.cs, Mon Jul 4 15:53:04 UTC+0800 2005.
 * ������Դ�ļ������ṩǰƥ����˺;�ȷ��������Ч��
 * @author feng xianwen.
 *
 *
 */
using System;
using System.Text;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace BJ.WebTools
{
	/// <summary>
	/// DataGridFilter ��ժҪ˵����
	/// </summary>
	public class DataGridFilter
	{

      #region ��Ա����
      private DataGrid mGrid;
      private Int32 mRowIndex = 2;
      private Boolean mAllowFilter = false;
		private string mFilterString = null;
		private Boolean mAllFilter   = true;
		private Boolean mPreciseFilter = false;
      private ArrayList mFilterColumns = new ArrayList();

		private bool IsInsertRows = false;
		private Int32 mDataRowCount = 0;			//��������ʾ��ȫ����¼����

//      private Boolean mEventBinded = false;
      #endregion

      #region ����
		public Int32 DataRowCount
		{
			get
			{
				return this.mDataRowCount;
			}
			set
			{
				if(value >= 0)
				{
					this.mDataRowCount = value;
				}
				else
				{
					throw(new Exception("��������������С��0!"));
				}
			}
		}

		public Boolean PreciseFilter
		{
			get
			{
				return mPreciseFilter;
			}
			set
			{
				mPreciseFilter = value;
			}
		}

		public Boolean AllFilter
		{
			get
			{
				return mAllFilter;
			}
			set
			{
				mAllFilter = value;
			}
		}

		public string FilterString
		{
			get
			{
				return mFilterString;
			}
		}
      /// <summary>
      /// ��ȡ�������Ƿ������ DataGrid ���� Filter��
      /// ͬʱʹ�����нԿɹ��ˡ�
      /// </summary>
      public Boolean AllowFilter
      {
         get
         {
            return this.mAllowFilter;
         }
         set
         {
            this.FilterColumns = null;

            this.mAllowFilter = value;

//            if (this.AllowFilter)
//            {
//               if (!this.mEventBinded)
//               {
//                  // ��
//                  if (DataGridFilter.IsFilterBack(Request))
//                  {
//                     this.Grid.PreRender += new System.EventHandler(this.Filter);
//                  }
//                  this.Grid.PreRender += new System.EventHandler(this.BindFilter);
//
//                  this.mEventBinded = true;
//               }
//            }
//            else
//            {
//               if (this.mEventBinded)
//               {
//                  // ���
//                  if (DataGridFilter.IsFilterBack(Request))
//                  {
//                     this.Grid.PreRender -= new System.EventHandler(this.Filter);
//                  }
//                  this.Grid.Page.PreRender -= new System.EventHandler(this.BindFilter);
//
//                  this.mEventBinded = false;
//               }
//            }
         }
      }

      /// <summary>
      /// ��ȡ������ DataGrid ��Ҫ���˵��С�
      /// ���û��Ԫ�ع��������С�
      /// </summary>
      /// <param name="colNames">Ҫ���˵��е����Ƽ���</param>
      private ArrayList FilterColumns
      {
         get
         {
            return this.mFilterColumns;
         }
         set
         {
            this.mFilterColumns.Clear();

            if (value != null)
            {
               this.mFilterColumns.AddRange(value);
            }
         }
      }

      /// <summary>
      /// ��ȡ������ HttpRequest
      /// </summary>
      public HttpRequest Request
      {
         get
         {
            return this.mGrid.Page.Request;
         }
      }

      /// <summary>
      /// Ҫ���˵� DataGrid��
      /// </summary>
      public DataGrid Grid
      {
         get
         {
            return this.mGrid;
         }
         set
         {
            this.mGrid = value;
         }
      }

      /// <summary>
      /// �����������ɵ� TABLE �ϵ���������
      /// </summary>
      public Int32 RowIndex
      {
         get
         {
            return this.mRowIndex;
         }
         set
         {
            this.mRowIndex = value;
         }
      }

      private Boolean ShowFilter 
      {
         get 
         {
            String showFilter = HtmlUtil.form(this.Grid.Page.Request, "_showFilter");
            return showFilter.Equals("1");
         }
      }
      #endregion

      #region ��̬����
      /// <summary>
      /// ���� DataView �����µ� DataTable��
      /// </summary>
      public static DataTable DataView2DataTable(DataView dv)
      {
         DataTable dt = new DataTable();

         for (int i=0; i<dv.Table.Columns.Count; i++)
         {
            dt.Columns.Add(dv.Table.Columns[i].ColumnName);
         }

         foreach (DataRowView drv in dv)
         {
            dt.Rows.Add(drv.Row.ItemArray);
         }

         return dt;
      }
      #endregion

      #region ���췽��
      /// <summary>
      /// ָ��Ҫ���˵� DataGrid��
      /// Ĭ�� rowIndex �� 2��
      /// </summary>
      /// <param name="grid"></param>
		public DataGridFilter(DataGrid grid)
		{
			if (grid == null)
			{
				throw new Exception("����ָ�� DataGrid��");
			}
			
			this.Grid = grid;	
			if (!(this.Grid.AllowPaging))
			{
				this.RowIndex = 1;
			}
		}

      /// <summary>
      /// ָ��Ҫ���˵� DataGrid �Լ������������ɵ� TABLE �ϵ���һ�С�
      /// </summary>
      /// <param name="grid"></param>
      /// <param name="rowIndex"></param>
      public DataGridFilter(DataGrid grid, Int32 rowIndex)
      {
         if (grid == null)
         {
            throw new Exception("����ָ�� DataGrid��");
         }
         if (rowIndex < 0)
         {
            throw new Exception("����������������Ӧ��С�� 0��");
         }


         this.RowIndex = rowIndex;
      }

      #endregion

      #region ����ʵ������
      /// <summary>
      /// �ж������Ƿ��������Ĺ��˲����ط��ġ�
      /// </summary>
      public Boolean IsFilterBack()
      {
         if (!this.Grid.Page.IsPostBack)
         {
            return true;
         }

         String filterBack = this.Grid.Page.Request.Form["_FilterBack"];
         return filterBack == "1";
      }

      /// <summary>
      /// �����Ҫ���˵��е�����
      /// </summary>
      /// <param name="columnIdx">����</param>
      public void AddColumn(String columnName)
      {
         this.FilterColumns.Add(new ColumnObject(columnName.Trim() , 0));
      }

		/// <summary>
		/// �����Ҫ���˵��е�����
		/// </summary>
		/// <param name="columnIdx">����</param>
		public void AddColumn(String columnName , Int32 filterType)
		{
			this.FilterColumns.Add(new ColumnObject(columnName.Trim() , filterType));
		}

		/// <summary>
		/// �����Ҫ���˵�������
		/// </summary>
		/// <param name="columnName">�����ƣ���DataGrid����һ��</param>
		/// <param name="fieldString">��SQL�е������ƣ�����SQL��ѯ</param>
		public void AddColumn(String columnName , String fieldString)
		{
			this.FilterColumns.Add(new ColumnObject(columnName.Trim() , fieldString));
		}

		/// <summary>
		/// �����Ҫ���˵�������
		/// </summary>
		/// <param name="columnName">�ֶ�����</param>
		/// <param name="type">�ֶ�����</param>
		public void AddColumn(String columnName , DbType type)
		{
			this.FilterColumns.Add(new ColumnObject(columnName.Trim() , 0 , type));
		}

		/// <summary>
		/// �����Ҫ���˵�������
		/// </summary>
		/// <param name="columnName">�ֶ�����</param>
		/// <param name="filterType">���˷�ʽ</param>
		/// <param name="type">�ֶ�����</param>
		public void AddColumn(String columnName , Int32 filterType , DbType type)
		{
			this.FilterColumns.Add(new ColumnObject(columnName.Trim() , filterType , type));
		}

		/// <summary>
		/// �����Ҫ���˵�������
		/// </summary>
		/// <param name="columnName">�ֶ�����</param>
		/// <param name="fieldString">��SQL�е������ƣ�����SQL��ѯ</param>
		/// <param name="type">�ֶ�����</param>
		public void AddColumn(String columnName , String fieldString , DbType type)
		{
			this.FilterColumns.Add(new ColumnObject(columnName.Trim() , fieldString , type));
		}

      /// <summary>
      /// �Ƴ���Ҫ���˵��е�����
      /// </summary>
      /// <param name="columnName">����</param>
      public void RemoveColumn(String columnName)
      {
         this.FilterColumns.Remove(new ColumnObject(columnName.Trim() , 0));
      }

      /// <summary>
      /// ���� DataGrid��
      /// </summary>
      public void Filter()
      {
         if (!this.ShowFilter)
         {
            BindFilter(null, null);
            return;
         }

         DataView dv = this.DataSource;

         if (dv == null)
         {
            throw new Exception(this.Grid.ID + "��δָ�� DataSource��");
         }

         DataTable dt = dv.Table;

         String filter = "";
         for (int i=0; i<dt.Columns.Count; i++)
         {
            String col = dt.Columns[i].ColumnName.ToUpper();
            String colFilter = HtmlUtil.form(Request, HtmlUtil.getAsName(col));
            colFilter = colFilter.Trim();

            if (!colFilter.Equals(""))
            {
               colFilter = colFilter.Replace("'", "''");
					ColumnObject columnObj = ColumnObject.GetObject(this.FilterColumns , col);

               if (!filter.Equals(""))
               {
                  filter += " AND ";
               }

					if(columnObj.FilterType == 2)
					{
						filter += "CONVERT([" + col + "],'System.String') = '" + colFilter + "' ";
					}
					else if(columnObj.FilterType == 0)
					{
						filter += "CONVERT([" + col + "],'System.String') LIKE '*" + colFilter + "*' ";
					}
					else if(columnObj.FilterType == 1)
					{
						filter += "CONVERT([" + col + "],'System.String') LIKE '" + colFilter + "*' ";
					}
            }
         }

         dv.RowFilter = filter;
			this.mFilterString = filter;

         Int32 ps = this.Grid.PageSize;
         Int32 pageCount = dv.Count/ps + (dv.Count%ps>0?1:0);

         if (this.Grid.CurrentPageIndex >= pageCount)
         {
            if (pageCount == 0)
            {
               this.Grid.CurrentPageIndex = 0;
            }
            else
            {
               this.Grid.CurrentPageIndex = pageCount - 1;
            }
         }

         this.Grid.DataSource = dv;
         this.Grid.DataBind();

         BindFilter(null, null);
		
		  //���˺�Ĳ������
		  if (this.IsInsertRows)
		  {
			 InsertBlankRows(dv.Count);
		  }
      }

		/// <summary>
		/// �õ���ǰ�����ַ���
		/// ֻ���õ�����sql����н��в�ѯ����
		/// </summary>
		/// <returns></returns>
		public String GetFilterString()
		{
			String filter = "";
            //Int32 index = 0;
			for(int i=0;i<this.FilterColumns.Count;i++)
			{
				ColumnObject columnObj = (ColumnObject)this.FilterColumns[i];
				String col = columnObj.ColumnName.ToUpper().Trim();
				String colFilter = HtmlUtil.form(Request, HtmlUtil.getAsName(col));
				colFilter = colFilter.Trim();

				if (!colFilter.Equals(""))
				{
					colFilter = colFilter.Replace("'", "''");
					
					if (!filter.Equals(""))
					{
						filter += " AND ";
					}

					String fieldName = columnObj.FieldString;
					if(fieldName == "" || fieldName == null)
					{
						fieldName = columnObj.ColumnName;
					}

//					bool isString = false;
//					foreach(DataColumn dc in this.Grid.Columns)
//					{
//						if(dc.ColumnName.ToUpper().Trim() == columnObj.ColumnName.ToUpper().Trim())
//						{
//							if(dc.DataType == System.Type.GetType("System.String"))
//							{
//								isString = true;
//							}
//							break;
//						}
//					}

					if(columnObj.FieldDbType == DbType.String)
					{
						if(columnObj.FilterType == 2)
						{
							filter += fieldName + " = '" + colFilter + "' ";
						}
						else if(columnObj.FilterType == 0)
						{
							filter += fieldName + " LIKE '%" + colFilter + "%' ";
						}
						else if(columnObj.FilterType == 1)
						{
							filter += fieldName + " LIKE '" + colFilter + "%' ";
						}
					}
					else
					{
						if(columnObj.FilterType == 2)
						{
							filter += "convert(Varchar(255) , " + fieldName + ") = '" + colFilter + "' ";
						}
						else if(columnObj.FilterType == 0)
						{
							filter += "convert(Varchar(255) , " + fieldName + ") LIKE '%" + colFilter + "%' ";
						}
						else if(columnObj.FilterType == 1)
						{
							filter += "convert(Varchar(255) , " + fieldName + ") LIKE '" + colFilter + "%' ";
						}
					}
				}
			}

			return filter;
		}

		/// <summary>
		/// �ڵ�ǰҳ����������ʱ����ӿ���
		/// </summary>
		/// <param name="count">���м�¼����</param>
		public void InsertBlankRows(int count)
		{
			InsertCurrentBlankRows(count , 0 , null);
		}

		/// <summary>
		/// �ڵ�ǰҳ����������ʱ����ӿ���
		/// </summary>
		/// <param name="count">���м�¼����</param>
		/// <param name="rowheight">�и�</param>
		/// <param name="attribute">������</param>
		public void InsertBlankRows(int count , int rowheight , String attribute)
		{
			if(rowheight < 0)
			{
				throw(new Exception("�и߲�����С��0������!"));
			}
			InsertCurrentBlankRows(count , rowheight , attribute);
		}

		/// <summary>
		/// �������ܣ��ڵ�ǰҳ����������ʱ����ӿ��У�
		/// </summary>
		/// <param name="count">���м�¼����</param>
		/// <param name="attribute"></param>
		private void InsertCurrentBlankRows(int count , int rowheight , String attribute)
		{
			
			if((this.Grid.CurrentPageIndex == count / this.Grid.PageSize) || (count == 0))
			{
				this.IsInsertRows = true;
				StringBuilder sb1 = new StringBuilder();
				int currCount  = 0;
				if (this.Grid.AllowPaging == true)
				{
					currCount =  count % this.Grid.PageSize;
				}
				else
				{
					currCount = count;
				}
			
				int rows = this.Grid.PageSize-currCount;
				if (!(this.Grid.AllowPaging))
				{
					rows ++;
				}

				//��ȡ�����е�������������-���ص�����
				int currCols = this.Grid.Columns.Count - this.GetShadowColumns();
				int insertIndex = this.RowIndex + currCount + 1;
				if (currCount < this.Grid.PageSize )
				{
					sb1.Append("<script>\n");	
					sb1.Append("var strBlankRow = ''; ");
					sb1.Append(" var ar = new Array();");
					
					for (int j=0;j<rows;j++)
					{
						if(rowheight > 0 || ((attribute != null) && (attribute != "")))
						{
							if(rowheight > 0)
							{
								sb1.Append("strBlankRow += '<tr height=" + rowheight + "';");
							}
							if(attribute != null)
							{
								sb1.Append("strBlankRow += ' " + attribute + "';");
							}
							sb1.Append("strBlankRow += '>';");
						}
						else
						{
							sb1.Append("strBlankRow += '<tr>';");
						}
						for (int i=0;i<currCols ;i++)
						{
							sb1.Append("strBlankRow += '<td>&nbsp;</td>';");
						}
						sb1.Append("strBlankRow +='</tr>';");
						sb1.Append("ar[" + j + "] = strBlankRow;");
						sb1.Append("  insertRow2Table(" + this.Grid.ID + ", ar[" + j + "], " + insertIndex + ");\n");	
						sb1.Append("strBlankRow ='';");
					}	

					sb1.Append("</script>\n");
					this.Grid.Page.ClientScript.RegisterStartupScript(this.GetType(), "insert", sb1.ToString());
				}	
			}
		}


		/// <summary>
		/// 		//ͳ��ҳ�����ص���
		/// </summary>
		/// <returns></returns>
		int GetShadowColumns()
		{
			int j = 0;
			for (int i=0;i<this.Grid.Columns.Count ;i++)
			{
				if(this.Grid.Columns[i].Visible == false)
				{
					j++;
				}
			}
			return j;
		}
      #endregion

      #region ˽��ʵ������
      /// <summary>
      /// Ϊ DataGrid �� Filter��
      /// </summary>
      private void BindFilter(object sender, System.EventArgs e)
      {
         if (this.Grid.Controls.Count <= 0)
         {
            throw new Exception(this.Grid.ID + " û�����ݡ�");
         }

         Table t = (Table)this.Grid.Controls[0];
         if (t.Rows.Count < this.RowIndex + 1)
         {
            throw new Exception("����������������Ӧ�ô��������������");
         }

         StringBuilder sb = new StringBuilder();
         sb.Append("<script>\n");

         String showFilter = HtmlUtil.form(Request, "_showFilter");

         if (!this.Grid.Page.IsPostBack)
         {
            showFilter = "1";
         }

         if (showFilter.Equals("1"))
         {
            sb.Append("var sTr = '<tr ");
         }
         else
         {
            sb.Append("var sTr = '<tr style=\"display:none\" ");
         }

         sb.Append("id=\"_MyFilterTr\" class=exclude title=\"�����������\">");
         sb.Append("';\n");

         DataView dv = this.DataSource;
         if (dv == null)
         {
            throw new Exception(this.Grid.ID + "��δָ�� DataSource��");
         }

         DataTable data = dv.Table;

         DataGridColumnCollection dgcc = this.Grid.Columns;
         TableCellCollection tcc = t.Rows[this.RowIndex].Cells;
         Int32 count = tcc.Count;


         if (!this.Grid.AutoGenerateColumns)
         {
            count = dgcc.Count;
         }

         for (int i=0; i<count; i++)
         {
            if (!this.Grid.AutoGenerateColumns)
            {
               if (!dgcc[i].Visible)
               {
                  continue;
               }
            }
            else
            {
               if (!tcc[i].Visible)
               {
                  continue;
               }
            }

            sb.Append("sTr += \"<td style='padding:0px;'>");

            Boolean canFilter = true;
            String colName = "";

            if (!this.Grid.AutoGenerateColumns)
            {
               if (dgcc[i].GetType().Name.Equals("BoundColumn"))
               {
                  colName = ((BoundColumn)dgcc[i]).DataField;
               } 
               else if (dgcc[i].GetType().Name.Equals("TemplateColumn"))
               {
                  colName = ((TemplateColumn)dgcc[i]).SortExpression;
               }
            }
            else
            {
               colName = data.Columns[i].ColumnName;
            }
            colName = colName.ToUpper();

            if (this.FilterColumns != null && this.FilterColumns.Count > 0)
            {
					if(ColumnObject.GetObject(this.FilterColumns , colName) != null)
					{
						canFilter = true;
					}
					else
					{
						canFilter = false;
					}
            }

            if (canFilter)
            {
               sb.Append("<input class=inputField type=text style='width:100%;height:100%;' ");
               String name = HtmlUtil.getAsName(colName);
               String val = HtmlUtil.form(Request, name);
               val = val.Trim();
               if (!val.Equals(""))
               {
                  sb.Append("value='" + HtmlUtil.getAsValue(val) + "' ");
               }
               sb.Append("name='" + name + "' onkeypress='__doFilter()'>");
            }

            sb.Append("</td>\";\n");
         }
         sb.Append("sTr += '</tr>';\n");
			sb.Append("if(typeof(document.getElementById('_MyFilterTr')) == 'undefined' || document.getElementById('_MyFilterTr') == null){ \n");
         sb.Append("	insertRow2Table(" + this.Grid.ID + ", sTr, " + this.RowIndex + ");\n");
			sb.Append("}\n");

			String pageLabel = "ҳ�룺";
			String countLabel = String.Format("�� <font color=blue>{0}</font> ��", ""+dv.Count);
			if(this.DataRowCount > 0)
			{
				countLabel = String.Format("�� <font color=blue>{0}</font> ��", ""+this.DataRowCount);
			}
			sb.Append("var r = " + this.Grid.ID + ".rows;\n");
		  //����ҳ�룬�ͼ�¼����ʾ
		  if (this.Grid.AllowPaging)
		  {

			  //ҳü

			  sb.Append("var c = r[0].cells[0];\n");
			  sb.Append("var oldC = c.innerHTML;\n");

			  sb.Append("var tmp = '<table cellpadding=0 cellspacing=0 border=0 width=100% id=\"_MyFilterHeaderUp\">';\n");
			  sb.Append("tmp += '<tr class=exclude><td>" + pageLabel + "' + oldC;\n");
			  sb.Append("tmp += '</td><td align=right>';\n");
		  

			  sb.Append("tmp += '<input type=hidden name=_FilterBack>';\n");
			  sb.Append("tmp += '<input type=hidden name=_showFilter");
			  if (showFilter.Equals("1"))
			  {
				  sb.Append(" value=1");
			  }
			  sb.Append(">';\n");

			  sb.Append("tmp += '<img id=imgfilter title=\"��ʾ������\" onmousedown=\"__changeFilter()\" src=\"" + Request.ApplicationPath);
			  if (showFilter.Equals("1"))
			  {
				  sb.Append("/images/filter/filterdown.gif\">'\n");
			  }
			  else
			  {
				  sb.Append("/images/filter/filter.gif\">'\n");
			  }

			  sb.Append("tmp += '<img onmousedown=\"__clearFilter();\" onmouseup=\"this.src=clear1;\" ");
			  sb.Append("onmouseout=\"this.src=clear1;\" '\n");
			  sb.Append("tmp += 'title=\"�����������\" src=\"" + Request.ApplicationPath + "/images/filter/clear.gif\">'\n");

			  sb.Append("tmp += '</td></tr></table>';\n");
			  sb.Append("if(typeof(document.getElementById('_MyFilterHeaderUp')) == 'undefined' || document.getElementById('_MyFilterHeaderUp') == null){ \n");
			  sb.Append("c.innerHTML = tmp;\n");
			  sb.Append("}\n");
			  //ҳ��
			  sb.Append("c = r[r.length-1].cells[0];\n");
			  sb.Append("oldC = c.innerHTML;\n");
			  sb.Append("tmp = '<table cellpadding=0 cellspacing=0 border=0 width=100% id=\"_MyFilterHeaderDn\">';\n");
			  sb.Append("tmp += '<tr class=exclude><td>" + pageLabel + "' + oldC;\n");
			  sb.Append("tmp += '</td><td align=right>" + countLabel + "</td></tr></table>';\n");
			  sb.Append("if(typeof(document.getElementById('_MyFilterHeaderDn')) == 'undefined' || document.getElementById('_MyFilterHeaderDn') == null){ \n");
			  sb.Append("c.innerHTML = tmp;\n");
			  sb.Append("}\n");
		  }
		  else
		  {
			  sb.Append(" var tmp = '<tr class=exclude><td colSpan= " + this.Grid.Columns.Count + "';\n");
			  sb.Append("tmp += ' align=right>" + countLabel + "</td></tr>';\n");
			  
			  sb.Append("insertRow2Table(" + this.Grid.ID + ", tmp, r.length);\n");
		
		  }


		  

		  //����
         sb.Append("\nvar clear1 = '" + Request.ApplicationPath + "/images/filter/clear.gif';");
         sb.Append("\nvar clear2 = '" + Request.ApplicationPath + "/images/filter/cleardown.gif';");
         sb.Append("\nvar filter1 = '" + Request.ApplicationPath + "/images/filter/filter.gif';");
         sb.Append("\nvar filter2 = '" + Request.ApplicationPath + "/images/filter/filterdown.gif';");
         sb.Append("\n");
         sb.Append("\nvar _myForm = document.forms[0];");
         sb.Append("\n\n");

			//ʼ�ձ��ֹ���״̬
			sb.Append("if( _myForm._showFilter.value == ''){\n");
			sb.Append("document.getElementById('imgfilter').src = filter2;\n");
			sb.Append("_MyFilterTr.style.display = 'block';\n");
			sb.Append("_myForm._showFilter.value = '1';\n");
			sb.Append("}\n");

         sb.Append("\nfunction __doFilter()");
         sb.Append("\n{");
         sb.Append("\n    if (event.keyCode != 13) ");
         sb.Append("\n    {");
         sb.Append("\n        return;");
         sb.Append("\n    }");
         sb.Append("\n    window.event.cancelBubble = true;");
         sb.Append("\n    window.event.returnValue = false;");
         sb.Append("\n    _myForm._FilterBack.value = '1';");
         sb.Append("\n    _myForm.submit();");
         sb.Append("\n}");
         sb.Append("\n");
         sb.Append("\nfunction __clearFilter()");
         sb.Append("\n{");
         sb.Append("\n    try { event.srcElement.src = clear2; } catch (e) {}");
         sb.Append("\n    ");
         sb.Append("\n    var cleared = false;");
         sb.Append("\n    ");
         sb.Append("\n    var nodes = _MyFilterTr.cells;");
         sb.Append("\n    for (var i=0; i<nodes.length; i++) {");
         sb.Append("\n        var c= nodes[i].childNodes;");
         sb.Append("\n        if (typeof(c) != 'undefined' && c.length > 0) ");
         sb.Append("\n        {");
         sb.Append("\n            if (c[0].nodeName.toUpperCase() == 'INPUT') ");
         sb.Append("\n            {");
         sb.Append("\n                if (c[0].value != '') ");
         sb.Append("\n                {");
         sb.Append("\n                    c[0].value = '';");
         sb.Append("\n                    cleared = true;");
         sb.Append("\n                }");
         sb.Append("\n            }");
         sb.Append("\n        }");
         sb.Append("\n    }");
         sb.Append("\n    ");
         sb.Append("\n    if (cleared) { _myForm._FilterBack.value='1'; _myForm.submit(); } ");
         sb.Append("\n}");
         sb.Append("\n");
         sb.Append("\nfunction __hasFilterValue() ");
         sb.Append("\n{");
         sb.Append("\n    var nodes = _MyFilterTr.cells;");
         sb.Append("\n    for (var i=0; i<nodes.length; i++) {");
         sb.Append("\n        var c= nodes[i].childNodes;");
         sb.Append("\n        if (typeof(c) != 'undefined' && c.length > 0) ");
         sb.Append("\n        {");
         sb.Append("\n            if (c[0].nodeName.toUpperCase() == 'INPUT') ");
         sb.Append("\n            {");
         sb.Append("\n                if (c[0].value != '') ");
         sb.Append("\n                {");
         sb.Append("\n                    return true;");
         sb.Append("\n                }");
         sb.Append("\n            }");
         sb.Append("\n        }");
         sb.Append("\n    }");
         sb.Append("\n    ");
         sb.Append("\n    return false;");
         sb.Append("\n}");
         sb.Append("\n");
         sb.Append("\nfunction __changeFilter()");
         sb.Append("\n{");
         sb.Append("\n    var obj = event.srcElement;");
         sb.Append("\n");
         sb.Append("\n    if (obj.src.indexOf(filter1) >= 0)");
         sb.Append("\n    {");
         sb.Append("\n        obj.src = filter2;");
         sb.Append("\n        _MyFilterTr.style.display = 'block';");
         sb.Append("\n        ");
         sb.Append("\n        _myForm._showFilter.value = '1';");
         sb.Append("\n");
         sb.Append("\n        if (__hasFilterValue()) {");
         sb.Append("\n    _myForm._FilterBack.value = '1';");
         sb.Append("\n            _myForm.submit();");
         sb.Append("\n        }");
         sb.Append("\n    }");
         sb.Append("\n    else");
         sb.Append("\n    {");
         sb.Append("\n        obj.src = filter1;");
         sb.Append("\n        _MyFilterTr.style.display = 'none';");
         sb.Append("\n");
         sb.Append("\n        _myForm._showFilter.value = '';");
         sb.Append("\n");
         sb.Append("\n        if (__hasFilterValue()) {");
         sb.Append("\n    _myForm._FilterBack.value = '1';");
         sb.Append("\n            _myForm.submit();");
         sb.Append("\n        }");
         sb.Append("\n    }");
         sb.Append("\n}\n");

         sb.Append("</script>\n");

         this.Grid.Page.ClientScript.RegisterStartupScript(this.GetType(), "", sb.ToString());
      }
//
//      private void Filter(object sender, DataGridPageChangedEventArgs e)
//      {
//         Filter();
//      }
//
//      private void Filter(object sender, DataGridSortCommandEventArgs e)
//      {
//         Filter();
//      }
//
//      private void Filter(object sender, System.EventArgs e)
//      {
//         Filter();
//      }

      /// <summary>
      /// ��ȡ DataGrid ����ͼ��
      /// </summary>
      private DataView DataSource
      {
         get
         {
            Object dv = this.Grid.DataSource;
            if (dv == null)
            {
               return null;
            }
            if (dv.GetType().Name.Equals("DataView"))
            {
               return (DataView)dv;
            }
            else if (dv.GetType().Name.Equals("DataTable"))
            {
               return ((DataTable)dv).DefaultView;
            }
            else
            {
               return null;
            }
         }
      }
      #endregion

   }

	#region �����ж���
	public class ColumnObject
	{
		private String mColumnName = null;
		private DbType mFieldDbType = DbType.Int32;	//�ֶ�����
		private String mFieldString = null;	//���˵��ֶ����ƣ��ɴ˷��ع����������Դ��ٲ�ѯ��
		private Int32 mFilterType  = 0;		//�������ͣ�����ǣ���ʾ��ȫ���ģ̣ɣˣŹ��ˣ��������ʾ��ǰƥ����ˣ����Ϊ����ʾ�Ǿ�ȷ����

		#region ��������
		public DbType FieldDbType
		{
			get
			{
				return this.mFieldDbType;
			}
		}
		public String FieldString
		{
			get
			{
				return this.mFieldString;
			}
		}
		public String ColumnName
		{
			get
			{
				return this.mColumnName;
			}
		}
		public Int32 FilterType
		{
			get
			{
				return this.mFilterType;
			}
		}
		#endregion

		public ColumnObject(String columnName , Int32 type)
		{
			this.mColumnName = columnName.Trim();
			this.mFilterType = type;
		}

		public ColumnObject(String columnName , Int32 type , DbType filedType)
		{
			this.mColumnName = columnName.Trim();
			this.mFilterType = type;
			this.mFieldDbType = filedType;
		}

		public ColumnObject(String columnName , String fieldString)
		{
			this.mColumnName = columnName.Trim();
			this.mFieldString= fieldString.Trim();
			this.mFilterType = 0;
		}

		public ColumnObject(String columnName , String fieldString , DbType filedType)
		{
			this.mColumnName = columnName.Trim();
			this.mFieldString= fieldString.Trim();
			this.mFilterType = 0;
			this.mFieldDbType = filedType;
		}

		public static ColumnObject GetObject(ArrayList columnList , String columnName)
		{
			int count = columnList.Count;
			ColumnObject columnObj = null;

			for(int i=0;i<count;i++)
			{
				if(((ColumnObject)columnList[i]).ColumnName.ToUpper() == columnName.Trim().ToUpper())
				{
					columnObj = (ColumnObject)columnList[i];break;
				}
			}

			return columnObj;
		}
		#endregion
	}
}
