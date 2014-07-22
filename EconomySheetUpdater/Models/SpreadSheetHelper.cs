﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Configuration;
using System.Web.Mvc;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace EconomySheetUpdater.Models
{
    public class SpreadSheetHelper
    {
        #region PROPS

        private static SpreadsheetsService _s;

        private static SpreadsheetsService _Service
        {
            get
            {
                if (_s == null)
                {
                    try
                    {
                        _s = new SpreadsheetsService("Kirkegata37SSS");
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Failed during initialization of Google Services: " + e.Message);
                    }
                }

                return _s;
            }
        }

        private static SpreadsheetFeed _f;

        private static SpreadsheetFeed _Feed
        {
            get
            {
                if (_f == null)
                {
                    try
                    {
                        _f = _Service.Query(new SpreadsheetQuery());
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Failed during initialization of Google Services: " + e.Message);
                    }
                }
                return _f;
            }
        }


        //This needs to be called separately for each controller
        public static void setFactory(string at)
        {
            _Service.RequestFactory = new GOAuth2RequestFactory(null, "Kirkegata37SSS",
                                                                new OAuth2Parameters {AccessToken = at});
        }

        //public static string AccessToken { set; get; }

        #endregion

        internal static bool UpdateSpreadSheet(string user, string id, string type, string amount)
        {
            var ss = GetSpreadSheet(WebConfigurationManager.AppSettings["SpreadsheetURI"]);
            var index = -1;
            var spent = -1;
            try
            {
                index = Convert.ToInt32(id);
                spent = Convert.ToInt32(amount);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Conversion Error! " + e.Message);
                return false;
            }
            var ws = ss.Worksheets.Entries[index] as WorksheetEntry;
            var listFeedLink = ws.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            var listFeed = Query(new ListQuery(listFeedLink.HRef.ToString()));
            ListEntry row = null;
            foreach (ListEntry r in listFeed.Entries)
            {
                if (String.Equals(r.Elements[0].Value, type, StringComparison.CurrentCultureIgnoreCase))
                    row = r;
            }
            if (row == null) return false;

            foreach (ListEntry.Custom element in row.Elements)
            {
                if (String.Equals(element.LocalName, user, StringComparison.CurrentCultureIgnoreCase))
                {
                    var oldValue = element.Value;
                    element.Value = "=" + oldValue + "+" + spent;
                }
            }
            try
            {
                row.Update();
                SetDeltaCell(ws, type);
            }
            catch (WebException e)
            {
                Debug.WriteLine("Probably not sufficent accessrights... \n" + e.Message);
                return false;
            }
            return true;
        }

        public static SpreadsheetEntry GetSpreadSheet(string uri)
        {
            return
                _Feed.Entries.SingleOrDefault(
                    res => res.Id.AbsoluteUri.Equals(uri, StringComparison.InvariantCultureIgnoreCase)) as
                SpreadsheetEntry;
        }

        public static ListFeed Query(ListQuery lq)
        {
            return _Service.Query(lq);
        }

        public static CellFeed Query(CellQuery lq)
        {
            return _Service.Query(lq);
        }

        private static void SetDeltaCell(WorksheetEntry ws, string type)
        {
            var level = getCellLevel(type);
            CellFeed cf = Query(new CellQuery(ws.CellFeedLink));
            var cell = cf.Entries.SingleOrDefault(s => s.Title.Text == "D" + level) as CellEntry;
            if (cell == null) return;
            cell.InputValue = "=(B" + level + "-C" + level + ")/2";
            cell.Update();
        }

        public static string[] getColumn(WorksheetEntry ws, char col, int startrange, int stoprange)
        {
            CellFeed cf = Query(new CellQuery(ws.CellFeedLink));
            var res = new string[stoprange - startrange + 1];
            for (int i = startrange, ind = 0; i <= stoprange; i++, ind++)
            {
                var cell = cf.Entries.SingleOrDefault(s => s.Title.Text == col + "" + i) as CellEntry;
                if (cell != null)
                {
                    try
                    {
                        res[ind] = cell.Value;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        Debug.WriteLine("i=" + i);
                        res[ind] = "0";
                    }
                }
                else
                {
                    res[ind] = "0";
                }
            }
            return res;
        }

        private static int getCellLevel(string type)
        {
            switch (type)
            {
                case "Electricity":
                    return 2;
                case "Internet":
                    return 3;
                case "Food":
                    return 4;
                case "Misc":
                    return 5;
                default:
                    return -1;
            }
        }

        public static List<SelectListItem> GetWorkSheets(SpreadsheetEntry spreadSheet)
        {
            var indexValue = 0;
            return spreadSheet.Worksheets.Entries.Select(worksheet => new SelectListItem
                {
                    Text = worksheet.Title.Text.ToString(),
                    Value = (indexValue++).ToString()
                }).ToList();
        }

        public static bool AddWorksheet(string title)
        {
            var ss = GetSpreadSheet(WebConfigurationManager.AppSettings["SpreadsheetURI"]);
            try
            {
                var ws = ss.Worksheets.Entries[0];
                ws.Title.Text = title;
                //_Service.Insert(ss.Worksheets, new WorksheetEntry (6, 4, title));
                _Service.Insert(ss.Worksheets, ws);
                return true;

            }
            catch (Exception e)
            {
                Debug.Write("HÄR ÄR EXCEPTIONET"+e.Message);
                return false;
            }
           /* ss = GetSpreadSheet(WebConfigurationManager.AppSettings["SpreadsheetURI"]);
            var worksheet = ss.Worksheets.Entries.Last() as WorksheetEntry;
            CellFeed cf = Query(new CellQuery(worksheet.CellFeedLink));

            var res = new List<bool> {UpdateCell(cf, 1, 2, "Electricity"),
            UpdateCell(cf, 1, 3, "Internet"),
            UpdateCell(cf, 1,4, "Food"),
            UpdateCell(cf, 1,5, "Misc"),
            UpdateCell(cf, 1,6, "Total Month"),


            UpdateCell(cf, 2,1, "Fredrik"),
            UpdateCell(cf, 2,6, "=9400-sum(D2:D5)"),
            
            UpdateCell(cf, 3,1, "Henri"),
            UpdateCell(cf, 3,6, "=8600+sum(D2:D5)"),

            UpdateCell(cf, 4,1, "Delta H to F"),
            UpdateCell(cf, 4,6, "=sum(D2+D3+D4+D5)")};
            
            
            
            SetDeltaCell(worksheet, "Electricity");
            SetDeltaCell(worksheet, "Internet");
            SetDeltaCell(worksheet, "Food");
            SetDeltaCell(worksheet, "Misc");
            return !res.Contains(false);*/
        }

        private static bool UpdateCell(AtomFeed cf, uint col, uint row, string Value)
        {
            //var cell = cf.Entries.SingleOrDefault(s => s.Title.Text == cellTitle) as CellEntry;
            var c = new CellEntry(row, col, Value) {Service = _Service};
            c.Update();
            //if (cell != null)
            //{
            //    cell.InputValue = Value;
            //    cell.Update();
                return true;
            //}
            //return false;
        }
    }
}