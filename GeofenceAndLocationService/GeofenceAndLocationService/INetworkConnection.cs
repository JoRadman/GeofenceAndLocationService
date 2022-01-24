using System;
using System.Collections.Generic;
using System.Text;

namespace GeofenceAndLocationService
{
    public interface INetworkConnection
    {
        bool IsConnected { get; }
        void CheckNetworkConnection();
    }
}
