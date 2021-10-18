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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Inphase.Project.CTQS
{
	/// <summary>
	/// ExportBadBill 的摘要说明。
	/// </summary>
	public class ExportBill
	{

		private System.Web.HttpContext dsPage = null;
        //private ExportType eType;
		string fileName = null;

		private Excel.Application m_objExcel =  null;
		private Excel.Workbooks m_objBooks = null;
		private Excel._Workbook m_objBook = null;
		private Excel.Sheets m_objSheets = null;
		private Excel._Worksheet m_objSheet = null;
		private Excel.Range m_objRange =  null;
		private Excel.Font m_objFont = null;

		private object m_objOpt = System.Reflection.Missing.Value;

		private string [] cellNameArray= {"A1","B1","C1","D1","E1","F1","G1","H1","I1","J1","K1",
											"L1","M1","N1","O1","P1","Q1","R1","S1","T1","U1","V1","W1","X1","Y1","Z1",
												"AA1","AB1","AC1","AD1","AE1","AF1","AG1","AH1","AI1","AJ1","AK1","AL1",
													"AM1","AN1","AO1","AP1","AQ1","AR1","AS1","AT1","AU1","AV1","AW1","AX1","AY1","AZ1",
														"BA1","BB1","BC1","BD1","BE1","BF1","BG1","BH1","BI1","BJ1","BK1","BL1",
															"BM1","BN1","BO1","BP1","BQ1","BR1","BS1","BT1","BU1","BV1","BW1","BX1","BY1","BZ1",
																 "CA1","CB1","CC1","CD1","CE1","CF1","CG1","CH1","CI1","CJ1","CK1","CL1",
																	"CM1","CN1","CO1","CP1","CQ1","CR1","CS1","CT1","CU1","CV1","CW1","CX1","CY1","CZ1"};

		public ExportBill()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
			dsPage = System.Web.HttpContext.Current;
		}

		#region 调用过程
		public enum ExportType
		{
			simple,complex
		}

		public void DoExportAsExcel(DataTable dt,string []  objHeaders,string title)
		{
			fileName = title;

			ExportAsExcel(dt , objHeaders ,fileName);
			System.GC.Collect();
			System.GC.Collect();
			System.GC.Collect();
		}

		public void DoExportAsExcel(DataTable dt,String boardFileName , Int32 startRow , String title)
		{
			fileName = title;

			ExportAsExcel(dt , fileName , boardFileName , startRow);
			System.GC.Collect();
			System.GC.Collect();
			System.GC.Collect();
		}

		public String DoSaveAsExcel(DataTable dt,string []  objHeaders,string title)
		{
			fileName = title;

			String ret = SaveAsExcel(dt , objHeaders ,fileName);
			System.GC.Collect();
			System.GC.Collect();
			System.GC.Collect();

			return ret;
		}
		#endregion

		#region 导出Excel文件
		/// <summary>
		/// 按模板格式保存输入的数据表格
		/// </summary>
		/// <param name="dt">数据表</param>
		/// <param name="boardFileName">模板文件</param>
		/// <param name="title">导出后形成的目标文件</param>
		/// <returns></returns>
		public String DoSaveAsExcel(DataTable dt,String boardFileName , Int32 startRow , String title)
		{
			fileName = title;

			String ret = SaveAsExcel(dt , fileName , boardFileName , startRow);
			System.GC.Collect();
			System.GC.Collect();
			System.GC.Collect();

			return ret;
		}

		private String SaveAsExcel(DataTable dt,string []  objHeaders,string fileName)
		{
			int rows   = dt.Rows.Count;
			int colums = dt.Columns.Count;
			int index,count = 0;

			//保存数据的数组
			string [,] objData = new string [rows,colums];

			if(colums == 0)
			{
				throw(new Exception("导出字段不能为空！"));
			}

			//循环填充数据
			foreach(DataRow dr in dt.Rows)
			{
				index = 0;
				foreach(DataColumn dc in dt.Columns)
				{
					objData[count,index] = dr[dc.ColumnName].ToString().Trim();
					index ++;
				}
				count ++;
			}

			// Start a new workbook in Excel.
			try
			{
				m_objExcel = new Excel.Application();
				m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;
				m_objBook = (Excel._Workbook)(m_objBooks.Add(m_objOpt));
				m_objSheets = (Excel.Sheets)m_objBook.Worksheets;
				m_objSheet = (Excel._Worksheet)(m_objSheets.get_Item(1));

				m_objExcel.Visible = false;

				// Create an array for the headers and add it to cells A1:C1.
				m_objRange = m_objSheet.get_Range("A1", cellNameArray[colums-1]);
				m_objRange.Value = objHeaders;
				m_objRange.Columns.AutoFit();
				m_objFont = m_objRange.Font;
				m_objFont.Bold = true;

				// Create an array with 3 columns and 100 rows and add it to
				// the worksheet starting at cell A2.
				if(rows > 0)
				{
					m_objRange = m_objSheet.get_Range("A2", m_objOpt);
					m_objRange = m_objRange.get_Resize(rows,colums);
					m_objRange.Value = objData;
					//m_objRange.Columns.AutoFit();
				}

				m_objRange.NumberFormatLocal = "@";

				//设置格式为全部都为字符型
				m_objSheet.UsedRange.NumberFormatLocal = "@";
				m_objSheet.Columns.AutoFit();
				

				//NumberFormatLocal 

				// Save the Workbook and quit Excel.
				string path = this.dsPage .Request.MapPath(this.dsPage .Request.ApplicationPath);
				string name = Guid.NewGuid().ToString();
				string filePath = path + @"\UpLoadFile\" + fileName + ".xls";

				m_objBook.SaveAs(filePath , m_objOpt, m_objOpt,
					m_objOpt, m_objOpt, m_objOpt, Excel.XlSaveAsAccessMode.xlNoChange,
					m_objOpt, m_objOpt, m_objOpt, m_objOpt);


				m_objBook.Close(false, m_objOpt, m_objOpt);
				m_objExcel.Quit();

				return name + ".xls";
			}
			catch(Exception e)
			{
				throw(new Exception(e.Message));
			}
			finally
			{
				DoClear();
			}			
		}

		/// <summary>
		/// 把数据表格导出到EXCEL文件中
		/// </summary>
		/// <param name="dt">数据表格</param>
		/// <param name="fileName">导出的EXCEL文件名称</param>
		/// <param name="boardFileName">EXCEL文件模板名称</param>
		/// <param name="startRow">开始行</param>
		/// <returns></returns>
		private String SaveAsExcel(DataTable dt ,String fileName , String boardFileName , Int32 startRow)
		{
			int rows   = dt.Rows.Count;
			int colums = dt.Columns.Count;
			int index,count = 0;

			//保存数据的数组
			string [,] objData = new string [rows,colums];

			if(colums == 0)
			{
				throw(new Exception("导出字段不能为空！"));
			}

			//循环填充数据
			foreach(DataRow dr in dt.Rows)
			{
				index = 0;
				foreach(DataColumn dc in dt.Columns)
				{
					objData[count,index] = dr[dc.ColumnName].ToString().Trim();
					index ++;
				}
				count ++;
			}

			// Start a new workbook in Excel.
			try
			{
				m_objExcel = new Excel.Application();
				m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;
				m_objBook = (Excel._Workbook)(m_objBooks.Open( boardFileName,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing));
				m_objSheets = (Excel.Sheets)m_objBook.Worksheets;
				m_objSheet = (Excel._Worksheet)(m_objSheets.get_Item(1));
				m_objExcel.Visible = false;

//				// Create an array for the headers and add it to cells A1:C1.
//				m_objRange = m_objSheet.get_Range("A1", cellNameArray[colums-1]);
//				m_objRange.Value = objHeaders;
//				m_objRange.Columns.AutoFit();
//				m_objFont = m_objRange.Font;
//				m_objFont.Bold = true;

				// Create an array with 3 columns and 100 rows and add it to
				// the worksheet starting at cell A2.
				if(rows > 0)
				{
					m_objRange = m_objSheet.get_Range("A" + startRow.ToString(), m_objOpt);
					m_objRange = m_objRange.get_Resize(rows,colums);
					m_objRange.Value = objData;
					m_objRange.NumberFormatLocal = "@";
				}

				//设置格式为全部都为字符型
				m_objSheet.UsedRange.NumberFormatLocal = "@";
				m_objSheet.Columns.AutoFit();
				

				//NumberFormatLocal 

				// Save the Workbook and quit Excel.
				string path = this.dsPage .Request.MapPath(this.dsPage .Request.ApplicationPath);
				string name = Guid.NewGuid().ToString();
				string filePath = path + @"\Templates\" + name + ".xls";

				m_objBook.SaveAs(filePath , m_objOpt, m_objOpt,
					m_objOpt, m_objOpt, m_objOpt, Excel.XlSaveAsAccessMode.xlNoChange,
					m_objOpt, m_objOpt, m_objOpt, m_objOpt);


				m_objBook.Close(false, m_objOpt, m_objOpt);
				m_objExcel.Quit();

				return name + ".xls";
			}
			catch(Exception e)
			{
				throw(new Exception(e.Message));
			}
			finally
			{
				DoClear();
			}			
		}

		private void ExportAsExcel(DataTable dt,string []  objHeaders,string fileName)
		{
			int rows   = dt.Rows.Count;
			int colums = dt.Columns.Count;
			int index,count = 0;

			//保存数据的数组
			string [,] objData = new string [rows,colums];

//			if(rows == 0)
//			{
//				throw(new Exception("数据源不能为空！"));
//			}

			if(colums == 0)
			{
				throw(new Exception("导出字段不能为空！"));
			}

			//循环填充数据
			foreach(DataRow dr in dt.Rows)
			{
				index = 0;
				foreach(DataColumn dc in dt.Columns)
				{
					objData[count,index] = dr[dc.ColumnName].ToString().Trim();
					index ++;
				}
				count ++;
			}

			// Start a new workbook in Excel.
			try
			{
				m_objExcel = new Excel.Application();
				m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;
				m_objBook = (Excel._Workbook)(m_objBooks.Add(m_objOpt));
				m_objSheets = (Excel.Sheets)m_objBook.Worksheets;
				m_objSheet = (Excel._Worksheet)(m_objSheets.get_Item(1));

				m_objExcel.Visible = false;

				// Create an array for the headers and add it to cells A1:C1.
				m_objRange = m_objSheet.get_Range("A1", cellNameArray[colums-1]);
				m_objRange.Value = objHeaders;
				m_objRange.Columns.AutoFit();
				m_objFont = m_objRange.Font;
				m_objFont.Bold = true;

				// Create an array with 3 columns and 100 rows and add it to
				// the worksheet starting at cell A2.
				if(rows > 0)
				{
					m_objRange = m_objSheet.get_Range("A2", m_objOpt);
					m_objRange = m_objRange.get_Resize(rows,colums);
					m_objRange.Value = objData;
					//m_objRange.Columns.AutoFit();
				}

				m_objRange.NumberFormatLocal = "@";

				//设置格式为全部都为字符型
				m_objSheet.UsedRange.NumberFormatLocal = "@";
				m_objSheet.Columns.AutoFit();
				

				//NumberFormatLocal 

				// Save the Workbook and quit Excel.
				string path = this.dsPage .Request.MapPath(this.dsPage .Request.ApplicationPath);
				string name = Guid.NewGuid().ToString();
				string filePath = path + @"\Upload\" + name + ".xls";

				m_objBook.SaveAs(filePath , m_objOpt, m_objOpt,
					m_objOpt, m_objOpt, m_objOpt, Excel.XlSaveAsAccessMode.xlNoChange,
					m_objOpt, m_objOpt, m_objOpt, m_objOpt);


				m_objBook.Close(false, m_objOpt, m_objOpt);
				m_objExcel.Quit();


				//开始导出的部分
				dsPage.Response.WriteFile(filePath , true);

				//			Thread.Sleep(2000);
				dsPage.Response.AddHeader("Content-Disposition", "attachment; filename=" +  fileName + ".xls");
				dsPage.Response.ContentType = "application/ms-Excel";
				dsPage.Response.ContentEncoding=System.Text.Encoding.GetEncoding("UTF-8");
				File.Delete(filePath);

				//			File.r
				dsPage.Response.End();
				dsPage.Response.Close();
			}
			catch(Exception e)
			{
				throw(new Exception(e.Message));
			}
			finally
			{
				DoClear();
			}			
		}

		private void ExportAsExcel(DataTable dt , String fileName , String boardFileName , Int32 startRow)
		{
			int rows   = dt.Rows.Count;
			int colums = dt.Columns.Count;
			int index,count = 0;

			//保存数据的数组
			string [,] objData = new string [rows,colums];

			//			if(rows == 0)
			//			{
			//				throw(new Exception("数据源不能为空！"));
			//			}

			if(colums == 0)
			{
				throw(new Exception("导出字段不能为空！"));
			}

			//循环填充数据
			foreach(DataRow dr in dt.Rows)
			{
				index = 0;
				foreach(DataColumn dc in dt.Columns)
				{
					objData[count,index] = dr[dc.ColumnName].ToString().Trim();
					index ++;
				}
				count ++;
			}

			// Start a new workbook in Excel.
			try
			{
				m_objExcel = new Excel.Application();
				m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;
				m_objBook = (Excel._Workbook)(m_objBooks.Open( boardFileName,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing));
				m_objSheets = (Excel.Sheets)m_objBook.Worksheets;
				m_objSheet = (Excel._Worksheet)(m_objSheets.get_Item(1));
				m_objExcel.Visible = false;

				// Create an array with 3 columns and 100 rows and add it to
				// the worksheet starting at cell A2.
				if(rows > 0)
				{
					m_objRange = m_objSheet.get_Range("A" + startRow.ToString(), m_objOpt);
					m_objRange = m_objRange.get_Resize(rows,colums);
					m_objRange.Value = objData;
					m_objRange.NumberFormatLocal = "@";
				}


				//设置格式为全部都为字符型
				m_objSheet.UsedRange.NumberFormatLocal = "@";
				m_objSheet.Columns.AutoFit();
				

				//NumberFormatLocal 

				// Save the Workbook and quit Excel.
				string path = this.dsPage .Request.MapPath(this.dsPage .Request.ApplicationPath);
				string name = Guid.NewGuid().ToString();
				string filePath = path + @"\Templates\" + name + ".xls";

				m_objBook.SaveAs(filePath , m_objOpt, m_objOpt,
					m_objOpt, m_objOpt, m_objOpt, Excel.XlSaveAsAccessMode.xlNoChange,
					m_objOpt, m_objOpt, m_objOpt, m_objOpt);


				m_objBook.Close(false, m_objOpt, m_objOpt);
				m_objExcel.Quit();


				//开始导出的部分
				dsPage.Response.WriteFile(filePath , true);

				//			Thread.Sleep(2000);
				dsPage.Response.AddHeader("Content-Disposition", "attachment; filename=" +  fileName + ".xls");
				dsPage.Response.ContentType = "application/ms-Excel";
				dsPage.Response.ContentEncoding=System.Text.Encoding.GetEncoding("UTF-8");
				File.Delete(filePath);

				//			File.r
				dsPage.Response.End();
				dsPage.Response.Close();
			}
			catch(Exception e)
			{
				throw(new Exception(e.Message));
			}
			finally
			{
				DoClear();
			}			
		}
		#endregion

		#region 得到Excel文件中Sheet信息
		/// <summary>
		/// 得到指定Excel文件的Sheet信息
		/// </summary>
		/// <param name="fileName">文件名称包括文件的详细路径</param>
		/// <returns></returns>
		public String[] GetExcelSheetName(string fileName)   
		{   
			String [] SheetName = null;
			if(fileName == String.Empty)   
			{   
				return SheetName;   
			}   
			try   
			{
				m_objExcel = new Excel.Application();
				m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;
				m_objBook = (Excel._Workbook)(m_objBooks.Open(fileName,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing,
					Type.Missing, Type.Missing, Type.Missing, Type.Missing));
				m_objSheets = (Excel.Sheets)m_objBook.Worksheets;
				m_objSheet = (Excel._Worksheet)(m_objSheets.get_Item(1));
				m_objExcel.Visible = false;
    
				Int32 sheetCount = m_objBook.Worksheets.Count;   
				if(sheetCount==0)   
				{   
					return SheetName;
				}   

				SheetName = new String[sheetCount];
				for(int i=1;i<=sheetCount;i++)   
				{   
					m_objSheet = (Excel._Worksheet)(m_objSheets.get_Item(i));
					SheetName[i-1] = m_objSheet.Name;   
				}  

				m_objBook.Close(false, m_objOpt, m_objOpt);
				m_objExcel.Quit();
			}  
			catch(Exception  ex)   
			{   
				throw(new Exception(ex.Message));
			}
			finally
			{
				DoClear();
			}		
			return  SheetName;
		} 
		#endregion

		#region 释放资源
		private void DoClear()
		{
			try
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject(m_objFont);
				m_objFont = null;
			}
			catch{}

				
			try
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject(m_objRange);
				m_objRange = null;
			}
			catch{}

				
			try
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject(m_objSheet);
				m_objSheet = null;
			}
			catch{}

				
			try
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject(m_objSheets);
				m_objSheets = null;
			}
			catch{}

				
			try
			{
				m_objBook.Close(false, m_objOpt, m_objOpt);
				System.Runtime.InteropServices.Marshal.ReleaseComObject(m_objBook);
				m_objBook = null;
			}
			catch{}

				
				
			try
			{
				m_objBooks.Close();
				System.Runtime.InteropServices.Marshal.ReleaseComObject(m_objBooks);
				m_objBooks = null;
			}
			catch{}

			try
			{
				m_objExcel.Quit();
				
				System.Runtime.InteropServices.Marshal.ReleaseComObject(m_objExcel);
				
				m_objExcel = null;
			}
			catch{}

			System.GC.Collect();
		}
		#endregion
	}
}
