using PushBullet.Core.Entities;
using PushBullet.Infrastructure;
using PushBullet.Infrastructure.ConfigurationFile;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace PushBullet
{
    public class Push
    {
        public IBullet Bullet { get; private set; }
        string accessToken;
        string baseUrl;
        string configPath;

        public Push(string accessToken, string baseUrl)
        {
            this.accessToken = accessToken;
            this.baseUrl = baseUrl;
        }

        public Push()
        {

        }

        public Push(string configPath)
        {
            this.configPath = configPath;
            ReadFromConfig();
        }

        private void ReadFromConfig()
        {
            this.accessToken = Configuration.ReadAccessToken(this.configPath);
            this.baseUrl = Configuration.ReadBaseUrl(this.configPath);
        }

        public string GetDefaultDevice()
        {
            return GetDefaultDevice(this.configPath);
        }

        public string GetDefaultDevice(string configPath)
        {
            return Configuration.ReadDefaultDeviceAsString(configPath);
        }

        public void ChangeConfigPath(string configPath)
        {
            this.configPath = configPath;
        }

        public void ReloadConfig()
        {
            ReadFromConfig();
        }

        public void SetPush(IBullet bullet)
        {
            this.Bullet = bullet;
        }

        public IBullet GetPush()
        {
            return this.Bullet;
        }

        public HttpResponseMessage Send()
        {
            return Command.Send(this.accessToken, this.baseUrl, this.Bullet );
        }

        public List<Device> GetDevices()
        {
            return Command.GetDevices(this.accessToken, this.baseUrl).Result;
        }
    }
}
