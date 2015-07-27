﻿using System;
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
        private TimeLoggerController tlc { get; set; }
        public Home()
        {
            InitializeComponent();
            tlc = new TimeLoggerController();
            this.datetimepicker.SelectedDate = DateTime.Today;
        }

        private void txtbox_start_TextChanged(object sender, TextChangedEventArgs e)
        {
            TimeSpan abc = new TimeSpan();
            if (!TimeSpan.TryParse(this.txtbox_start.Text, out abc))
                this.txtbox_start.Foreground = new SolidColorBrush(Colors.Red);
            else
                this.txtbox_start.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd1d1d1"));
            //Setzt den Enabled Wert für den Action-Button abhängig davon ob die Inhalte beider Textboxen zulässige Eingaben sind
            TimeSpan start = new TimeSpan();
            TimeSpan end = new TimeSpan();
            this.action_button.IsEnabled = TimeSpan.TryParse(this.txtbox_start.Text, out start) && TimeSpan.TryParse(this.txtbox_end.Text, out end);
        }

        private void txtbox_end_TextChanged(object sender, TextChangedEventArgs e)
        {
            TimeSpan abc = new TimeSpan();
            if (!TimeSpan.TryParse(this.txtbox_end.Text, out abc))
            {
                this.txtbox_end.Foreground = new SolidColorBrush(Colors.Red);
                //this.txtbox_end.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.txtbox_end.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd1d1d1"));
                //this.txtbox_end.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd1d1d1"));
            }
            //Setzt den Enabled Wert für den Action-Button abhängig davon ob die Inhalte beider Textboxen zulässige Eingaben sind
            TimeSpan start = new TimeSpan();
            TimeSpan end = new TimeSpan();
            this.action_button.IsEnabled = TimeSpan.TryParse(this.txtbox_start.Text, out start) && TimeSpan.TryParse(this.txtbox_end.Text, out end);
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
            LogItem item = this.tlc.GetLogItemByDate((DateTime)this.datetimepicker.SelectedDate);
            if (item != null)
            {
                this.txtbox_start.Text = item.Start.ToString(@"hh\:mm");
                this.txtbox_end.Text = item.End.ToString(@"hh\:mm");
            }
            else
            {
                this.txtbox_start.Text = String.Empty;
                this.txtbox_end.Text = String.Empty;
            }
        }

        private void action_button_Click(object sender, RoutedEventArgs e)
        {
            this.tlc.Log(new LogItem(String.Format("{0};{1};{2}", (DateTime)this.datetimepicker.SelectedDate, this.txtbox_start.Text, this.txtbox_end.Text)));
            action_button.Foreground = Brushes.Green;
        }
    }
}
