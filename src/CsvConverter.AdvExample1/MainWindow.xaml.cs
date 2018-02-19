using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using CsvConverter.ClassToCsv;
using CsvConverter.CsvToClass;

namespace AdvExample1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Custom Converter
        private void CustomConverterCreateCsvButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = SaveFile(CustomConverterCSVFile.Text);
                if (dialog.ShowDialog() != true)
                    return;

                Random rand = new Random(DateTime.Now.Second);
                int numberToCreate = rand.Next(10, 100);

                using (var fs = File.Create(dialog.FileName))
                using (var sw = new StreamWriter(fs, Encoding.Default))
                {
                    var service = new ClassToCsvService<Person>(sw);
                    for (int i = 0; i < numberToCreate; i++)
                    {
                        var newPerson = new Person()
                        {
                            FirstName = $"First{rand.Next(1, 5000)}",
                            LastName = $"Last{rand.Next(1, 5000)}",
                            Age = rand.Next(5, 80),
                            PercentageBodyFat = rand.Next(1, 20) / 1.2m,
                            AvgHeartRate = rand.Next(60, 80) / 1.1
                        };

                        service.WriterRecord(newPerson);
                    }
                }

                LogMessage($"Created {numberToCreate} people in {dialog.FileName}.");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void CustomConverterLoadCsvButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = LoadFile(CustomConverterCSVFile.Text);
                if (dialog.ShowDialog() != true)
                    return;

                using (var fs = File.OpenRead(dialog.FileName))
                using (var sr = new StreamReader(fs, Encoding.Default))
                {
                    var csv = new CsvToClassService<Person>(sr);
                    csv.Configuration.IgnoreBlankRows = true;

                    while (csv.CanRead())
                    {
                        Person record = csv.GetRecord();
                        LogMessage(record.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void CustomConverterFindCsvFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (dialog.ShowDialog() != true)
                return;

            CustomConverterCSVFile.Text = dialog.FileName;
        }
        #endregion


        #region Custom Pre-Processor
        private void CustomPreprocessorCreateCsvButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = SaveFile(CustomPreprocessorCSVFile.Text);
                if (dialog.ShowDialog() != true)
                    return;

                Random rand = new Random(DateTime.Now.Second);
                int numberToCreate = rand.Next(10, 100);

                using (var fs = File.Create(dialog.FileName))
                using (var sw = new StreamWriter(fs, Encoding.Default))
                {
                    var service = new ClassToCsvService<Car>(sw);
                    for (int i = 0; i < numberToCreate; i++)
                    {
                        var newCar = new Car()
                        {
                            Make = rand.Next(1, 100) > 50 ? $"M{rand.Next(1, 5000000)}" : "M", 
                            Model = rand.Next(1, 100) > 50 ? $"M{rand.Next(1, 5000000)}" : "M",
                            Year = rand.Next(1995, 2018),
                        };

                        service.WriterRecord(newCar);
                    }
                }

                LogMessage($"Created {numberToCreate} cars in {dialog.FileName}.");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void CustomPreprocessorLoadCsvButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = LoadFile(CustomPreprocessorCSVFile.Text);
                if (dialog.ShowDialog() != true)
                    return;

                using (var fs = File.OpenRead(dialog.FileName))
                using (var sr = new StreamReader(fs, Encoding.Default))
                {
                    var csv = new CsvToClassService<Car>(sr);
                    csv.Configuration.IgnoreBlankRows = true;

                    while (csv.CanRead())
                    {
                        Car record = csv.GetRecord();
                        LogMessage(record.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void CustomPreprocessorFindCsvFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (dialog.ShowDialog() != true)
                return;

            CustomPreprocessorCSVFile.Text = dialog.FileName;
        }
        #endregion



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
