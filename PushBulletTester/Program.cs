using PushBullet;
using PushBullet.Core.Entities;
using System;
using System.Net;
using System.Net.Http;

namespace PushBulletTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Push pb = new Push("Configuration/Configuration.xml");
            Note note = new Note("Title", "Body", pb.GetDefaultDevice());
            pb.SetPush(note);
            HttpResponseMessage resp = pb.Send();

            Console.ReadKey();
        }
    }
}
