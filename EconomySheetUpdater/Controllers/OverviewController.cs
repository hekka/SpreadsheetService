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

namespace EconomySheetUpdater.Controllers
{
    public class OverviewController : Controller
    {
        //
        // GET: /Overview/

        public ActionResult IndexHelper()
        {
            return new RedirectResult("Index");
        }

        public async Task<ActionResult> Index(int WsId = -1)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
           AuthorizeAsync(new CancellationToken(false));
            if (result.Credential != null)
            {
                SpreadSheetHelper.setFactory(result.Credential.Token.AccessToken);

            }
            return View(new OverviewModel().GetModel(WsId));
        }


    }
}
