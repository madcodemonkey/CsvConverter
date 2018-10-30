using CsvConverter;
using CsvConverter.RowTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace AdvExample3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RowReaderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // The RowReader class can parse strings for you directly and handle
                // quotes and other things you will encounter in a CSV file
                using (MemoryStream ms = new MemoryStream())
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    sw.WriteLine("This,is,\"a row with, commas\",in,     it");
                    sw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);

                    using (StreamReader sr = new StreamReader(ms))
                    {
                        var reader = new RowReader(sr);
                        while (reader.CanRead())
                        {
                            PrintColumnList(reader.ReadRow());
                        }
                    }                     
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void RowWriterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> stringList = new List<string>();
                stringList.Add("This");
                stringList.Add("is");
                stringList.Add("a");
                stringList.Add("quote, comma");
                stringList.Add("test");

                using (MemoryStream ms = new MemoryStream())
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    var writer = new RowWriter(sw);
                    writer.Write(stringList);
                    sw.Flush();

                    ms.Seek(0, SeekOrigin.Begin);
                    using(StreamReader sr = new StreamReader(ms))
                    {
                        while(sr.EndOfStream == false)
                        {
                            LogMessage(sr.ReadLine());
                        }
                    }

                }
                    
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }


        private void BothButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var columnList = new List<string>();

                List<string> stringList = new List<string>();
                stringList.Add("jack, says");
                stringList.Add("he ");
                stringList.Add("ran");
                stringList.Add("over");
                stringList.Add("John's");
                stringList.Add("foot");

                using (MemoryStream ms = new MemoryStream())
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    var writer = new RowWriter(sw);
                    writer.Write(stringList);
                    sw.Flush();

                    ms.Seek(0, SeekOrigin.Begin);
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        var reader = new RowReader(sr);
                        while (reader.CanRead())
                        {
                            PrintColumnList(reader.ReadRow());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void PrintColumnList(List<string> columnList)
        {
            foreach (string column in columnList)
            {
                LogMessage($"Column data --->{column}<---");
            }
        }

        private Microsoft.Win32.OpenFileDialog LoadFile(string fileName)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.InitialDirectory = string.IsNullOrEmpty(fileName) ?
            "c:\\" : Path.GetDirectoryName(fileName);
            dialog.FileName = fileName;
            return dialog;
        }

        private Microsoft.Win32.SaveFileDialog SaveFile(string fileName)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.InitialDirectory = string.IsNullOrEmpty(fileName) ?
                "c:\\" : Path.GetDirectoryName(fileName);
            dialog.FileName = fileName;
            return dialog;
        }

        #region Logging
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearLog();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveLog();
        }

        private delegate void NoArgsDelegate();
        private void ClearLog()
        {
            if (Dispatcher.CheckAccess())
            {
                RtbLog.Document.Blocks.Clear();
            }
            else this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NoArgsDelegate(ClearLog));
        }

        /// <summary>Threadsafe logging method.</summary>
        private void LogMessage(string message)
        {
            if (Dispatcher.CheckAccess())
            {
                var p = new Paragraph(new Run(message));
                p.Foreground = Brushes.Black;
                RtbLog.Document.Blocks.Add(p);
            }
            else this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<string>(LogMessage), message);
        }

        private void LogError(Exception ex)
        {
            if (Dispatcher.CheckAccess())
            {
                // We are back on the UI thread here so calling LogMessage will not cause a BeginInvoke for all these LogMessage calls:
                LogMessage(ex.Message);
                LogMessage(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    LogMessage(ex.InnerException.Message);
                    LogMessage(ex.InnerException.StackTrace);
                }
            }
            else this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<Exception>(LogError), ex);
        }

        private void SaveLog()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            if (dialog.ShowDialog() != true)
                return;

            using (var fs = new FileStream(dialog.FileName, FileMode.Create))
            {
                var myTextRange = new TextRange(RtbLog.Document.ContentStart, RtbLog.Document.ContentEnd);
                myTextRange.Save(fs, DataFormats.Text);
            }
        }



        #endregion

    
    }
}
