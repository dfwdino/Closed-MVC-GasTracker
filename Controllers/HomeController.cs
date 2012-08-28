using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using mvc_MilesTracker.Models;

namespace mvc_MilesTracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "HI";
            return View();
        }

        public ActionResult DisplayAuto()
        {

            mvc_MilesTracker.Models.SQLFunctions HelperSQL = new SQLFunctions();

            //Should be a better way in passing values???
            ViewData.Model = HelperSQL.GetAllAutos();

            return View();
        }

        public ActionResult DisplayGas(string AutoID)
        {
            mvc_MilesTracker.Models.SQLFunctions HelperSQL = new SQLFunctions();

            //Should be a better way in passing values???
            ViewData.Model = HelperSQL.GetMilesForAuto();
            return View();
        }

        

    }
}
