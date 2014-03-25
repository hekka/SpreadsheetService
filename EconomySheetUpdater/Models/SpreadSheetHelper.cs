using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Configuration;
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
                        var parameters = new OAuth2Parameters {AccessToken = AccessToken};
                        _s.RequestFactory = new GOAuth2RequestFactory(null, "Kirkegata37SSS", parameters);
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

        public static string AccessToken { set; private get; }

        #endregion

        internal static void UpdateSpreadSheet(string user, string id, string type, string amount)
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
            if (row == null) return;

            foreach (ListEntry.Custom element in row.Elements)
            {
                if (String.Equals(element.LocalName, user, StringComparison.CurrentCultureIgnoreCase))
                {
                    var oldValue = element.Value;
                    element.Value = "="+oldValue +"+"+ spent;

                }
            }
            row.Update();
            SetDeltaCell(ws, type);
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
            var cell = cf.Entries.SingleOrDefault(s => s.Title.Text == "D"+level) as CellEntry;
            if (cell == null) return;
            cell.InputValue = "=(B"+level+"-C"+level+")/2";
            cell.Update();
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
    }
}