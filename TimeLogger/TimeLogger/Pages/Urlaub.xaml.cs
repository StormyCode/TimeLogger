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
            foreach (VacationType type in TimeLoggerController.Instance.VacationTypes)
            {
                if (type.Enabled)
                {
                    vacation_type.Items.Add(new ComboBoxItem() { Content = type.Name });
                }
            }
            UpdateTextBlock();
        }

        private void datetimepicker_vacation_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            bool found = false;
            VacationType type = TimeLoggerController.Instance.GetDateVacationType((DateTime)datetimepicker_vacation.SelectedDate);
            if (type != null)
            {
                for (int i = 0; i < vacation_type.Items.Count; i++)
                {
                    if (type.Name == ((ComboBoxItem)vacation_type.Items[i]).Content.ToString())
                    {
                        vacation_type.SelectedIndex = i;
                        found = true;
                        break;
                    }
                }
            }
            if (!found)
                vacation_type.SelectedIndex = -1;
        }

        private void vacation_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vacation_type.SelectedItem != null)
            {
                VacationType type = TimeLoggerController.Instance.GetVacationTypeByName(((ComboBoxItem)vacation_type.SelectedItem).Content.ToString());
                if (type != null && type.Enabled)
                {
                    TimeLoggerController.Instance.UpdateVacationList((DateTime)datetimepicker_vacation.SelectedDate, type);
                } 
            }

            UpdateTextBlock();
            //lbl_resturlaub.Content = TimeLoggerController.Instance.GetRemainingVacationDays();
            //lbl_gleittage.Content = TimeLoggerController.Instance.CountVacationType(VacationLog.VacationType.Flextime);
            //lbl_homeoffice.Content = TimeLoggerController.Instance.CountVacationType(VacationLog.VacationType.HomeOffice);
        }

        private void UpdateTextBlock()
        {
            StringBuilder sb = new StringBuilder();
            foreach (VacationType vtype in TimeLoggerController.Instance.VacationTypes)
            {
                if (vtype.Enabled && vtype.CountEnabled)
                {
                    sb.AppendLine(""+vtype.Name+": "+TimeLoggerController.Instance.CountVacationType(vtype.Name));
                }
            }
            textBlock.Text = sb.ToString();
        }
    }
}
