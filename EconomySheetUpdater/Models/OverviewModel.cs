using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Globalization;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml.Linq;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace EconomySheetUpdater.Models
{
    public class OverviewModel
    {
        public string Month { get; set; }
        public int[] Henri;
        public int[] Fredrik;
        public int[] Delta;
        public string WsId;
        public IEnumerable<SelectListItem> OtherWorksheets { get; set; }

        public OverviewModel GetModel(int id)
        {
            if (id==-1)
                return GetModel();
            var spreadSheet = SpreadSheetHelper.GetSpreadSheet(WebConfigurationManager.AppSettings["SpreadsheetURI"]);
            var wse = spreadSheet.Worksheets.Entries[id] as WorksheetEntry;
            if (wse != null)
            {
                Henri = SpreadSheetHelper.getColumn(wse, 'C', 2, 6);
                Fredrik = SpreadSheetHelper.getColumn(wse, 'B', 2, 6);
                Delta = SpreadSheetHelper.getColumn(wse, 'D', 2, 6);
                Month = wse.Title.Text;
                OtherWorksheets = SpreadSheetHelper.GetWorkSheets(spreadSheet);
                return this;
            }
            throw new ModelValidationException();
        }

        public OverviewModel GetModel()
        {
            var spreadSheet = SpreadSheetHelper.GetSpreadSheet(WebConfigurationManager.AppSettings["SpreadsheetURI"]);
            var currentMonth = DateTime.Now.ToString("MMMM");
            WorksheetEntry wse = null;
            foreach (var entry in spreadSheet.Worksheets.Entries)
            {
                
                if (entry.Title.Text.Equals(currentMonth, StringComparison.InvariantCultureIgnoreCase))
                {
                    wse = entry as WorksheetEntry;
                    currentMonth = entry.Title.Text;
                    break;
                }
            }
            if (wse != null)
            {
                Henri = SpreadSheetHelper.getColumn(wse, 'C', 2, 6);
                Fredrik = SpreadSheetHelper.getColumn(wse, 'B', 2, 6);
                Delta = SpreadSheetHelper.getColumn(wse, 'D', 2, 6);
                Month = currentMonth;
                OtherWorksheets = SpreadSheetHelper.GetWorkSheets(spreadSheet);
                return this;
            }
            throw new ModelValidationException();
        }
    }
}