using PushBullet.Core.Entities;
using PushBullet.Infrastructure;
using PushBullet.Infrastructure.ConfigurationFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PushBullet.Configurator
{
    class Configurator
    {
        private string ConfigPath;
        private string DevicesPath = "Configuration/devices.json";

        public Configurator()
        {
        }
        public Configurator(string configPath)
        {
            this.ConfigPath = configPath;
        }
        public Configurator(string configPath, string devicesPath)
        {
            this.ConfigPath = configPath;
            this.DevicesPath = devicesPath;
        }
        public void Start()
        {
            if (string.IsNullOrEmpty(this.ConfigPath) || !File.Exists(ConfigPath))
            {
                SetConfigPath();
            }
            MainMenu();
        }
        private void SetConfigPath()
        {
            Console.WriteLine("Enter path (q to skip):");
            ConfigPath = Console.ReadLine().Trim().ToLower();
            if (ConfigPath != "q")
            {
                if (!File.Exists(ConfigPath))
                {
                    Console.WriteLine("File or path does not exist. Create config [y/n]?");
                    if (Console.ReadLine().ToString().ToLower() == "y")
                        File.Create(ConfigPath);
                    else
                        SetConfigPath();
                }
            }
        }

        void MainMenu()
        {
            bool quit = false;
            while (!quit)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Main menu:\n1. Show Access-Token\n2. Set Access-Token\n3. List devices\n4. Show Default Device\n5. Set Detfault Device.\n6. Send Push\nc. Change config file\nq. Exit program");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowAccessToken();
                        break;
                    case "2":
                        SetAccessToken();
                        break;
                    case "3":
                        ListDevicesMenu();
                        break;
                    case "4":
                        ShowDefaultDevice();
                        break;
                    case "5":
                        SetDefaultDevice();
                        break;
                    case "6":
                        SendPush();
                        break;
                    case "c":
                        SetConfigPath();
                        break;
                    case "q":
                        quit = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Could not understand your choice, please try again.");
                        break;
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        bool ConfigFileExists(string path)
        {
            return File.Exists(path);
        }

        static void PrintDevice(Device device)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(String.Format("Device\nNickname: {0}\nModel: {1}\nManufacturer: {2}\nIden: {3}\nActive: {4}", device.nickname, device.model, device.manufacturer, device.iden, device.active));
            Console.ForegroundColor = ConsoleColor.White;
        }
        void ShowAccessToken()
        {
            if (ConfigFileExists(ConfigPath))
            {
                Console.WriteLine(Configuration.ReadAccessToken(ConfigPath));
            }
            else
                SetConfigPath();
        }
        void SetAccessToken()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("New Access-Token: ");
            Configuration.WriteAccessToken(Console.ReadLine());
            Console.ForegroundColor = ConsoleColor.White;
        }

        List<Device> ListDevicesOnline()
        {
            Push push = new Push(ConfigPath);
            List<Device> devices = null;
            devices = push.GetDevices();
            return devices;
        }

        List<Device> ListDevicesConfig()
        {
            Push push = new Push(ConfigPath);
            List<Device> devices = null;
            if (!ConfigFileExists(DevicesPath))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Devices path not set, creating file");
                File.Create(DevicesPath);
                Console.ForegroundColor = ConsoleColor.White;
            }
            devices = Configuration.ReadDevicesFromConfig(DevicesPath);
            return devices;
        }

        void ListDevicesMenu()
        {
            List<Device> devices = null;
            bool exit = false;
            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" List Devices\n 1. Online\n 2. Config\n 3. Update config\n q. Quit from Devices");
                string s = Console.ReadLine();
                switch (s)
                {
                    case "1":
                        devices = ListDevicesOnline();
                        if (devices != null)
                        {
                            Console.WriteLine("Devices online: \n");
                            ListDevices(devices);
                            Console.WriteLine(" Update config-file [y/n]?");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("No devices online");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    case "2":
                        devices = ListDevicesConfig();
                        if (devices != null)
                        {
                            Console.WriteLine("Devices in config: \n");
                            ListDevices(devices);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("No devices in config");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    case "3":
                        if (devices == null)
                            Console.WriteLine("Fetch devices first");
                        else
                            Configuration.WriteDevicesToConfig(DevicesPath, devices);
                        break;
                    case "q":
                        exit = true;
                        break;
                    default:
                        break;
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        void SetDefaultDevice()
        {
            bool exit = false;
            List<Device> devices = null;
            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Get devices from\n1. Online\n2. Config\nq. Quit from DefaultDevice");
                string s = Console.ReadLine();
                bool devicesSuccess = false;
                switch (s)
                {
                    case "1":
                        devices = ListDevicesOnline().Where(x => x.pushable).ToList();
                        devicesSuccess = devices != null && devices.Count > 0;
                        break;
                    case "2":
                        devices = ListDevicesConfig().Where(x => x.pushable).ToList();
                        devicesSuccess = devices != null && devices.Count > 0;
                        break;
                    case "q":
                        exit = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Could not understand");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
                if (devicesSuccess)
                {
                    Console.WriteLine("Choose a device:");
                    ListDevices(devices);
                    string s2 = Console.ReadLine();

                    int index;
                    if (int.TryParse(s2, out index))
                    {
                        if (index <= devices.Count && index > 0)
                        {
                            index--;
                            Configuration.WriteDefaultDevice(ConfigPath, devices[index]);
                            Console.WriteLine("Updated default device");
                            PrintDevice(devices[index]);
                        }
                    }
                }
            }
        }
        void ShowDefaultDevice()
        {
            Console.WriteLine("Default device is set to;");
            Device device = Configuration.ReadDefaultDeviceAsDevice(ConfigPath);
            PrintDevice(device);
        }
        
        void ListDevices(List<Device> devices)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (devices.Count > 0)
            {
                int i = 1;
                foreach (Device d in devices)
                {
                    if (d.pushable)
                    {
                        Console.WriteLine(String.Format("{0}. {1} {2}", i, d.manufacturer, d.nickname));
                        i++;
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        void SendPush()
        {
            if (ConfigFileExists(ConfigPath))
            {
                Device device = Configuration.ReadDefaultDeviceAsDevice(ConfigPath);
                if (device != null)
                {
                    Console.WriteLine("Default device:");
                    PrintDevice(device);

                    string title, body;
                    Console.WriteLine("Title:");
                    title = Console.ReadLine();
                    Console.WriteLine("Message:");
                    body = Console.ReadLine();

                    Push p = new Push(ConfigPath);
                    Note note = new Note(title, body, device.iden);

                    Console.WriteLine("Send? [y/q]");
                    string send = Console.ReadLine();
                    if (send.ToLower() == "y")
                    {
                        p.SetPush(note);
                        p.Send();
                    }
                    else
                    {
                        Console.WriteLine("Did not send");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Device not selected, please select device to push to");
                    Console.ForegroundColor = ConsoleColor.White;
                    SetDefaultDevice();
                    SendPush();
                }
            }
        }
    }
}


//if (string.IsNullOrEmpty(UserConfiguration.ReadAccessToken()))
//{
//    Console.ForegroundColor = ConsoleColor.Red;
//    Console.WriteLine("Access token not set, please enter");
//    string s = Console.ReadLine();
//    UserConfiguration.WriteAccessToken(s);
//}

//DeviceId.iden = UserConfiguration.ReadDefaultDeviceAsString();
//if (!string.IsNullOrWhiteSpace(DeviceId.iden))
//{
//    List<Device> devices = UserConfiguration.ReadDevicesFromConfig();
//    if (devices.Count > 0)
//    {
//        Device d = devices.Find(x => x.iden == DeviceId.iden);
//        if (d != null)
//        {
//            DeviceId = d;
//        }
//    }
//}

////Device d = new Device();
////d.iden = "asdfasdf";
////d.nickname = "asdfasdf";
////d.active = true;
////UserConfiguration.WriteDefaultDevice(d);


//IConfiguration configuration = UserConfiguration.Read();
//Notify notify = new Notify(configuration);