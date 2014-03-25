using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace EconomySheetUpdater.Models
{
    public class SpreadSheetUpdateViewModel
    {
        public IEnumerable<SelectListItem> Users { get; set; }
        public string SelectedUser { get; set; }

        public IEnumerable<SelectListItem> ExpenditureTypes { get; set; }
        public string SelectedExpenditureType { get; set; }
        
        public string SelectedAmount { get; set; }

        public string SpreadsheetTitle { get; set; }

        public IEnumerable<SelectListItem> Worksheets { get; set; }
        public string SelectedWorksheetId { get; set; }

        public static SpreadSheetUpdateViewModel GetModel()
        {
            var spreadSheet = GetSpreadSheet(WebConfigurationManager.AppSettings["SpreadsheetURI"]);
            var worksheets = GetWorkSheets(spreadSheet);

            var tempWorkSheet = spreadSheet.Worksheets.Entries[0];
            AtomLink listFeedLink = tempWorkSheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = SpreadSheetHelper.Service.Query(listQuery);
            var expenditureTypes = GetExpenditureTypes(listFeed);

            var users = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Henri",
                    Value = "Henri"
                },
                new SelectListItem
                {
                    Text = "Fredrik",
                    Value = "Fredrik"
                }
            };

            var model = new SpreadSheetUpdateViewModel
            {
                Worksheets = worksheets,
                SpreadsheetTitle = spreadSheet.Title.Text,
                ExpenditureTypes = expenditureTypes,
                Users = users
            };
            return model;
        }

        private static IEnumerable<SelectListItem> GetExpenditureTypes(ListFeed listFeed)
        {
            var expenditureTypes = new List<SelectListItem>();
            foreach (ListEntry r in listFeed.Entries)
            {
                if (!r.Elements[0].Value.Contains("Total"))
                    expenditureTypes.Add(new SelectListItem
                    {
                        Text = r.Elements[0].Value,
                        Value = r.Elements[0].Value
                    });
            }
            return expenditureTypes;
        }

        private static List<SelectListItem> GetWorkSheets(SpreadsheetEntry spreadSheet)
        {
            var indexValue = 0;
            var worksheets = new List<SelectListItem>();
            foreach (var worksheet in spreadSheet.Worksheets.Entries)
            {
                worksheets.Add(new SelectListItem
                {
                    Text = worksheet.Title.Text.ToString(),
                    Value = (indexValue++).ToString()
                });
            }
            return worksheets;
        }

        private static SpreadsheetEntry GetSpreadSheet(string uri)
        {
            return
                SpreadSheetHelper.Feed.Entries.SingleOrDefault(
                    res => res.Id.AbsoluteUri.Equals(uri, StringComparison.InvariantCultureIgnoreCase)) as
                SpreadsheetEntry;
        }
    }
}