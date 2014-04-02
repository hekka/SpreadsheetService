using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google.GData.Spreadsheets;

namespace EconomySheetUpdater.Controllers
{
    public class WorksheetController : Controller
    {
        //
        // GET: /Worksheet/
        [HttpGet]
        public ActionResult Worksheet(string id)
        {
            return View();
        }

    }
}
