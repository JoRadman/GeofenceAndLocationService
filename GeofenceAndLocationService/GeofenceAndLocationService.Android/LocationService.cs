using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace GeofenceAndLocationService.Droid
{
    [Service]
    public class LocationService : Service, ILocationListener
    {
        public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
        public event EventHandler<ProviderDisabledEventArgs> ProviderDisabled = delegate { };
        public event EventHandler<ProviderEnabledEventArgs> ProviderEnabled = delegate { };
        public event EventHandler<StatusChangedEventArgs> StatusChanged = delegate { };

        private int wakelockcount = 0;
        private System.Timers.Timer _timer = null;
        private PowerManager.WakeLock wakelock = null;

        System.Timers.Timer upisiPodatke = new System.Timers.Timer();

        NotificationHelper helper;

        public LocationService()
        {
        }

        // Set our location manager as the system location service
        protected LocationManager LocMgr = Android.App.Application.Context.GetSystemService("location") as LocationManager;

        readonly string logTag = "LocationService";
        IBinder binder;

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Debug(logTag, "OnCreate called in the Location Service");

            StartForeground(12345678, GetNotification());

            PowerManager pmanager = (PowerManager)this.GetSystemService("power");
            wakelock = pmanager.NewWakeLock(WakeLockFlags.Partial, "servicewakelock");
            wakelock.SetReferenceCounted(false);
        }

        private Notification GetNotification()
        {
            helper = new NotificationHelper(this);
            return helper.GetNotification1().Build();

            //NotificationChannel channel = new NotificationChannel("channel_01", "My Channel", NotificationImportance.Default);

            //NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
            //notificationManager.CreateNotificationChannel(channel);

            //Notification.Builder builder = new Notification.Builder(ApplicationContext, "channel_01").SetAutoCancel(true);
            //return builder.Build();
        }

        // This gets called when StartService is called in our App class
        [Obsolete("deprecated in base class")]
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Debug(logTag, "LocationService started");

            SetWakeLock();

            AlarmManager manager = (AlarmManager)GetSystemService(AlarmService);
            long triggerAtTime = SystemClock.ElapsedRealtime() + (20 * 1000);
            Intent alarmintent = new Intent(this, typeof(AlarmReceiver));

            PendingIntent pendingintent = PendingIntent.GetBroadcast(this, 0, alarmintent, 0);
            if (Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                manager.Cancel(pendingintent);
                manager.SetAndAllowWhileIdle(AlarmType.ElapsedRealtimeWakeup, triggerAtTime, pendingintent);
                Log.Info(logTag, "Alarm SetAndAllowWhileIdle Set");

            }
            else if (Android.OS.Build.VERSION.SdkInt == BuildVersionCodes.Kitkat || Android.OS.Build.VERSION.SdkInt == BuildVersionCodes.Lollipop)
            {
                manager.Cancel(pendingintent);
                manager.SetExact(AlarmType.ElapsedRealtimeWakeup, triggerAtTime, pendingintent);
            }
            timer(this, null);

            return StartCommandResult.Sticky;
        }

        public void SetWakeLock()
        {
            try
            {
                if (!wakelock.IsHeld)
                {
                    wakelock.Acquire();
                    wakelockcount += 1;
                    Log.Info(logTag, "WakeLock Armed");
                }
            }
            catch (Exception ex)
            {
                Log.Debug(this.ToString(), ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void timer(object sender, EventArgs e)
        {
            try
            {
                Log.Info(logTag, "Starting Timer");

                if (_timer != null)
                {
                    _timer.Enabled = false;
                    _timer.Dispose();
                    _timer = null;
                }
                _timer = new System.Timers.Timer();
                //Trigger event every 5 seconds
                _timer.Interval = 10000;
                _timer.Elapsed += OnTimeEvent; //new System.Timers.ElapsedEventHandler(OnTimeEvent);
                _timer.Enabled = true;
                _timer.AutoReset = true;
                _timer.Start();
            }
            catch (Exception ex)
            {
                Log.Debug(this.ToString(), ex.Message + "\n" + ex.StackTrace);
            }
        }

        private async void OnTimeEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            StartLocationUpdates();
        }

        // This gets called once, the first time any client bind to the Service
        // and returns an instance of the LocationServiceBinder. All future clients will
        // reuse the same instance of the binder
        public override IBinder OnBind(Intent intent)
        {
            Log.Debug(logTag, "Client now bound to service");

            binder = new LocationServiceBinder(this);
            return binder;
        }

        // Handle location updates from the location manager
        public void StartLocationUpdates()
        {
            try
            {
                LocMgr.RequestLocationUpdates("gps", Declare.TimerGeofence, 0, this);

                Log.Debug(logTag, "Now sending location updates");
            }
            catch (Exception ex)
            {

            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug(logTag, "Service has been terminated");

            StopForeground(true);

            // Stop getting updates from the location manager:
            LocMgr.RemoveUpdates(this);
        }


        // ILocationListener is a way for the Service to subscribe for updates
        // from the System location Service

        public async void OnLocationChanged(Android.Locations.Location location)
        {
            this.LocationChanged(this, new LocationChangedEventArgs(location));

            if(Declare.TrajeGeofence == true)
            {
                float[] dist = new float[1];

                Location.DistanceBetween(location.Latitude, location.Longitude, Convert.ToDouble(Declare.Longitude), Convert.ToDouble(Declare.Longitude), dist);

                if ((dist[0] / Declare.Radius > 1))
                {
                    DependencyService.Get<IMessage>().LongTime("You're outside marked area.");
                }
                else
                {
                    DependencyService.Get<IMessage>().LongTime("You're inside marked area");
                }
            }
            if(Declare.trajeGetLocation == true)
            {
                DependencyService.Get<IMessage>().LongTime("Your position is - Longitude: " + location.Longitude.ToString() + " , Latitude: " + location.Latitude.ToString());
            }
        }

        public async void OnProviderDisabled(string provider)
        {
            this.ProviderDisabled(this, new ProviderDisabledEventArgs(provider));
        }

        public void OnProviderEnabled(string provider)
        {
            this.ProviderEnabled(this, new ProviderEnabledEventArgs(provider));
        }

        public async void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            this.StatusChanged(this, new StatusChangedEventArgs(provider, status, extras));
        }
    }
}