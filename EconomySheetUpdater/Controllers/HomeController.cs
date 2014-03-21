using System;
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
        private SpreadsheetsService _service;
        private SpreadsheetFeed _feed;

        public async Task<ActionResult> IndexAsync(CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                                   AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                if (initService(result.Credential.Token.AccessToken))
                {
                    var model = new SpreadSheetUpdateViewModel
                        {
                            Entry = getSpreadSheet(WebConfigurationManager.AppSettings["SpreadsheetURI"])
                        };
                    return View(model);
                }
                return View();
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new SpreadSheetUpdateViewModel();
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(SpreadSheetUpdateViewModel model)
        {
            // call methods to update spreadsheet
            // with the _service and data from model

            //return back to same screen
            return View("Index", model);
        }

        private bool initService(string _accessToken)
        {
            try
            {
                _service = new SpreadsheetsService("Kirkegata37SSS");
                var parameters = new OAuth2Parameters {AccessToken = _accessToken};
                _service.RequestFactory = new GOAuth2RequestFactory(null, "Kirkegata37SSS", parameters);
                _feed = _service.Query(new SpreadsheetQuery());
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed during initialization of Google Services: " + e.Message);
                return false;
            }
        }

        private SpreadsheetEntry getSpreadSheet(string uri)
        {
            return
                _feed.Entries.SingleOrDefault(
                    res => res.Id.AbsoluteUri.Equals(uri, StringComparison.InvariantCultureIgnoreCase)) as
                SpreadsheetEntry;
        }
    }
}