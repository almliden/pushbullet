using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PushBullet.Infrastructure.ConfigurationFile
{
    public class Device
    {
        public bool active { get; set; }
        public string iden { get; set; }
        public double created { get; set; }
        public double modified { get; set; }
        public string type { get; set; }
        public string kind { get; set; }
        public string nickname { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
        public int app_version { get; set; }
        public bool pushable { get; set; }
        public string icon { get; set; }
        public bool? generated_nickname { get; set; }
        public string fingerprint { get; set; }
        public string push_token { get; set; }
        public string key_fingerprint { get; set; }
        public string remote_files { get; set; }

        public Device FromXml(XElement xml)
        {
            Device device = new Device();
            device.active = Convert.ToBoolean(xml.Element("active").Value);
            device.iden = Encoding.UTF8.GetString(Encoding.Default.GetBytes(xml.Element("iden").Value.ToString()));
            device.created = Convert.ToDouble(xml.Element("created").Value.Replace('.', ','));
            device.modified = Convert.ToDouble(xml.Element("modified").Value.Replace('.', ','));
            device.type = Encoding.UTF8.GetString(Encoding.Default.GetBytes(xml.Element("type").Value.ToString()));
            device.kind = Encoding.UTF8.GetString(Encoding.Default.GetBytes(xml.Element("kind").Value.ToString()));
            device.nickname = Encoding.UTF8.GetString(Encoding.Default.GetBytes(xml.Element("nickname").Value.ToString()));
            device.manufacturer = Encoding.UTF8.GetString(Encoding.Default.GetBytes(xml.Element("manufacturer").Value.ToString()));
            device.model = Encoding.UTF8.GetString(Encoding.Default.GetBytes(xml.Element("model").Value.ToString()));
            device.app_version = Convert.ToInt32(xml.Element("app_version").Value);
            device.pushable = xml.Element("pushable").Value == "true" ? true : false;
            device.icon = Encoding.UTF8.GetString(Encoding.Default.GetBytes(xml.Element("icon").Value.ToString()));
            if (!string.IsNullOrEmpty(xml.Element("generated_nickname").Value))
            {
                device.generated_nickname = xml.Element("generated_nickname").Value == "true" ? true : false;
            }

            if (xml.Elements("fingerprint").FirstOrDefault() != null)
                device.fingerprint = Encoding.UTF8.GetString(Encoding.Default.GetBytes(xml.Element("fingerprint").Value.ToString()));

            //if (!string.IsNullOrEmpty(xml.Element("fingerprint").Value))
            //    device.fingerprint = Encoding.UTF8.GetString(Encoding.Default.GetBytes(xml.Element("fingerprint").Value.ToString()));
            if (xml.Elements("push_token").FirstOrDefault() != null)
                device.push_token = Encoding.UTF8.GetString(Encoding.Default.GetBytes(xml.Element("push_token").Value.ToString()));
            if (xml.Elements("key_fingerprint").FirstOrDefault() != null)
                device.key_fingerprint = Encoding.UTF8.GetString(Encoding.Default.GetBytes(xml.Element("key_fingerprint").Value.ToString()));
            if (xml.Elements("remote_files").FirstOrDefault() != null)
                device.remote_files = Encoding.UTF8.GetString(Encoding.Default.GetBytes(xml.Element("remote_files").Value.ToString()));

            return device;
        }
    }
}
