using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace EconomySheetUpdater.Models
{
    public class WorksheetModel
    {
        public static HashSet<string> ReservedTitles = new HashSet<string>();
        public string NewTitle { get; set; }
        public MvcHtmlString ErrorMessage { get; set; }

        public WorksheetModel getModel()
        {
            var spreadSheet = SpreadSheetHelper.GetSpreadSheet(WebConfigurationManager.AppSettings["SpreadsheetURI"]);
            ReservedTitles.Clear();
            foreach (var entry in spreadSheet.Worksheets.Entries)
            {
                ReservedTitles.Add(entry.Title.Text);
            }
            return this;
        }
        public WorksheetModel getErrorModel()
        {
            var model = getModel();
            model.ErrorMessage = new MvcHtmlString("Name already exists in spreadsheet<br>These names are taken:<br>"+string.Join("<br>", ReservedTitles));
            return model;
        }
    }
}