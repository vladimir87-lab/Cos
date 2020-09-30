using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using ControlOper.Service;
using System.Configuration;


// Q*ualityControl3   - таблица на сервере
// Q*uality Control   - таблица на локалхосте


namespace ControlOper.Service
{
    public class SqlQuery
    {
        Function func = new Function();
        public static SqlConnection conn;
       // public static string connstr = Function.ReadStringConnect(System.Web.Hosting.HostingEnvironment.MapPath("/Content/connect.txt"));
        public static string connstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        // public static string connstr = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = ControlOper; Integrated Security = True";
        public void ConnectSQLServer()
        {
            conn = new SqlConnection(connstr);
            conn.Open();
        }

        // выводим true если логин и пароль соответствует
        public bool SelectUser(string login, string pass)
        {
            string passmd5 = Function.getMd5Hash(pass.Trim());
            bool flag = false;
            SqlCommand cmd = new SqlCommand("SELECT TOP (1000) [Id] ,[Login] ,[Pass] ,[Role] FROM[oktell].[dbo].[MP-Users-QC] Where [Login] = '" + login + "' and [Pass] = '" + passmd5 + "';", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows) // если есть данные
            {

                flag = true;
            }

            reader.Close();
            conn.Close();
            conn.Dispose();
            return flag;
        }

        //Выводим список всех работников колл-центра
        public List<object[]> SelectListAllManager()
        {
            ConnectSQLServer();
            SqlCommand cmd = new SqlCommand("SELECT TOP (1000) [Login],[Type],[Name],[ID] FROM[oktell_settings].[dbo].[A_Users]", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            
                List<object[]> result = new List<object[]>();
                while (reader.Read())
                {
                    object[] str = new Object[reader.FieldCount];
                    int fieldCount = reader.GetValues(str);
                    result.Add(str);                 
                }
                reader.Close();
                conn.Close();
                conn.Dispose();
                return result;
        }
        //Выводим всех оценивающих пользователей
        public List<string> SelectAllOcenManager()
        {
            ConnectSQLServer();
          //  SqlCommand cmd = new SqlCommand("SELECT b.[Meneger] FROM [oktell].[dbo].[QualityControl3] a left join[dbo].[MP-Value-QC] b ON a.idchain = b.IdChain WHERE b.[Meneger] != 'null'", conn);
            SqlCommand cmd = new SqlCommand("SELECT [Meneger] FROM [dbo].[MP-Value-QC] WHERE [Meneger] != 'null'", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            List<string> result = new List<string>();
            while (reader.Read())
            {
                result.Add(reader[0].ToString());
            }
            reader.Close();
            conn.Close();
            conn.Dispose();
            return result;
        }
        //Выводим список заказов общий
        public List<object[]> PodrazdelManager()
        {


            ConnectSQLServer();
            SqlCommand cmd = new SqlCommand("SELECT distinct [НазваниеГородаОбращения]  FROM[oktell].[dbo].[QualityControl3] order by  [НазваниеГородаОбращения] asc", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            List<object[]> result = new List<object[]>();
            while (reader.Read())
            {
                object[] str = new Object[reader.FieldCount];
                int fieldCount = reader.GetValues(str);
                result.Add(str);
            }
            reader.Close();
            conn.Close();
            conn.Dispose();
            return result;
        }
        //Выводим список заказов общий
        public List<object[]> SelectOrderManager()
        {
            DateTime now = DateTime.Now;
            DateTime startDate = new DateTime(now.Year, now.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            string otdate = "2018-01-01T00:mm";
            string dodate = endDate.ToString("yyyy-MM-ddTHH:mm");

            ConnectSQLServer();
            SqlCommand cmd = new SqlCommand("SELECT a.[ЛогинОператора],a.[НазваниеГородаОбращения],a.[НаименованиеПодразделения],a.[ЛогинМенеджера],a.[ТекстОбращения],a.[idchain],a.[подтема],a.[ДатаВремяЗвонка],b.[IdChain], a.[Результат], a.[Карточки], a.[Причина закрытия]  FROM [oktell].[dbo].[QualityControl3] a left join[dbo].[MP-Value-QC] b ON a.idchain = b.IdChain and a.[ДатаВремяЗвонка] = b.[data] WHERE [ДатаВремяЗвонка] >= '" + otdate + "' AND [ДатаВремяЗвонка] <= '" + dodate + "' ORDER BY [ДатаВремяЗвонка] DESC ", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            List<object[]> result = new List<object[]>();
            while (reader.Read())
            {
                object[] str = new Object[reader.FieldCount];
                int fieldCount = reader.GetValues(str);
                result.Add(str);
            }
            reader.Close();
            conn.Close();
            conn.Dispose();
            return result;
        }
        // Выводим карточку по идчейну
        public List<object[]> SelectOrderId(string idorder)
        {
            string newidch = idorder.Split('|')[0];
            string ntime = idorder.Split('|')[1].Replace('$', '.').Replace('-', ':');
            ConnectSQLServer();
           // SqlCommand cmd = new SqlCommand("SELECT TOP (1000) a.[ЛогинОператора], a.[НазваниеГородаОбращения],a.[НаименованиеПодразделения],a.[ЛогинМенеджера],a.[ТекстОбращения],a.[idchain], a.[file1] ,a.[file2] , b.[IdChain], a.[file3] FROM[oktell].[dbo].[QualityControl3] a left join[dbo].[MP-Value-QC] b ON a.idchain = b.IdChain  WHERE a.[idchain] ='" + idorder + "'", conn);
            SqlCommand cmd = new SqlCommand("SELECT TOP (1000) a.[ЛогинОператора], a.[НазваниеГородаОбращения],a.[НаименованиеПодразделения],a.[ЛогинМенеджера],a.[ТекстОбращения],a.[idchain], a.[file1], a.[file2], a.[file3], a.[file3], b.[IdChain], b.[Meneger], b.[Oc1], b.[Oc2], b.[Oc3], b.[Oc4], b.[Oc5], b.[Oc6], b.[Oc7], b.[Oc8], b.[Oc9], b.[Oc10], b.[Oc11], b.[Oc12], b.[Oc13], b.[Oc14], b.[Oc15], b.[Comment], b.[IsOcObosnov], b.[NizOcOperOrMeneg], case  WHEN a.[rating]='' THEN '80' ELSE isnull (a.[rating],'80') END [rating], a.[e-mail], a.[Результат], a.[taskname], a.[НазваниеОрганизацииКлиента], a.[КонтактноеЛицоКлиента], a.[КонтрагентИзменён], a.[НомерКонтактногоТелефонаКлиента], a.[НомерТелефонаКлиента], b.[err1], b.[err2], b.[err3], b.[err4], b.[err5], a.[тип продукции], a.[ИНН], a.[тип контрагента], b.[err6], a.[ГородСтроительства], a.[FILLIAL], a.[Причина отказа], a.[ДатаПриходаВОфис] , a.[SendTo1C], a.[Карточки],a.[подтема],a.[Причина закрытия], a.[Номер обращения], a.[Number], a.[ДатаВремяЗвонка] FROM[oktell].[dbo].[QualityControl3] a left join[dbo].[MP-Value-QC] b ON a.idchain = b.IdChain and a.[ДатаВремяЗвонка] = b.[data]  WHERE a.[idchain] ='" + newidch + "' and [ДатаВремяЗвонка] LIKE '%" + ntime + "%'", conn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<object[]> result = new List<object[]>();
            while (reader.Read())
            {
                object[] str = new Object[reader.FieldCount];
                int fieldCount = reader.GetValues(str);
                result.Add(str);
            }
            reader.Close();
            conn.Close();
            conn.Dispose();
            return result;
        }

        // выдираем логи звонков и нужные направления
        public List<object[]> SelectLogPachId(string idorder)
        {
            string newidch = idorder.Split('|')[0];
            string ntime = idorder.Split('|')[1].Replace('$', '.').Replace('-', ':');
            ConnectSQLServer();
            //  SqlCommand cmd = new SqlCommand("SELECT TOP (1000) a.[ЛогинОператора], a.[НазваниеГородаОбращения],a.[НаименованиеПодразделения],a.[ЛогинМенеджера],a.[ТекстОбращения],a.[idchain], a.[file1] ,a.[file2] , b.[IdChain], a.[file3] FROM[oktell].[dbo].[QualityControl3] a left join[dbo].[MP-Value-QC] b ON a.idchain = b.IdChain  and a.ДатаВремяЗвонка=b.[data]  WHERE a.[idchain] ='" + newidch + "' and [ДатаВремяЗвонка] LIKE '%"+ ntime + "%'", conn);       // локальный код
            SqlCommand cmd = new SqlCommand("select top 1000 FilePath = /*rd.Path*/'https://oktell.metallprofil.ru:8082/' + replace(substring(ts, 1, 10), '_', '') + '/' + substring(ts, 13, 2) + substring(ts, 16, 2) + '/mix_' + aln + '_' + bln + '__' + ts + '.mp3', Kind = case when  IsOperator = 1 and IsManager = 1 then 'оператор-менеджер' when  IsOperator = 1 and IsManager = 0 then 'оператор-клиент' when  IsOperator = 0 and IsManager = 1 then 'менеджер-клиент' end, TimeStart, conn.* from (select ca.*, aln = case when ca.alinenum < blinenum then alinenum else blinenum end, bln = case when blinenum > alinenum then blinenum else alinenum end, ts = replace(convert(nvarchar(10), TimeStart, 121), '-', '_') + '__' + replace(convert(nvarchar(20), TimeStart, 114), ':', '_'), case when(ca.AUserId not in ('AB000000-0000-0000-0000-000000000000', 'BF000000-0000-0000-0000-000000000000') or ca.BUserId not in ('AB000000-0000-0000-0000-000000000000', 'BF000000-0000-0000-0000-000000000000')) then 1 else 0 end as IsOperator, case when len(AOutNumber) in (4, 6) or len(BOutNumber) in (4, 6) then  1 else 0 end as IsManager from A_Stat_Connections_1x1 as ca  where ca.IdChain = '" + idorder + "' and IsRecorded = 1) as conn join A_Stat_RecordDirectories rd on rd.Id = conn.IdRecDir", conn);  // серверный    код

            SqlDataReader reader = cmd.ExecuteReader();

            List<object[]> result = new List<object[]>();
            while (reader.Read())
            {
                object[] str = new Object[reader.FieldCount];
                int fieldCount = reader.GetValues(str);
                result.Add(str);
            }
            reader.Close();
            conn.Close();
            conn.Dispose();
            return result;
        }

        //Выводим список всех менеджеров сервиса
        public List<object[]> SelectModerators()
        {
            ConnectSQLServer();
            SqlCommand cmd = new SqlCommand("SELECT TOP (1000) [Id],[Login],[Pass],[Role],[Name]FROM[oktell].[dbo].[MP-Users-QC]", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            List<object[]> result = new List<object[]>();
            while (reader.Read())
            {
                object[] str = new Object[reader.FieldCount];
                int fieldCount = reader.GetValues(str);
                result.Add(str);
            }
            reader.Close();
            conn.Close();
            conn.Dispose();
            return result;
        }

        // Добавляем менеджера
        public void AddManeger(string iduser)
        {
            ConnectSQLServer();
            SqlCommand cmd = new SqlCommand("SELECT TOP (1000) [Login],[Type],[Name],[ID] FROM[oktell_settings].[dbo].[A_Users] WHERE[ID] = '"+ iduser + "'", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            string name="";
            string login="";
            
            if (reader.HasRows) // если есть данные
            {
                while (reader.Read())
                {
                    login = reader[0].ToString();
                    name = reader[2].ToString();
                }
            }
            reader.Close();

            SqlCommand cmd2 = new SqlCommand("INSERT INTO [oktell].[dbo].[MP-Users-QC](Login, Pass , Role, Name) Values  ('"+ login + "',null,'maneger', N'"+ name + "')", conn);
            cmd2.ExecuteNonQuery();
            
            conn.Close();
            conn.Dispose();
        }
        // удаляем менеджера из сервиса
        public void DellManeger(string iduser)
        {
            ConnectSQLServer();
            SqlCommand cmd = new SqlCommand("DELETE FROM[oktell].[dbo].[MP-Users-QC] WHERE ID = '"+ iduser + "'",conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }
        // Установка пароля
        public void SetPassManeger(string iduser , string pass)
        {
            pass = Function.getMd5Hash(pass);
            ConnectSQLServer();
            SqlCommand cmd = new SqlCommand("UPDATE [oktell].[dbo].[MP-Users-QC] SET [Pass] = '"+ pass + "' WHERE [Id] = '" + iduser + "'", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }

        // Находим юзера по ИД
        public List<object[]> FoundUser(string iduser)
        {
            ConnectSQLServer();
            SqlCommand cmd = new SqlCommand("SELECT TOP (1000) [Id],[Login],[Pass],[Role],[Name] FROM[oktell].[dbo].[MP-Users-QC] WHERE[ID] = '" + iduser + "'", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            List<object[]> result = new List<object[]>();
            while (reader.Read())
            {
                object[] str = new Object[reader.FieldCount];
                int fieldCount = reader.GetValues(str);
                result.Add(str);
            }
            reader.Close();
            conn.Close();
            conn.Dispose();
            return result;
        }

        // Находим юзера по логину
        public List<object[]> FoundUser(string login , string m)
        {
            ConnectSQLServer();
            SqlCommand cmd = new SqlCommand("SELECT TOP (1000) [Id],[Login],[Pass],[Role],[Name] FROM[oktell].[dbo].[MP-Users-QC] WHERE[Login] = '" + login + "'", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            List<object[]> result = new List<object[]>();
            while (reader.Read())
            {
                object[] str = new Object[reader.FieldCount];
                int fieldCount = reader.GetValues(str);
                result.Add(str);
            }
            reader.Close();
            conn.Close();
            conn.Dispose();
            return result;
        }

        // Добовляем оценку заказа
        public void InsertOcenkaOrder(string idchain, string oc1, string oc2, string oc3, string oc4, string oc5, string oc6, string oc7, string oc8, string oc9, string oc10, string oc11, string oc12, string oc13, string oc14, string oc15, string comment, string userlogin, string IsOcObosn, string NizOc, string error1, string error2, string error3, string error4, string error5, string error6, string data)
        {
            ConnectSQLServer();
            DateTime dt = DateTime.Now;
            SqlCommand comsel = new SqlCommand("SELECT TOP (1000) a.[ЛогинОператора],a.[НазваниеГородаОбращения],a.[НаименованиеПодразделения],a.[ЛогинМенеджера],a.[ТекстОбращения],a.[idchain],a.[подтема],a.[ДатаВремяЗвонка],b.[IdChain], a.[Результат]  FROM [oktell].[dbo].[QualityControl3] a join[dbo].[MP-Value-QC] b ON a.idchain = b.IdChain WHERE a.[idchain] = '" + idchain + "' and b.[data] = '"+ data + "'", conn);

            SqlDataReader read = comsel.ExecuteReader();

            if(read.HasRows)
            {
                read.Close();
                SqlCommand upcom = new SqlCommand("UPDATE [dbo].[MP-Value-QC] SET Meneger = '"+ userlogin + "', Oc1 = '"+ oc1 + "', Oc2 = '" + oc2 + "', Oc3 = '" + oc3 + "', Oc4 = '" + oc4 + "', Oc5 = '" + oc5 + "', Oc6 = '" + oc6 + "', Oc7 = '" + oc7 + "', Oc8 = '" + oc8 + "', Oc9 = '" + oc9 + "', Oc10 = '" + oc10 + "', Oc11 = '" + oc11 + "', Oc12 = '" + oc12 + "', Oc13 = '" + oc13 + "', Oc14 = '" + oc14 + "', Oc15 = '" + oc15+ "', Comment = N'"+ comment + "', DateTime = '" + dt+ "', IsOcObosnov= N'"+ IsOcObosn + "', NizOcOperOrMeneg = N'"+ NizOc + "', err1=N'"+error1+ "', err2=N'" + error2 + "', err3=N'" + error3 + "', err4=N'" + error4 + "', err5=N'" + error5 + "', err6=N'" + error6 + "'  Where[IdChain] = '" + idchain+ "'  and [data] = '" + data + "'", conn);
                upcom.ExecuteNonQuery();

            }
            else
            {
                read.Close();
                SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[MP-Value-QC](IdChain, Meneger , Oc1, Oc2, Oc3, Oc4, Oc5, Oc6, Oc7, Oc8, Oc9, Oc10, Oc11, Oc12, Oc13, Oc14, Oc15, Comment, DateTime, IsOcObosnov, NizOcOperOrMeneg, err1, err2, err3, err4, err5, err6, data ) Values  ('" + idchain + "','" + userlogin + "','" + oc1 + "','" + oc2 + "','" + oc3 + "','" + oc4 + "','" + oc5 + "','" + oc6 + "','" + oc7 + "','" + oc8 + "','" + oc9 + "','" + oc10 + "','" + oc11 + "','" + oc12 + "','" + oc13 + "','" + oc14 + "','" + oc15 + "', N'" + comment + "','" + dt + "', N'"+ IsOcObosn + "', N'"+ NizOc + "', N'"+error1+ "', N'" + error2 + "', N'" + error3 + "', N'" + error4 + "', N'" + error5 + "', N'" + error6 + "', '"+data+"')", conn);
                cmd.ExecuteNonQuery();
            }
                   

            conn.Close();
            conn.Dispose();
        }

        // Запрос с фильтром/////////////////////////////////////////////////////////////////////////////
        public List<object[]> FilterSQLOrder(string[] oper, string datetime1, string datetime2, string[] podr, string[] podtema, string[] resultat, string[] cart, string ocenen, string telef, string namber1c, string[] username, string filtidchain, string[] ocenclient, string[] Prichzak, string Creatlid)
        {
            string kod_oper = "";
            if (oper==null) { kod_oper = " and a.[ЛогинОператора] NOT IN ('')"; }
            else {
                string str="";
                if (oper.Length > 1)
                {
                    foreach (var iten in oper)
                    {
                        str = str + ",N'" + iten.Trim() +"'";
                    }
                    str = str.Substring(1);
                    kod_oper = " and a.[ЛогинОператора] IN (" + str + ")";
                }
                else
                {
                    kod_oper = " and a.[ЛогинОператора] IN (N'" + oper[0] + "')";
                }
                
            }
            string kod_datetime = "[ДатаВремяЗвонка] >= '"+datetime1+"' and [ДатаВремяЗвонка] <= '"+ datetime2 + "'";
            string kod_podr = "";
            if (podr == null) { kod_podr = " "; }
            else
            {
                string str = "";
                if (podr.Length > 1)
                {
                    foreach (var iten in podr)
                    {
                        str = str + ",N'" + iten + "'";
                    }
                    str = str.Substring(1);
                    kod_podr = " and [НазваниеГородаОбращения] IN (" + str + ")";
                }
                else
                {
                    kod_podr = " and [НазваниеГородаОбращения] IN (N'" + podr[0] + "')";
                }

            }

            string kod_podtema = "";
            if (podtema==null) { kod_podtema = " "; }
            else
            {
                string str = "";
                if (podtema.Length > 1)
                {
                    foreach (var iten in podtema)
                    {
                        str = str + ",N'" + iten + "'";
                    }
                    str = str.Substring(1);
                    kod_podtema = " and [подтема] IN (" + str + ")";
                }
                else
                {
                    kod_podtema = " and [подтема] IN (N'" + podtema[0] + "')";
                }

            }

            string kod_resultat = "";
            if (resultat==null) { kod_resultat = " "; }
            else
            {
                string str = "";
                if (resultat.Length > 1)
                {
                    foreach (var iten in resultat)
                    {
                        str = str + ",N'" + iten + "'";
                    }
                    str = str.Substring(1);
                    kod_resultat = " and [Результат] IN (" + str + ")";
                }
                else
                {
                    kod_resultat = " and [Результат]  IN (N'" + resultat[0] + "')";
                }

            }

            string kod_cart = "";
            if (cart==null) { kod_cart = " "; }
            else
            {
                string str = "";
                if (cart.Length > 1)
                {
                    foreach (var iten in cart)
                    {
                        str = str + ",N'" + iten + "'";
                    }
                    str = str.Substring(1);
                    kod_cart = " and [Карточки] IN (" + str + ")";
                }
                else
                {
                    kod_cart = " and [Карточки]  IN (N'" + cart[0] + "')";
                }

            }

            string kod_userlogin = "";
            if (ocenen == "1")
            {
                if (username == null) { kod_userlogin = " "; }
                else
                {
                    string str = "";
                    if (username.Length > 1)
                    {
                        foreach (var iten in username)
                        {
                            str = str + ",N'" + iten + "'";
                        }
                        str = str.Substring(1);
                        kod_userlogin = " and [Meneger] IN (" + str + ")";
                    }
                    else
                    {
                        kod_userlogin = " and [Meneger]  IN (N'" + username[0] + "')";
                    }

                }
            }

            string kod_ocenclient = "";
            if (ocenclient == null) { kod_ocenclient = " "; }
            else
            {
                string str = "";
                if (ocenclient.Length > 1)
                {
                    foreach (var iten in ocenclient)
                    {
                        str = str + ",N'" + iten + "'";
                        if (iten == "80")
                        {
                            str = str + ",''";
                        }
                    }
                    str = str.Substring(1);
                    if (ocenclient.Contains("80"))
                    {
                        kod_ocenclient = " and ([rating] IN (" + str + ") or [rating] is null)";
                    }
                    else
                    {
                        kod_ocenclient = " and [rating] IN (" + str + ")";
                    }
                }
                else
                {
                    if (ocenclient[0] == "80")
                    {
                        kod_ocenclient = " and ([rating]  IN (N'" + ocenclient[0] + "', '') or rating is null)";
                    }
                    else
                    {
                        kod_ocenclient = " and [rating]  IN (N'" + ocenclient[0] + "')";
                    }
                }

            }



            string kod_telef = "";
            if (!string.IsNullOrEmpty(telef))
            {
                kod_telef = " and [НомерТелефонаКлиента] = '"+ telef + "' ";
            }

            string kod_1c = "";
            if (!string.IsNullOrEmpty(namber1c))
            {
                kod_1c = " and ([Number] = '" + namber1c + "'or [Номер обращения] = '" + namber1c + "')";
            }

            string kod_idc = "";
            if (!string.IsNullOrEmpty(filtidchain))
            {
                kod_idc = " and a.[idchain] = '" + filtidchain + "'";
            }

            string kod_creatlid = "";
            if (!string.IsNullOrEmpty(Creatlid))
            {
                kod_creatlid = " and ([Number] is not null or [Number] != '')";
            }

            string kod_prich = "";
            if (Prichzak == null) { kod_prich = " "; }
            else
            {
                string str = "";
                if (Prichzak.Length >= 1)
                {
                    foreach (var iten in Prichzak)
                    {
                        str = str + ",N'" + iten + "'";
                    }
                    str = str.Substring(1);
                    if (Prichzak.Contains(""))
                    {
                        kod_prich = " and ([Причина закрытия] IN (" + str + ", '') or [Причина закрытия] is null)";
                    }
                    else
                    {
                        kod_prich = " and [Причина закрытия] IN (" + str + ")";
                    }
                }
                else
                {
                    kod_prich = " and ([Причина закрытия]='' or [Причина закрытия] is null)";
                }

            }


            string command = "";
            if (ocenen == "0")
            {
                command = "SELECT  a.[ЛогинОператора],a.[НазваниеГородаОбращения],a.[НаименованиеПодразделения],a.[ЛогинМенеджера],a.[ТекстОбращения],a.[idchain],a.[подтема],a.[ДатаВремяЗвонка],b.[IdChain]  FROM [oktell].[dbo].[QualityControl3] a left join[dbo].[MP-Value-QC] b ON a.idchain = b.IdChain and a.[ДатаВремяЗвонка] = b.[data] WHERE " + kod_datetime + "  " + kod_oper + " " + kod_podr + " " + kod_podtema + " " + kod_resultat + "" + kod_cart + " " + kod_telef+ " "+ kod_1c + "  " + kod_userlogin + " " + kod_idc + " " + kod_ocenclient + " "+ kod_creatlid+" "+ kod_prich + " ORDER BY[ДатаВремяЗвонка] DESC ";

            } else if(ocenen == "1")
            {
                command = "SELECT  a.[ЛогинОператора],a.[НазваниеГородаОбращения],a.[НаименованиеПодразделения],a.[ЛогинМенеджера],a.[ТекстОбращения],a.[idchain],a.[подтема],a.[ДатаВремяЗвонка],b.[IdChain]  FROM [oktell].[dbo].[QualityControl3] a  join[dbo].[MP-Value-QC] b ON a.idchain = b.IdChain and a.[ДатаВремяЗвонка] = b.[data]  WHERE " + kod_datetime + "  " + kod_oper + " " + kod_podr + " " + kod_podtema + " " + kod_resultat + "" + kod_cart + " " + kod_telef + " " + kod_1c + " " + kod_userlogin + " " + kod_idc + " " + kod_creatlid + " " + kod_prich + " and b.[data] is not null ORDER BY[ДатаВремяЗвонка] DESC ";
              
            } else
            {
                command = "SELECT  a.[ЛогинОператора],a.[НазваниеГородаОбращения],a.[НаименованиеПодразделения],a.[ЛогинМенеджера],a.[ТекстОбращения],a.[idchain],a.[подтема],a.[ДатаВремяЗвонка],b.[IdChain]  FROM [oktell].[dbo].[QualityControl3] a left join[dbo].[MP-Value-QC] b ON a.idchain = b.IdChain and a.[ДатаВремяЗвонка] = b.[data]  WHERE b.idchain is null and " + kod_datetime + "  " + kod_oper + " " + kod_podr + " " + kod_podtema + " " + kod_resultat + "" + kod_cart + " " + kod_telef + " " + kod_1c + " " + kod_userlogin + " " + kod_idc + " " + kod_creatlid + " " + kod_prich + " ORDER BY[ДатаВремяЗвонка] DESC ";
               
            }

            ConnectSQLServer();
            SqlCommand cmd = new SqlCommand(command, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            List<object[]> result = new List<object[]>();
            while (reader.Read())
            {
                object[] str = new Object[reader.FieldCount];
                int fieldCount = reader.GetValues(str);
                result.Add(str);
            }
            
            reader.Close();
            conn.Close();
            conn.Dispose();
            return result;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
    }





}