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
        private TimeLoggerController tlc { get; set; }
        public BasicPage1()
        {
            InitializeComponent();
            this.tlc = new TimeLoggerController(@"C://Users/" + Environment.UserName + "/desktop/");
        }

        private void RichTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.tlc.ReadLogFile();
            richtxtbox.Document.Blocks.Clear();
            string txt = String.Empty;
            foreach (LogItem item in this.tlc.LogList.OrderBy(o=>o.Date).Reverse().ToList())
            {
                if (txt.Length > 0)
                    txt += "\n";
                txt += String.Format("{0}\t{1}\t{2}", item.Date.ToShortDateString(), item.Start.ToString(@"hh\:mm"), item.End.ToString(@"hh\:mm"));
            }
            richtxtbox.AppendText(txt);
        }
    }
}
