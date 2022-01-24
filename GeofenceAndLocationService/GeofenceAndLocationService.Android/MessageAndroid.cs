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
using GeofenceAndLocationService.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(MessageAndroid))]
namespace GeofenceAndLocationService.Droid
{
    public class MessageAndroid : IMessage
    {
        public void LongTime(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
           Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show()
           );
        }

        public void ShortTime(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}