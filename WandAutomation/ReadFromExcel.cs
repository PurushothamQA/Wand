using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SpreadsheetGear;

namespace WandAutomation
{
    public class ReadFromExcel : WandLogin

    {
        public static IWorkbook _workbook = Factory.GetWorkbookSet()
            .Workbooks.Open(Path.Combine(Directory.GetCurrentDirectory(), "ExcelFile\\TimeSheetData.xlsx"));

        public static IList<string> ReadCredentialsFromExcel()
        {
            var sheetName = _workbook.Worksheets["Credentials"];
            var userName = sheetName.UsedRange[0, 1].Text;
            var password = sheetName.UsedRange[1, 1].Text;
            IList<string> lists = new List<string>();
            lists.Add(userName);
            lists.Add(password);
            return lists;
        }

        public static string ReturnValueFromExcel(string key)
        {

            var sheetName = _workbook.Worksheets["Timesheet"];
            IRange rows = sheetName.UsedRange;
            for (var i = 0; i < rows.RowCount; i++)
            {
                if (sheetName.UsedRange.Cells[i, 0].Text == key)
                {
                    return sheetName.UsedRange.Cells[i, 1].Text;
                }
            }
            return null;
        }
    }
}


