using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using ControlOper.Service;

namespace ControlOper.Controllers
{
    public class AdminController : Controller
    {
        SqlQuery sq = new SqlQuery();

        [Authorize]       
        public ActionResult Index()
        {
            if (User.Identity.Name == "administrator")
            {
                return View();
            }
            else
            {
                return Redirect("/");
            }
        }
        
        // Список всех работников колл-центра
        [Authorize]
        public ActionResult ListManeger()
        {
            if (User.Identity.Name == "administrator")
            {
                List<object[]> sdr = sq.SelectListAllManager();
                sdr = sdr.OrderBy(i => i[0]).ToList();
                return View(sdr);
            }
            else
            {
                return Redirect("/");
            }

        }
        
        // Список менеджеров системы
        [Authorize]
        public ActionResult ListModerators()
        {
            if (User.Identity.Name == "administrator")
            {
                List<object[]> sdr = sq.SelectModerators();
                sdr = sdr.OrderBy(i=> i[1]).ToList();
                return View(sdr);
            }
            else
            {
                return Redirect("/");
            }

        }

        // Добавить менеджера
        [Authorize]
        [HttpGet]
        public ActionResult AddManeger(string iduser)
        {
            if (User.Identity.Name == "administrator")
            {
                iduser = iduser.ToUpper();
                sq.AddManeger(iduser);
                return Redirect("/listmoderators");
            }
            else
            {
                return Redirect("/");
            }

        }

        // Удалить менеджера       
        [Authorize]
        [HttpGet]
        public ActionResult DellManeger(string iduser)
        {
            if (User.Identity.Name == "administrator")
            {
                sq.DellManeger(iduser);
                return Redirect("/listmoderators");
            }
            else
            {
                return Redirect("/");
            }


        }
        // Установить пароль менеджеру (страница)
        [Authorize]
        [HttpGet]
        public ActionResult SetPassManeger(string iduser)
        {
            if (User.Identity.Name == "administrator")
            {
                List<object[]> sdr = sq.FoundUser(iduser);
                return View(sdr);
            }
            else
            {
                return Redirect("/");
            }

        }

        // Установить пароль менеджеру (пост запрос)
        [Authorize]
        [HttpPost]
        public ActionResult SetPassManeger(string iduser , string pass)
        {
            if (User.Identity.Name == "administrator")
            {
                sq.SetPassManeger(iduser, pass);
                return Redirect("/listmoderators");
            }
            else
            {
                return Redirect("/");
            }

        }

    }
}