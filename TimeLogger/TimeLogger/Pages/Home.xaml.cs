using FirstFloor.ModernUI.Presentation;
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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
            datetimepicker.SelectedDate = DateTime.Today;
            AppearanceManager.Current.AccentColor = (Color)ColorConverter.ConvertFromString(TimeLoggerController.Instance.Settings.AccentColor);
        }

        private void txtbox_start_TextChanged(object sender, TextChangedEventArgs e)
        {
            TimeSpan abc = new TimeSpan();
            if (!TimeSpan.TryParse(txtbox_start.Text, out abc))
                txtbox_start.Foreground = new SolidColorBrush(Colors.Red);
            else
                txtbox_start.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd1d1d1"));
            //Setzt den Enabled Wert für den Action-Button abhängig davon ob die Inhalte beider Textboxen zulässige Eingaben sind
            action_button.IsEnabled = TimeLog.Validate(txtbox_start.Text) && TimeLog.Validate(txtbox_end.Text);
        }

        private void txtbox_end_TextChanged(object sender, TextChangedEventArgs e)
        {
            TimeSpan abc = new TimeSpan();
            if (!TimeSpan.TryParse(txtbox_end.Text, out abc))
            {
                txtbox_end.Foreground = new SolidColorBrush(Colors.Red);
                //this.txtbox_end.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                txtbox_end.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd1d1d1"));
                //this.txtbox_end.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd1d1d1"));
            }
            //Setzt den Enabled Wert für den Action-Button abhängig davon ob die Inhalte beider Textboxen zulässige Eingaben sind
            TimeSpan start = new TimeSpan();
            TimeSpan end = new TimeSpan();
            action_button.IsEnabled = TimeSpan.TryParse(txtbox_start.Text, out start) && TimeSpan.TryParse(txtbox_end.Text, out end);
        }

        private void datetimepicker_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (Mouse.Captured is Calendar || Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        private void datetimepicker_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeLog item = TimeLoggerController.Instance.GetLogItemByDate((DateTime)datetimepicker.SelectedDate);
            if (item != null)
            {
                txtbox_start.Text = item.Start;
                txtbox_end.Text = item.End;
            }
            else
            {
                txtbox_start.Text = string.Empty;
                txtbox_end.Text = string.Empty;
            }
        }

        private void action_button_Click(object sender, RoutedEventArgs e)
        {
            TimeLoggerController.Instance.WriteLog(new TimeLog(string.Format("{0};{1};{2}", (DateTime)datetimepicker.SelectedDate, txtbox_start.Text, txtbox_end.Text)));
            //action_button.Foreground = Brushes.LightGreen;

            FirstFloor.ModernUI.Windows.Controls.ModernDialog dialog = new FirstFloor.ModernUI.Windows.Controls.ModernDialog()
            {
                Content = "Eintrag erfolgreich gespeichert!",
                Title = "Speichervorgang"
            };
            dialog.ShowDialog();

        }

        private void txtbox_start_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtbox_start.Text = DateTime.Now.AddMinutes(-TimeLoggerController.Instance.Settings.AutoInsertTime).ToShortTimeString();
        }

        private void txtbox_end_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtbox_end.Text = DateTime.Now.AddMinutes(TimeLoggerController.Instance.Settings.AutoInsertTime).ToShortTimeString();
        }
    }
}
