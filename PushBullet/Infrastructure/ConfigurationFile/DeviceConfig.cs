using PushBullet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushBullet.Infrastructure.ConfigurationFile
{
    public class DeviceConfig
    {
        public List<Device> devices { get; set; }
        public Device defaultDevice { get; set; }
    }
}
