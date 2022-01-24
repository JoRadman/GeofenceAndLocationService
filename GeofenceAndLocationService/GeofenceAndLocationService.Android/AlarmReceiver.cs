using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace GeofenceAndLocationService.Droid
{
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                context.StartService(new Intent(context, typeof(LocationService)));

                Log.Debug("Alarm", "Alarm started.");
            }
            catch (Exception ex)
            {
                Log.Debug(this.ToString(), ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}