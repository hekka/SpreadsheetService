using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Globalization;
using System.Web.Configuration;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace EconomySheetUpdater.Models
{
    public class OverviewModel
    {
        public string Month { get; set; }
        public double[] Henri;
        public double[] Fredrik;
        public double[] Delta;

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
                }
            }
            if (wse != null)
            {
                Henri = SpreadSheetHelper.getColumn(wse, 'C', 2, 6);
                Fredrik = SpreadSheetHelper.getColumn(wse, 'B', 2, 6);
                Delta = SpreadSheetHelper.getColumn(wse, 'D', 2, 6);
                Month = currentMonth;
                return this;
            }
            throw new ModelValidationException();
        }
    }
}