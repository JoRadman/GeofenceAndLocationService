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

namespace GeofenceAndLocationService.Droid
{
    public class NotificationHelper : ContextWrapper
    {
        public const string PRIMARY_CHANNEL = "default";
        public const string SECONDARY_CHANNEL = "second";

        NotificationManager manager;
        NotificationManager Manager
        {
            get
            {
                if (manager == null)
                {
                    manager = (NotificationManager)GetSystemService(NotificationService);
                }
                return manager;
            }
        }

        public NotificationHelper(Context context) : base(context)
        {
            var chan1 = new NotificationChannel(PRIMARY_CHANNEL,
                    "mychannel", NotificationImportance.Default);
            chan1.LockscreenVisibility = NotificationVisibility.Private;
            Manager.CreateNotificationChannel(chan1);
        }

        public Notification.Builder GetNotification1()
        {
            return new Notification.Builder(ApplicationContext, PRIMARY_CHANNEL)
                     //.SetContentTitle(title)
                     //.SetContentText(body)
                     //.SetSmallIcon(SmallIcon)
                     .SetAutoCancel(true);
        }

        public void Notify(int id, Notification.Builder notification)
        {
            Manager.Notify(id, notification.Build());
        }
    }
}