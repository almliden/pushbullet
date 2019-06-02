using PushBullet.Common;
using PushBullet.Core.Entities;
using PushBullet.Infrastructure.ConfigurationFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PushBullet.Infrastructure
{
    static class Command
    {
        public static HttpResponseMessage Send(string accessToken, string baseUrl, IBullet bullet)
        {
            string json = "";
            switch (bullet.GetBulletType())
            {
                case BulletType.Note:
                    Note note = (Note)bullet;
                    //if (bullet.GetPushType() != PushType.NotSet)
                    //{
                    json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        device_iden = bullet.GetPushTarget(),
                        type = bullet.GetBulletType().ToString().ToLower(),
                        title = note.Title,
                        body = note.Body
                    });
                    //}
                    //else
                    //{
                    //    json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    //    {
                    //        type = note.PushType.ToString().ToLower(),
                    //        title = note.Title,
                    //        body = note.Body
                    //    });
                    //}
                    break;
                case BulletType.Link:
                    json = Newtonsoft.Json.JsonConvert.SerializeObject((Link)bullet);
                    //json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    //{

                    //});
                    break;
                case BulletType.File:
                    break;
                default:
                    break;
            }

            //Helpers helper = new Helpers();
            Uri uri = new Uri(baseUrl + Helper.BulletTypeUrl[bullet.GetBulletType()]);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = client.PostAsync(uri, httpContent).Result;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "PushBulletClient");
                client.DefaultRequestHeaders.Add("Access-Token", accessToken);

                // Do the actual request and await the response
                var httpResponse = client.PostAsync(uri, httpContent).Result;

                // If the response contains content we want to read it!
                if (httpResponse.Content != null)
                {
                    return httpResponse;

                    //var responseContent = httpResponse.Content.ReadAsStringAsync().Result;

                    //HttpWebResponse HttpWResp = (HttpWebResponse)HttpWReq.GetResponseAsync();
                    //using (var streamReader = new StreamReader(HttpWResp.GetResponseStream()))
                    //{
                    //    var result = streamReader.ReadToEnd();
                    //}


                    // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
                }
            }
            throw new Exception("Could not send request");
        }

        public async static Task<List<Device>> GetDevices(string accessToken, string baseUrl)
        {
            Uri uri = new Uri(baseUrl + "devices");
            List<Device> devices = new List<Device>();

 
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "PushBulletClient");
                client.DefaultRequestHeaders.Add("Access-Token", accessToken);


                string result;
                using (Stream strea = await client.GetStreamAsync(uri))
                {
                    using (StreamReader reader = new StreamReader(strea))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                DevicesResponse dr = Newtonsoft.Json.JsonConvert.DeserializeObject<DevicesResponse>(result);
                devices = dr.devices;

                return devices;

                //Stream stream = await client.GetStreamAsync(uri);
                //StreamReader streamReader = new StreamReader(stream);
                //string result = null;

                //while ((result = streamReader.ReadLine()) != null)
                //{

                //}



                //var httpResponse = client.GetAsync(uri).Result;
                
                //if (httpResponse.Content != null)
                //{


                //    //HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();
                //    string result;
                //    using (var stream = httpResponse.Content.ReadAsStreamAsync())
                //    {
                //        result = stream;
                //    }

                //    using (var streamReader = new StreamReader(HttpWResp.GetResponseStream()))
                //    {
                //        result = streamReader.ReadToEnd();
                //    }

                //    DevicesResponse dr = Newtonsoft.Json.JsonConvert.DeserializeObject<DevicesResponse>(result);
                //    devices = dr.devices;

                   
                //    //var responseContent = httpResponse.Content.ReadAsStringAsync().Result;

                //    //HttpWebResponse HttpWResp = (HttpWebResponse)HttpWReq.GetResponseAsync();
                //    //using (var streamReader = new StreamReader(HttpWResp.GetResponseStream()))
                //    //{
                //    //    var result = streamReader.ReadToEnd();
                //    //}


                    // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
                }

            throw new WebException("Could not get devices");
        }
    }
}
