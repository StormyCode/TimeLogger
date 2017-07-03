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
using TimeLogger.Models;

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
            richtxtbox.Document.Blocks.Clear();
            string txt = string.Empty;
            foreach (TimeLog item in TimeLoggerController.Instance.Log.OrderByDescending(o=>o.Date))
            {
                if (txt.Length > 0)
                    txt += "\n";
                txt += string.Format("{0}\t{1}\t{2}\t{3}", item.Date.ToShortDateString(), item.Start, item.End != "00:00" ? item.End : "TBD", item.GetDifference().ToString(@"hh\:mm"));
            }
            richtxtbox.AppendText(txt);

            //Die GesamtDifferenz in das vorgesehene Label schreiben
            lbl_gesamtdiff.Content = TimeLoggerController.Instance.GetLogSum();
        }

        private void save_logfile_Click(object sender, RoutedEventArgs e)
        {
            TimeLoggerController.Instance.SaveLogFileToFile();
            System.Windows.Forms.MessageBox.Show("LogFile wurde erfolgreich exportiert!", "Export", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }
    }
}
