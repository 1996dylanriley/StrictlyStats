using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace StrictlyStatistics.UIComponents
{
    public static class Alert
    {
        public static void ShowAlertWithSingleButton(Activity self, string title, string message, string buttonText)
        {
            Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(self);
            AlertDialog alert = dialog.Create();
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetButton(buttonText, (c, ev) =>
            {
                alert.Hide();
            });
            alert.Show();
        }
    }
}