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

        public static void ShowAlertWithTwoButtons(Activity self, string title, string message, string buttonText1, string buttonText2, Action but1Callback, Action but2Callback)
        {
            Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(self);
            AlertDialog alert = dialog.Create();
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetButton(buttonText1, (c, ev) =>
            {
                but1Callback();
                alert.Hide();
            });
            alert.SetButton2(buttonText2, (c, ev) =>
            {
                but2Callback();
                alert.Hide();
            });
            alert.Show();
        }
    }
}