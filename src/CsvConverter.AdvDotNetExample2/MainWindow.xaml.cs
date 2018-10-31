using CsvConverter;
using CsvConverter.Mapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace AdvExample2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = SaveFile("c:\\temp\\Person.csv");
                if (dialog.ShowDialog() != true)
                    return;

                using (var fs = File.Create(dialog.FileName))
                using (var sw = new StreamWriter(fs))
                {
                    var writer = new CsvWriterService<Person>(sw);
                    writer.WriterRecord(new Person() { FirstName = "Tom", LastName = "Yo", Age = 90 });
                    writer.WriterRecord(new Person() { FirstName = "Jame", LastName = "Adam", Age = 34 });
                    writer.WriterRecord(new Person() { FirstName = "Killroy", LastName = "Back", Age = 12 });
                    writer.WriterRecord(new Person() { FirstName = "James", LastName = "Madison", Age = 23 });
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = LoadFile("c:\\temp\\Person.csv");
                if (dialog.ShowDialog() != true)
                    return;

                LogMessage("Before overriding");
                using (var fs = File.OpenRead(dialog.FileName))
                using (var sr = new StreamReader(fs))
                {
                    var reader = new CsvReaderService<Person>(sr);

                    while (reader.CanRead())
                    {
                        var person = reader.GetRecord();
                        LogMessage(person.ToString());
                    }
                }

                LogMessage("After overriding");
                using (var fs = File.OpenRead(dialog.FileName))
                using (var sr = new StreamReader(fs))
                {
                    // Keep in mind that your replacement converter must be standalone.
                    // You cannot rely in any default converters.
                    var reader = new CsvReaderService<Person>(sr);
                    reader.DefaultConverterFactory.ReplaceConverter(typeof(string), typeof(CsvConverterStringTextLengthEnforcer));

                    // Create all the converters.
                    reader.Init();

                    // Also, you will have to find the converter after initialize to tweak converter settings since
                    // settings are normally passed in with attributes and default converters don't use attributes.
                    //List<ColumnToPropertyMap> columnList = reader.ColumnMapList
                    //    .Where(w => w.PropInformation.PropertyType == typeof(string))                        
                    //    .ToList();
                    // OR
                    List<ColumnToPropertyMap> columnList = reader.ColumnMapList
                       .Where(w => w.ReadConverter.GetType() == typeof(CsvConverterStringTextLengthEnforcer))
                       .ToList();

                    foreach (var item in columnList)
                    {
                        var converter = item.ReadConverter as CsvConverterStringTextLengthEnforcer;
                        if (converter != null)
                        {
                            converter.MinimumLength = 5;
                            converter.MaximumLength = 6;
                            converter.CharacterToAddToShortStrings = '-';
                        }
                    }


                    while (reader.CanRead())
                    {
                        var person = reader.GetRecord();
                        LogMessage(person.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
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
