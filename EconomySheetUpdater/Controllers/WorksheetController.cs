using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EconomySheetUpdater.Models;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Sample.MVC4;
using Google.GData.Spreadsheets;

namespace EconomySheetUpdater.Controllers
{
    public class WorksheetController : Controller
    {
        //
        // GET: /Worksheet/
        [HttpGet]
        public async Task<ActionResult> Worksheet()
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
           AuthorizeAsync(new CancellationToken(false));
            if (result.Credential != null)
            {
                SpreadSheetHelper.setFactory(result.Credential.Token.AccessToken);

            }
            return View(new WorksheetModel().getModel());
        }
        [HttpGet]
        public ActionResult WorksheetHelper()
        {
            return new RedirectResult("Worksheet");
        }
        [HttpPost]
        public ActionResult Worksheet(WorksheetModel model)
        {
            
            if (WorksheetModel.ReservedTitles.Contains(model.NewTitle) || !SpreadSheetHelper.AddWorksheet(model.NewTitle))
            {
                return View(new WorksheetModel().getErrorModel());
            }
            return View(new WorksheetModel().getModel());
        }
    }
}
