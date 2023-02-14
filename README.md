# DTIMS
驾校管理系统需保存


							SQLSERVER 日期格式转换大全
一般存入数据库中的时间格式为yyyy-mm-ddhh:mm:ss 如果要转换为yyyy-mm-dd  短日期格式.可以使用convert函数.下面是sqlserver帮助中关于convert函数的声明:

使用 CONVERT：

CONVERT (data_type[(length)],expression[,style])

参数

expression

是任何有效的 Microsoft® SQL Server™ 表达式。
data_type

目标系统所提供的数据类型，包括bigint和sql_variant。不能使用用户定义的数据类型。

length

nchar、nvarchar、char、varchar、binary或varbinary数据类型的可选参数。

style

日期格式样式，借以将datetime或smalldatetime数据转换为字符数据（nchar、nvarchar、char、varchar、nchar或nvarchar数据类型）；或者字符串格式样式，借以将float、real、money或smallmoney数据转换为字符数据（nchar、nvarchar、char、varchar、nchar或nvarchar数据类型）。

=======================================================================

SQL Server 支持使用科威特算法的阿拉伯样式中的数据格式。

在表中，左侧的两列表示将datetime或smalldatetime转换为字符数据的style值。给style值加 100，可获得包括世纪数位的四位年份 (yyyy)。

不带世纪数位 (yy)	带世纪数位 (yyyy)	
标准	
输入/输出**
-	0 或 100 (*)	默认值	mon dd yyyy hh:miAM（或 PM）
1	101	美国	mm/dd/yyyy
2	102	ANSI	yy.mm.dd
3	103	英国/法国	dd/mm/yy
4	104	德国	dd.mm.yy
5	105	意大利	dd-mm-yy
6	106	-	dd mon yy
7	107	-	mon dd, yy
8	108	-	hh:mm:ss
-	9 或 109 (*)	默认值 + 毫秒	mon dd yyyy hh:mi:ss:mmmAM（或 PM）
10	110	美国	mm-dd-yy
11	111	日本	yy/mm/dd
12	112	ISO	yymmdd
-	13 或 113 (*)	欧洲默认值 + 毫秒	dd mon yyyy hh:mm:ss:mmm(24h)
14	114	-	hh:mi:ss:mmm(24h)
-	20 或 120 (*)	ODBC 规范	yyyy-mm-dd hh:mm:ss[.fff]
-	21 或 121 (*)	ODBC 规范（带毫秒）	yyyy-mm-dd hh:mm:ss[.fff]
-	126(***)	ISO8601	yyyy-mm-dd Thh:mm:ss:mmm（不含空格）
-	130*	科威特	dd mon yyyy hh:mi:ss:mmmAM
-	131*	科威特	dd/mm/yy hh:mi:ss:mmmAM
*    默认值（style0 或 100、9 或 109、13 或 113、20 或 120、21 或 121）始终返回世纪数位 (yyyy)。
** 当转换为datetime时输入；当转换为字符数据时输出。
*** 专门用于 XML。对于从datetime或smalldatetime到character数据的转换，输出格式如表中所示。对于从float、money或smallmoney到character数据的转换，输出等同于style2。对于从real到character数据的转换，输出等同于style1。

 

重要  默认情况下，SQL Server 根据截止年份 2049 解释两位数字的年份。即，两位数字的年份 49 被解释为 2049，而两位数字的年份 50 被解释为 1950。许多客户端应用程序（例如那些基于 OLE 自动化对象的客户端应用程序）都使用 2030 作为截止年份。SQL Server 提供一个配置选项（"两位数字的截止年份"），借以更改 SQL Server 所使用的截止年份并对日期进行一致性处理。然而最安全的办法是指定四位数字年份。

当从smalldatetime转换为字符数据时，包含秒或毫秒的样式将在这些位置上显示零。当从datetime或smalldatetime值进行转换时，可以通过使用适当的char或varchar数据类型长度来截断不需要的日期部分。

=======================================================================

如果只取yyyy-mm-dd格式时间, 就可以用 convert(nvarchar(10),field,120)
120 是格式代码,  nvarchar(10) 是指取出前10位字符.

 

语句及查询结果：
SELECT CONVERT(varchar(100), GETDATE(), 0): 05 16 2006 10:57AM
SELECT CONVERT(varchar(100), GETDATE(), 1): 05/16/06
SELECT CONVERT(varchar(100), GETDATE(), 2): 06.05.16
SELECT CONVERT(varchar(100), GETDATE(), 3): 16/05/06
SELECT CONVERT(varchar(100), GETDATE(), 4): 16.05.06
SELECT CONVERT(varchar(100), GETDATE(), 5): 16-05-06
SELECT CONVERT(varchar(100), GETDATE(), 6): 16 05 06
SELECT CONVERT(varchar(100), GETDATE(), 7): 05 16, 06
SELECT CONVERT(varchar(100), GETDATE(), 8): 10:57:46
SELECT CONVERT(varchar(100), GETDATE(), 9): 05 16 2006 10:57:46:827AM
SELECT CONVERT(varchar(100), GETDATE(), 10): 05-16-06
SELECT CONVERT(varchar(100), GETDATE(), 11): 06/05/16
SELECT CONVERT(varchar(100), GETDATE(), 12): 060516
SELECT CONVERT(varchar(100), GETDATE(), 13): 16 05 2006 10:57:46:937
SELECT CONVERT(varchar(100), GETDATE(), 14): 10:57:46:967
SELECT CONVERT(varchar(100), GETDATE(), 20): 2006-05-16 10:57:47
SELECT CONVERT(varchar(100), GETDATE(), 21): 2006-05-16 10:57:47.157
SELECT CONVERT(varchar(100), GETDATE(), 22): 05/16/06 10:57:47 AM
SELECT CONVERT(varchar(100), GETDATE(), 23): 2006-05-16
SELECT CONVERT(varchar(100), GETDATE(), 24): 10:57:47
SELECT CONVERT(varchar(100), GETDATE(), 25): 2006-05-16 10:57:47.250
SELECT CONVERT(varchar(100), GETDATE(), 100): 05 16 2006 10:57AM
SELECT CONVERT(varchar(100), GETDATE(), 101): 05/16/2006
SELECT CONVERT(varchar(100), GETDATE(), 102): 2006.05.16
SELECT CONVERT(varchar(100), GETDATE(), 103): 16/05/2006
SELECT CONVERT(varchar(100), GETDATE(), 104): 16.05.2006
SELECT CONVERT(varchar(100), GETDATE(), 105): 16-05-2006
SELECT CONVERT(varchar(100), GETDATE(), 106): 16 05 2006
SELECT CONVERT(varchar(100), GETDATE(), 107): 05 16, 2006
SELECT CONVERT(varchar(100), GETDATE(), 108): 10:57:49
SELECT CONVERT(varchar(100), GETDATE(), 109): 05 16 2006 10:57:49:437AM
SELECT CONVERT(varchar(100), GETDATE(), 110): 05-16-2006
SELECT CONVERT(varchar(100), GETDATE(), 111): 2006/05/16
SELECT CONVERT(varchar(100), GETDATE(), 112): 20060516
SELECT CONVERT(varchar(100), GETDATE(), 113): 16 05 2006 10:57:49:513
SELECT CONVERT(varchar(100), GETDATE(), 114): 10:57:49:547
SELECT CONVERT(varchar(100), GETDATE(), 120): 2006-05-16 10:57:49
SELECT CONVERT(varchar(100), GETDATE(), 121): 2006
Select CONVERT(varchar(100), GETDATE(), 126): 2006-05-16T10:57:49.827
Select CONVERT(varchar(100), GETDATE(), 130): 18 ???? ?????? 1427 10:57:49:907AM
Select CONVERT(varchar(100), GETDATE(), 131): 18/04/1427 10:57:49:920AM
