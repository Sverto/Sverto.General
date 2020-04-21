using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Word;

namespace Sverto.General.Office
{
    public interface IWordInstance : IDisposable
    {
        IWordInstance OpenDocument(string path);
        void CloseDocument(bool save = true);
        List<string> ReadDocumentData();
        bool IsDocumentOpen { get; }
        bool IsVisible { get; set; }
    }


    public class WordInstance : IWordInstance
    {
        private Application _App;
        private Document _Doc;

        public WordInstance()
        {
            _App = new Application();
        }

        public IWordInstance OpenDocument(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException();
            if (_App == null)
                throw new InvalidOperationException("Word instance is not open.");
            if (_Doc != null)
                throw new InvalidOperationException("A document is already open.");

            _Doc = _App.Documents.Open(path);
            return this;
        }

        public void CloseDocument(bool save = true)
        {
            _Doc?.Close(save);
            _Doc = null;
        }

        public List<string> ReadDocumentData()
        {
            if (_Doc == null)
                throw new InvalidOperationException("Document is not open.");

            var data = new List<string>();
            foreach (Paragraph objParagraph in _Doc.Paragraphs)
                data.Add(objParagraph.Range.Text.Trim());
            return data;
        }

        public bool IsDocumentOpen
        {
            get { return _Doc != null; }
        }

        public bool IsVisible
        {
            get { return _App?.Visible ?? false; }
            set { if (_App != null) _App.Visible = value; }
        }

        public void Dispose()
        {
            try
            {
                _Doc?.Close();
                _App?.Quit(); // Can trigger RPC Exception if WINWORD process crashed 
            }
            catch { }
            _Doc = null;
            _App = null;
        }
    }
}
