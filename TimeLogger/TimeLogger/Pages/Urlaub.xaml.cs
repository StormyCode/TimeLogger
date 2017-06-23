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
    /// Interaction logic for Urlaub.xaml
    /// </summary>
    public partial class Urlaub : UserControl
    {
        public Urlaub()
        {
            InitializeComponent();
            datetimepicker_vacation.SelectedDate = DateTime.Today;
            if (TimeLoggerController.Instance.GetRemainingVacationDays() > 0)
                vacation_type.Items.Add(new ComboBoxItem() { Content = "Urlaub" });
            vacation_type.Items.Add(new ComboBoxItem() { Content = "Gleittag" });
            vacation_type.Items.Add(new ComboBoxItem() { Content = "Arbeitstag" });
            vacation_type.Items.Add(new ComboBoxItem() { Content = "HomeOffice" });
        }

        private void datetimepicker_vacation_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TimeLoggerController.Instance.GetDateVacationType((DateTime)datetimepicker_vacation.SelectedDate) == VacationLog.VacationType.Work
                && (((DateTime)datetimepicker_vacation.SelectedDate).DayOfWeek == DayOfWeek.Saturday
                    || ((DateTime)datetimepicker_vacation.SelectedDate).DayOfWeek == DayOfWeek.Sunday))
                vacation_type.SelectedIndex = -1;
            else
            {
                switch (TimeLoggerController.Instance.GetDateVacationType((DateTime)datetimepicker_vacation.SelectedDate))
                {
                    case VacationLog.VacationType.Vacation:
                        vacation_type.SelectedIndex = 0;
                        break;
                    case VacationLog.VacationType.Flextime:
                        vacation_type.SelectedIndex = 1;
                        break;
                    case VacationLog.VacationType.Work:
                        vacation_type.SelectedIndex = 2;
                        break;
                    case VacationLog.VacationType.HomeOffice:
                        vacation_type.SelectedIndex = 3;
                        break;
                    default:
                        break;
                }
            }

        }

        private void vacation_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VacationLog.VacationType type = VacationLog.VacationType.Work;
            switch (vacation_type.SelectedIndex)
            {
                case 0:
                    type = VacationLog.VacationType.Vacation;
                    break;
                case 1:
                    type = VacationLog.VacationType.Flextime;
                    break;
                case 3:
                    type = VacationLog.VacationType.HomeOffice;
                    break;
                default:
                    break;
            }
            TimeLoggerController.Instance.UpdateVacationList((DateTime)datetimepicker_vacation.SelectedDate, type);
            lbl_resturlaub.Content = TimeLoggerController.Instance.GetRemainingVacationDays();
            lbl_gleittage.Content = TimeLoggerController.Instance.CountVacationType(VacationLog.VacationType.Flextime);
            lbl_homeoffice.Content = TimeLoggerController.Instance.CountVacationType(VacationLog.VacationType.HomeOffice);
        }
    }
}
