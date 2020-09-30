using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using ControlOper.Service;
using System.Web.Security;

namespace ControlOper.Controllers
{
    public class HomeController : Controller
    {
        SqlQuery sq = new SqlQuery();


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {

            return View();
        }


        public ActionResult Exit()
        {
            FormsAuthentication.SignOut();
            return Redirect("/login");
        }


        [HttpPost]
        public ActionResult Login(string login, string pass)
        {
            sq.ConnectSQLServer();
            if (sq.SelectUser(login, pass))
            {
                FormsAuthentication.SetAuthCookie(login, true);
                return Redirect("/admin");
            }
            else
            {
                ViewBag.otvet = "Не правильный логин или пароль";
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Maneger(int page=1)
        {
            List<object[]> sdr = sq.SelectOrderManager();
           
            List<object[]> listpodrazd = new List<object[]>();
            List<object> listpodtema = new List<object>();
            List<object> listprichkzak = new List<object>();
            List<object> listopers = new List<object>();
            List<object> listresulr = new List<object>();
            List<object> listcart = new List<object>();

            List<string> listuserlogin = sq.SelectAllOcenManager();

            //
            foreach (var item in sdr)
            {
              //  listpodrazd.Add(item[1]);
                listpodtema.Add(item[6]);
                listopers.Add(item[0]);
                listresulr.Add(item[9]);
                listcart.Add(item[10]);
                listprichkzak.Add(item[11]);


            }
            listuserlogin = listuserlogin.Distinct().ToList();
            listuserlogin.Sort();
            //   listpodrazd = listpodrazd.Distinct().ToList();
            //  listpodrazd.Sort();
            listpodrazd = sq.PodrazdelManager();
              listpodtema = listpodtema.Distinct().ToList();
            listopers = listopers.Distinct().ToList();
            List<string> listopers_str = new List<string>();
            for (int i = 0; i < listopers.Count; i++)
            {
                try
                {
                    listopers_str.Add((string)listopers[i].ToString());
                }
                catch {  }
            }
            listopers_str.Sort();

            listresulr = listresulr.Distinct().ToList();
            listcart = listcart.Distinct().ToList();
            listprichkzak = listprichkzak.Distinct().ToList();
            ViewBag.ListPadrazdel = listpodrazd;
            ViewBag.ListPodtema = listpodtema;
            ViewBag.listopers = listopers_str;
            ViewBag.listresult = listresulr;
            ViewBag.listacrt = listcart;
            ViewBag.listprichzak = listprichkzak;
            ViewBag.listuserlogin = listuserlogin;


            int colstrnic = sdr.Count / 50 + 1;
            sdr = sdr.Skip(page * 50 - 50).Take(50).ToList();
            ViewBag.Colstrnic = colstrnic;
            ViewBag.namberpage = page;
            return View(sdr);

        }

        [Authorize]
        [HttpGet]
        public ActionResult OrderId(string idorder)
        {
            ViewBag.Idorder = idorder;
            string data = idorder.Split('|')[1];
            List<object[]> sdr = sq.SelectOrderId(idorder);
            List<object[]> pachlog = sq.SelectLogPachId(idorder);
            ViewBag.PachLog = pachlog;
            ViewBag.UserRol = sq.FoundUser(User.Identity.Name, "name").FirstOrDefault()[3].ToString();
            ViewBag.Data = sdr.FirstOrDefault()[58];
            return PartialView(sdr);
          //  return View(sdr);
        }

        [Authorize]
        [HttpPost]
        public JsonResult OrderId(string idorder, string oc1, string oc2, string oc3, string oc4, string oc5, string oc6, string oc7, string oc8, string oc9, string oc10, string oc11, string oc12, string oc13, string oc14, string oc15, string comment, string IsOcObos, string NizOc, string err1, string err2, string err3, string err4, string err5, string err6, string data)
        {
            string login = sq.FoundUser(User.Identity.Name, "name").FirstOrDefault()[1].ToString();
            sq.InsertOcenkaOrder(idorder, oc1, oc2, oc3, oc4, oc5, oc6, oc7, oc8, oc9, oc10, oc11, oc12, oc13, oc14, oc15, comment, login, IsOcObos, NizOc, err1, err2, err3, err4, err5, err6, data);
            data = data.Split(' ')[1];
            data = data.Replace(':', '_');
            data = data.Replace('.', '$');
            string newid = idorder + "|" + data;
            return Json(newid);
        }


        [Authorize]
        [HttpPost]
        public ActionResult FilterOrder(string[] podrazdel, string[] oper , string[] podtema, string date1, string date2, string time1, string time2, string[] result, string[] cart, string[] username, string ocenen, string tel, string namber1c, string filtidchain, string[] ocenclient, string[] Prichzak, string Creatlid, int page=1)
        {
            string datetime1 = "";
            string datetime2 = "";

            if (date1 != "" || date2 !="" || time1 != "" || time2 != "")
            {
                datetime1 = date1 + " " + time1;
                datetime2 = date2 + " " + time2;

            }
            List<object[]> sdrfilter = sq.FilterSQLOrder(oper, datetime1, datetime2, podrazdel, podtema, result, cart, ocenen, tel, namber1c, username, filtidchain, ocenclient, Prichzak, Creatlid);

            ViewBag.Colstrnic = sdrfilter.Count / 50 +1;
            sdrfilter = sdrfilter.Skip(page * 50 - 50).Take(50).ToList();
            ViewBag.namberpage = page;
            return PartialView(sdrfilter);

        }


    }
}