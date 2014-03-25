using System;
using System.Diagnostics;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace EconomySheetUpdater.Models
{
    public class SpreadSheetHelper
    {
        public static SpreadsheetsService Service { get; set; }
        public static SpreadsheetFeed Feed { get; set; }

        public static bool InitService(string accessToken)
        {
            try
            {
                Service = new SpreadsheetsService("Kirkegata37SSS");
                var parameters = new OAuth2Parameters { AccessToken = accessToken };
                Service.RequestFactory = new GOAuth2RequestFactory(null, "Kirkegata37SSS", parameters);
                Feed = Service.Query(new SpreadsheetQuery());
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed during initialization of Google Services: " + e.Message);
                return false;
            }
        }

        internal static void UpdateSpeadSheet(string user, string workSheet, string type, string amount)
        {
            throw new NotImplementedException();
        }
    }
}