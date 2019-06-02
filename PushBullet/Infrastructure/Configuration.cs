using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PushBullet.Common;
using PushBullet.Core.Entities;
using PushBullet.Infrastructure.ConfigurationFile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace PushBullet.Infrastructure
{
    public static class Configuration
    {
        public static IConfiguration Read()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddXmlFile("Configuration/Configuration.xml", optional: false, reloadOnChange: true);
                return builder.Build();
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine(ex, "exception");
                Console.WriteLine("Program failed: " + ex);
                Console.ReadKey();
                throw (ex);
            }
        }

        public static void WriteDevicesToConfig(string path, List<Device> devices)
        {
            try
            {
                using (StreamWriter writer = System.IO.File.CreateText(path))
                {
                    bool first = true;
                    writer.WriteLine("{\"devices\":[");
                    foreach (Device d in devices)
                    {
                        if (!first)
                            writer.Write(",");
                        else
                            first = false;
                        writer.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(d));
                    }
                    writer.WriteLine("]}"); //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong in WriteDevicesToConfig");
            }
        }

        public static void WriteAccessToken(string token)
        {
            //string s = Extensions.Serialize<Device>(device);
            //XElement xe = XElement.Parse(s);
            //var val = xe.ToString();

            XDocument xdoc = XDocument.Load("Configuration/Configuration.xml");
            var element = xdoc.Root.Elements("AccessToken").SingleOrDefault();
            if (element != null)
            {

                //var element = 
                //    (from x in xdoc.Descendants()
                //     var element = xdoc.Elements("bookPrice").Single();

                //     Changed ("DefaultDevice")
                //     select x).Single();

                //Elements("DefaultDevice").Single();
                //element.SetValue(device); // writes the object
                //element.SetValue(val); //&lt;  &gt;
                element.SetValue(token);

                ////element.Value = jsonDevice;
                xdoc.Save("Configuration/Configuration.xml");
            }
            //else
            //{
            //    //not sure if this would work though
            //    var parent = xdoc.Root;
            //    parent.AddAfterSelf(xe);
            //    xdoc.Save("Configuration/Configuration.xml");
            //}
        }

        public static string ReadBaseUrl(string path)
        {
            string s = File.ReadAllText(path);
            string accessToken = "";

            XElement xdoc = XElement.Parse(s);
            accessToken =
                (
                    from x in xdoc.Elements("BaseUri")
                    select x.Value
                ).FirstOrDefault();

            return accessToken;
        }

        //public static string ReadBaseUrl()
        //{
        //    return ReadBaseUrl("Configuration/Configuration.xml");
        //}

        //public static string ReadAccessToken()
        //{
        //    return ReadAccessToken("Configuration/Configuration.xml");
        //}

        public static string ReadAccessToken(string path)
        {
            string s = File.ReadAllText(path);
            string accessToken = "";

            XElement xdoc = XElement.Parse(s);
            accessToken =
                (
                    from x in xdoc.Elements("AccessToken")
                    select x.Value
                ).FirstOrDefault();

            return accessToken;
        }

        public static List<Device> ReadDevicesFromConfig(string path)
        {
            List<Device> devices = new List<Device>();
            try
            {
                // read file into a string and deserialize JSON to a type
                //DeviceConfig deviceConfig = JsonConvert.DeserializeObject<DeviceConfig>(File.ReadAllText(@"devices.json"));

                // deserialize JSON directly from a file
                using (StreamReader file = File.OpenText(path))
                //using (StreamReader file = File.OpenText(@"devices.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    DeviceConfig deviceConfig = (DeviceConfig)serializer.Deserialize(file, typeof(DeviceConfig));
                    return deviceConfig.devices;
                }
                //return deviceConfig.devices;


                //string a = "";
                ////using (StreamWriter writer = System.IO.File.AppendText("devices.json"))
                //using (StreamReader reader = new StreamReader("devices.json"))
                //{
                //    while (!reader.EndOfStream)
                //    {
                //        string s = reader.ReadLine();
                //        a += s;
                //        //try
                //        //{
                //        //    devices.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<Device>(s));
                //        //}
                //        //catch (Exception ex)
                //        //{
                //        //    Console.WriteLine("Can´t read, " + ex.InnerException.ToString());
                //        //}
                //    }
                //    try
                //    {

                //        JsonArray 
                //        devices = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Device>>(a);
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine("Something went wrong reading cofig " + ex.InnerException.ToString());
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong in ReadDevicesFromConfig");
            }
            return devices;
        }

        public static Device ReadDefaultDeviceAsDevice(string path)
        {
            string s = File.ReadAllText(path);
            Device loadedDevice = new Device();

            XElement xdoc = XElement.Parse(s);
            var devices =
                //from x in xdoc.Elements("Device")
                from x in xdoc.Elements("DefaultDevice")
                select x;

            var devss =
                from x in devices.Elements("Device")
                select x;

            foreach (XElement xelement in devss)
            {
                if (xelement != null && xelement.HasElements)
                {
                    loadedDevice = new Device().FromXml(xelement);
                }
            }
            return loadedDevice;
        }

        public static string ReadDefaultDeviceAsString(string path)
        {
            string s = File.ReadAllText(path);
            string loadedDevice = "";

            XElement xdoc = XElement.Parse(s);
            var devices =
                from x in xdoc.Elements("DefaultDevice")
                select x;

            foreach (XElement xelement in devices)
            {
                if (xelement != null && xelement.HasElements)
                {
                    loadedDevice = xelement.Value;
                }
            }
            return loadedDevice;
        }

        public static void WriteDefaultDevice(string path, Device device)
        {
            //XElement d = XElement.Parse();
            string s = SerializerExtensions.Serialize<Device>(device);
            XElement xe = XElement.Parse(s);
            var val = xe.ToString();

            XDocument xdoc = XDocument.Load(path);
            var element = xdoc.Root.Elements("DefaultDevice").SingleOrDefault();
            if (element != null)
            {
                var currentDefault = element.Elements("Device").FirstOrDefault();
                if (currentDefault != null)
                    currentDefault.Remove();   
                //element.SetElementValue("Device", device.iden);
                //element.SetElementValue("Device", s);
                element.Add(xe);
                //element.SetElementValue("Device", xe);
                xdoc.Save(path);
            }
            else
            {
                //not sure if this would work though
                var parent = xdoc.Root;
                parent.AddAfterSelf(xe);
                xdoc.Save(path);
            }
        }
    }
}
