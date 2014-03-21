using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Sample.MVC4;
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
                SpreadsheetsService service = new SpreadsheetsService("Kirkegata37SSS");
                var query = new SpreadsheetQuery();
                var hora = service.Query(query);
                //ViewBag.feed = 

                
                return View();
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }
    }
}