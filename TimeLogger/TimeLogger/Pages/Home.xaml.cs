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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
            TimeLoggerController tlc = new TimeLoggerController(@"C://Users/" + Environment.UserName + "/desktop/");
        }

        private void txtbox_start_TextChanged(object sender, TextChangedEventArgs e)
        {
            TimeSpan abc = new TimeSpan();
            if (!TimeSpan.TryParse(this.txtbox_start.Text, out abc))
                this.txtbox_start.Foreground = new SolidColorBrush(Colors.Red);
            else
                this.txtbox_start.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd1d1d1"));
        }

        private void txtbox_end_TextChanged(object sender, TextChangedEventArgs e)
        {
            TimeSpan abc = new TimeSpan();
            if (!TimeSpan.TryParse(this.txtbox_end.Text, out abc))
                this.txtbox_end.Foreground = new SolidColorBrush(Colors.Red);
            else
                this.txtbox_end.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd1d1d1"));
        }
    }
}
