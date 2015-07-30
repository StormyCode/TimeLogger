using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeLogger.Pages
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class BasicPage1 : UserControl
    {
        public BasicPage1()
        {
            InitializeComponent();

        }

        private void RichTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TimeLoggerController.GetInstance().ReadLogFile();
            richtxtbox.Document.Blocks.Clear();
            string txt = String.Empty;
            foreach (LogItem item in TimeLoggerController.GetInstance().LogList.OrderBy(o=>o.Date).Reverse().ToList())
            {
                if (txt.Length > 0)
                    txt += "\n";
                txt += String.Format("{0}\t{1}\t{2}\t{3}", item.Date.ToShortDateString(), item.Start.ToString(@"hh\:mm"), item.End.ToString(@"hh\:mm"), item.GetDifference().ToString(@"hh\:mm"));
            }
            richtxtbox.AppendText(txt);

            //Die GesamtDifferenz in das vorgesehene Label schreiben
            this.lbl_gesamtdiff.Content = TimeLoggerController.GetInstance().GetLogSum();
        }

        private void save_logfile_Click(object sender, RoutedEventArgs e)
        {
            TimeLoggerController.GetInstance().SaveLogFileToFile();
        }
    }
}
