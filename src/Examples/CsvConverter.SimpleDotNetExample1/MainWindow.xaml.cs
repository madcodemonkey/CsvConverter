﻿using CsvConverter;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace SimpleDotNetExample1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateCsvButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = SaveFile();
                if (dialog.ShowDialog() != true)
                    return;

                Random rand = new Random(DateTime.Now.Second);
                int numberToCreate = rand.Next(10, 100);

                using (var fs = File.Create(dialog.FileName))
                using (var sw = new StreamWriter(fs, Encoding.Default))
                {
                    var writerService = new CsvWriterService<Employee>(sw);
                    for (int i = 0; i < numberToCreate; i++)
                    {
                        var newEmp = new Employee()
                        {
                            FirstName = $"First{rand.Next(1, 5000)}",
                            LastName = $"Last{rand.Next(1, 5000)}",
                            Age = rand.Next(5, 80),
                            PercentageBodyFat = rand.Next(1, 20) / 1.2m,
                            AvgHeartRate = rand.Next(60, 80) / 1.1
                        };

                        writerService.WriteRecord(newEmp);
                    }
                }

                LogMessage($"Created {numberToCreate} employees in {dialog.FileName}.");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void LoadCsvButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.InitialDirectory = string.IsNullOrEmpty(CSVFile.Text) ?
                "c:\\" : Path.GetDirectoryName(CSVFile.Text);
                dialog.FileName = CSVFile.Text;
                if (dialog.ShowDialog() != true)
                    return;

                using (var fs = File.OpenRead(dialog.FileName))
                using (var sr = new StreamReader(fs, Encoding.Default))
                {
                    var csv = new CsvReaderService<Employee>(sr);
                    csv.Configuration.BlankRowsAreReturnedAsNull = true;

                    while (csv.CanRead())
                    {
                        Employee record = csv.GetRecord();
                        LogMessage(record.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void FindCsvFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = SaveFile();
            if (dialog.ShowDialog() != true)
                return;

            LogMessage(dialog.FileName);
        }

        private Microsoft.Win32.SaveFileDialog SaveFile()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.InitialDirectory = string.IsNullOrEmpty(CSVFile.Text) ?
                "c:\\" : Path.GetDirectoryName(CSVFile.Text);
            dialog.FileName = CSVFile.Text;
            return dialog;
        }


        #region Logging
        private delegate void NoArgsDelegate();

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearLog();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveLog();
        }

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
