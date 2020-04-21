using System;
using Microsoft.Office.Interop.Excel;

namespace Sverto.General.Office
{
    public interface IExcelInstance : IDisposable
    {
        IExcelInstance OpenWorkbook(string path);
        IExcelInstance OpenWorksheet(int worksheetId);
        IExcelInstance OpenWorksheet(string worksheetName);
        void CloseWorkbook(bool save = true);
        void AppendToWorksheet(object[,] data, bool save = true);
        bool IsWorkbookOpen { get; }
        bool IsVisible { get; set; }
        bool? IsWorkbookReadOnly { get; }
    }


    public class ExcelInstance : IExcelInstance
    {
        private Application _App;
        private Workbook _Workbook;
        private Worksheet _Worksheet;

        public ExcelInstance()
        {
            _App = new Application();
        }

        public IExcelInstance OpenWorkbook(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException();
            if (_App == null)
                throw new InvalidOperationException("Excel instance is not open.");
            if (_Workbook != null)
                throw new InvalidOperationException("A workbook is already open.");

            _Workbook = _App.Workbooks.Open(path);
            return this;
        }

        public void CloseWorkbook(bool save = true)
        {
            _Workbook?.Close(save);
            _Workbook = null;
            _Worksheet = null;
        }

        public IExcelInstance OpenWorksheet(int worksheetId)
        {
            return OpenWorksheet(id: worksheetId);
        }

        public IExcelInstance OpenWorksheet(string worksheetName)
        {
            return OpenWorksheet(name: worksheetName);
        }

        private IExcelInstance OpenWorksheet(string name = null, int id = -1)
        {
            if (name == null && id < 0)
                throw new ArgumentException("Worksheet name or id required.");
            if (_Workbook == null)
                throw new InvalidOperationException("Workbook is not open.");

            if (id >= 0)
                _Worksheet = _Workbook.Worksheets[id];
            else
                _Worksheet = _Workbook.Worksheets[name];

            if (_Worksheet == null)
                throw new Exception("Worksheet with given name or id not found.");

            return this;
        }

        public void AppendToWorksheet(object[,] data, bool save = true)
        {
            if (data == null)
                throw new ArgumentNullException();
            if (_Workbook == null)
                throw new InvalidOperationException("Worksheet is not open.");

            // Find first empty row (based on first 3 columns)
            int rowNr = 1;
            while ((_Worksheet.Cells[rowNr, 1] as Range).Value != null ||
                    (_Worksheet.Cells[rowNr, 2] as Range).Value != null ||
                    (_Worksheet.Cells[rowNr, 3] as Range).Value != null)
            {
                rowNr++;
            }
            //int rowNr = _Worksheet.UsedRange.Rows.Count + 1;
            //while ((_Worksheet.Cells[rowNr - 1, 1] as Range).Value == null &&
            //        (_Worksheet.Cells[rowNr - 1, 2] as Range).Value == null &&
            //        (_Worksheet.Cells[rowNr - 1, 3] as Range).Value == null &&
            //        rowNr > 0)
            //{
            //    rowNr--;
            //}

            // Set modify range and apply data
            Range range = _Worksheet.Range[_Worksheet.Cells[rowNr, 1], _Worksheet.Cells[rowNr + data.GetLength(0) - 1, data.GetLength(1) - 1]];
            range.Value = data;
            // Save
            if (save)
                _Workbook.Save();
        }


        public bool IsVisible
        {
            get { return _App?.Visible ?? false; }
            set { if (_App != null) _App.Visible = value; }
        }

        public bool IsWorkbookOpen
        {
            get { return _Workbook != null; }
        }

        public bool? IsWorkbookReadOnly
        {
            get { return _Workbook?.ReadOnly; }
        }

        public void Dispose()
        {
            try
            {
                _Workbook?.Close(); // saved
                _App?.Quit();
            }
            catch { }
            _Workbook = null;
            _App = null;
        }
    }


    public class ExcelApiException : Exception
    {
        public ExcelApiException() : base() { }
        public ExcelApiException(string message) : base(message) { }
        public ExcelApiException(string message, Exception innerException) : base(message, innerException) { }
    }
}
