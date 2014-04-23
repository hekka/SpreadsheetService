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

        public string ErrorMessage { get; set; }

        public static SpreadSheetUpdateViewModel GetModel()
        {
            var spreadSheet = SpreadSheetHelper.GetSpreadSheet(WebConfigurationManager.AppSettings["SpreadsheetURI"]);
            var worksheets = SpreadSheetHelper.GetWorkSheets(spreadSheet);

            var tempWorkSheet = spreadSheet.Worksheets.Entries[0];
            AtomLink listFeedLink = tempWorkSheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = SpreadSheetHelper.Query(listQuery);
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



        public static SpreadSheetUpdateViewModel GetErrorModel()
        {
            var model = GetModel();
            model.ErrorMessage = "wtf u done??";
            return model;
        }
    }
}