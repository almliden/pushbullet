using PushBullet.Infrastructure.ConfigurationFile;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushBullet.Infrastructure
{
    public class DevicesResponse
    {
        public List<object> accounts { get; set; }
        public List<object> blocks { get; set; }
        public List<object> channels { get; set; }
        public List<object> chats { get; set; }
        public List<object> clients { get; set; }
        public List<object> contacts { get; set; }
        public List<Device> devices { get; set; }
        public List<object> grants { get; set; }
        public List<object> pushes { get; set; }
        public List<object> profiles { get; set; }
        public List<object> subscriptions { get; set; }
        public List<object> texts { get; set; }
    }
}
