using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TimeLogger
{
    class CustomDialog
    {
        /// <summary>Simple input dialog box</summary>
        /// <returns>null if the dialog box was closed with "cancel",
        /// otherwise the user input<returns>
        public static string ShowInputDialog(string initialTitle = "", string initialText = "", string initialInput = null)
        {
            // create the dialog content
            TextBox content = new TextBox()
                {
                    VerticalAlignment = VerticalAlignment.Bottom
                };
            content.Text = initialInput;
            Label lbl = new Label()
            {
                Content = initialText,
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid g = new Grid()
                {
                    Height = 50
                };
            g.Children.Add(content);
            g.Children.Add(lbl);
            // create the ModernUI dialog component with the buttons
            var dlg = new ModernDialog
            {
                Title = initialTitle,
                ShowInTaskbar = false,
                Content = g,
                MinHeight = 0,
                MinWidth = 0,
                MaxHeight = 480,
                MaxWidth = 640,
            };
            dlg.Buttons = new Button[] { dlg.OkButton, dlg.CancelButton };

            // register the event to retrieve the result of the dialog box 
            string result = null;
            dlg.OkButton.Click += (object sender, RoutedEventArgs e) =>
            {
                result = content.Text;
            };

            dlg.ShowDialog();
            return result;
        }
    }
}
