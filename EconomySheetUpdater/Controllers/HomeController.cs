using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using EconomySheetUpdater.Models;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Sample.MVC4;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace EconomySheetUpdater.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> IndexAsync(CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                                   AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                if (SpreadSheetHelper.InitService(result.Credential.Token.AccessToken))
                {
                    var model = SpreadSheetUpdateViewModel.GetModel();
                    return View(model);
                }
                return View();
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }

        [HttpPost]
        public ActionResult IndexAsync(SpreadSheetUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                SpreadSheetHelper.UpdateSpeadSheet(model.SelectedUser, model.SelectedWorksheetId,
                    model.SelectedExpenditureType, model.SelectedAmount);
            }
            return View(SpreadSheetUpdateViewModel.GetModel());
        }

        [HttpGet]
        public ActionResult Index()
        {
            return null;
        }
    }
}