using System;
using System.Collections.Generic;
using System.Text;

namespace GeofenceAndLocationService
{
    public interface IAutoStartInfo
    {
        void StartSetting();
        bool CheckIsXiaomi();
    }
}
