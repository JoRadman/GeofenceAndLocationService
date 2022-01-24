using System;
using System.Collections.Generic;
using System.Text;

namespace GeofenceAndLocationService
{
    public interface IMessage
    {
        void LongTime(string message);
        void ShortTime(string message);
    }
}
