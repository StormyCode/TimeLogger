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
    /// Interaction logic for Urlaub.xaml
    /// </summary>
    public partial class Urlaub : UserControl
    {
        public Urlaub()
        {
            InitializeComponent();
        }

        private void datetimepicker_vacation_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (TimeLoggerController.GetInstance().GetDateVacationType((DateTime)this.datetimepicker_vacation.SelectedDate))
            {
                case TimeLoggerController.VacationType.Vacation:
                    this.vacation_type.SelectedIndex = 0;
                    break;
                case TimeLoggerController.VacationType.Flexitime:
                    this.vacation_type.SelectedIndex = 1;
                    break;
                case TimeLoggerController.VacationType.Work:
                    this.vacation_type.SelectedIndex = 2;
                    break;
                default:
                    break;
            }
            
        }
    }
}
