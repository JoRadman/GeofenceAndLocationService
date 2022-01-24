# GeofenceAndLocationService

This is very simple Android app where you can receive location langitude and latitude, or to check whether you're inside geofence area or not.
But behind this simple app is very complicated c# code.

You can constantly receive latitude and longitude and save these information into some database. On this way you can check location of user.
You'll receive location update every 10 seconds. It's very important to allow app to use your location, without that it is not possible to use this location.

Also, one of the biggest problems was when the smarphone goes to sleep, for many solutions app then stops receiving location updates.
I solved this ith AlarmReceiver and Notification Service. On this way smatphone can go to sleep, but app will continue to receive location updates.

You can also assign geofence latitude and longitude and radius from that dot. Then app will check every time are you inside of that geofence are or outside of it.
I developed this for checking work time hours outside the company, where you don't have different devices and cards for checking work time.

Almost all apps today are using user's location, so this code has many applicabilites.
