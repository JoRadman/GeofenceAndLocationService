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

[assembly: Dependency(typeof(AutoStartImplementation))]
namespace GeofenceAndLocationService.Droid
{
    public class AutoStartImplementation : IAutoStartInfo
    {
        public bool CheckIsXiaomi()
        {
            String manufacturer = "xiaomi";
            String manufacturer1 = "Xiaomi";
            String manufacturer2 = "XIAOMI";

            String AndroidManufacturer = Android.OS.Build.Manufacturer;

            if (manufacturer.Equals(Android.OS.Build.Manufacturer) || manufacturer1.Equals(Android.OS.Build.Manufacturer) || manufacturer2.Equals(Android.OS.Build.Manufacturer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void StartSetting()
        {
            //this will open auto start screen where user can enable permission for your app
            Intent intent = new Intent();
            intent.SetComponent(new ComponentName("com.miui.securitycenter", "com.miui.permcenter.autostart.AutoStartManagementActivity"));
            Forms.Context.StartActivity(intent);
        }
    }
}