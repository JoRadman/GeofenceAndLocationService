using System;
using System.Collections.Generic;
using System.Text;

namespace GeofenceAndLocationService
{
    public interface IBatteryInfo
    {
        void StartSetting();
        bool CheckIsEnableBatteryOptimizations();
    }
}
