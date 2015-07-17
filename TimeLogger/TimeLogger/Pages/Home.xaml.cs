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
        }

        private void txtbox_start_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!LogItem.Validate(this.datetimepicker.DisplayDate, this.txtbox_start.Text.ToString(), this.txtbox_start.Text.ToString()))
                this.txtbox_start.Foreground = new SolidColorBrush(Colors.Red);
        }
    }
}
