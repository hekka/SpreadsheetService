using Google.GData.Spreadsheets;

namespace EconomySheetUpdater.Models
{
    public class SpreadSheetUpdateViewModel
    {
        public SpreadsheetEntry Entry { get; set; }
        public string Name { get; set; }
        public string ExpenditureType { get; set; }
        public string Amount { get; set; }
    }

}