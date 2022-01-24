using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GeofenceAndLocationService
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btn_StartGettingLocation_Clicked(object sender, EventArgs e)
        {
            Declare.trajeGetLocation = true;
            Declare.TrajeGeofence = false;
            btn_StartGettingLocation.BackgroundColor = Color.Green;
            btn_StartCheckingGeofence.BackgroundColor = Color.Aqua;
        }

        private void btn_StartCheckingGeofence_Clicked(object sender, EventArgs e)
        {
            if(Declare.Latitude == 0 || Declare.Longitude == 0 || Declare.Radius == 0)
            {
                DependencyService.Get<IMessage>().LongTime("Please save geofence data.");
            }
            else
            {
                Declare.trajeGetLocation = false;
                Declare.TrajeGeofence = true;
                btn_StartGettingLocation.BackgroundColor = Color.Aqua;
                btn_StartCheckingGeofence.BackgroundColor = Color.Green;
            }
        }

        private void btn_SaveGeofenceData_Clicked(object sender, EventArgs e)
        {
            if(en_Latitude.Text != string.Empty && en_Longitude.Text != string.Empty && en_Radius.Text != string.Empty)
            {
                Declare.Latitude = Convert.ToDecimal(en_Latitude.Text);
                Declare.Longitude = Convert.ToDecimal(en_Longitude.Text);
                Declare.Radius = Convert.ToInt32(en_Radius.Text);
            }
            else
            {
                DependencyService.Get<IMessage>().LongTime("Please insert all geofence data.");
            }
        }
    }
}
