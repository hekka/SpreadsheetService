using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using EconomySheetUpdater.Models;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Sample.MVC4;

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
                SpreadSheetHelper.AccessToken = result.Credential.Token.AccessToken;

                var model = SpreadSheetUpdateViewModel.GetModel();
                return View(model);
            }
 
            return new RedirectResult(result.RedirectUri);
            
        }

        [HttpPost]
        public ActionResult IndexAsync(SpreadSheetUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                SpreadSheetHelper.UpdateSpreadSheet(model.SelectedUser, model.SelectedWorksheetId,
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