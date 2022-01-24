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

[assembly: Dependency(typeof(BatteryImplementation))]
namespace GeofenceAndLocationService.Droid
{
    public class BatteryImplementation : IBatteryInfo
    {
        public bool CheckIsEnableBatteryOptimizations()
        {

            PowerManager pm = (PowerManager)Android.App.Application.Context.GetSystemService(Context.PowerService);
            //enter you package name of your application
            bool result = pm.IsIgnoringBatteryOptimizations("com.companyname.geofenceandlocationservice");
            return result;
        }

        public void StartSetting()
        {
            Intent intent = new Intent();

            intent.SetAction(Android.Provider.Settings.ActionApplicationDetailsSettings);
            intent.SetData(Android.Net.Uri.Parse("package:" + Android.App.Application.Context.PackageName));
            //intent.SetAction(Android.Provider.Settings.ActionIgnoreBatteryOptimizationSettings);

            Forms.Context.StartActivity(intent);
            //StartActivity(intent);
        }
    }
}