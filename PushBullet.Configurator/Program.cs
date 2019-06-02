using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace PushBullet.Configurator
{
    class Program
    {
        static void Main(string[] args)
        {
            Configurator configurator;
            if (args.Length > 0)
                configurator = new Configurator(args[0]);
            else if (args.Length == 2)
                configurator = new Configurator(args[0], args[1]);
            else
                configurator = new Configurator();
            configurator.Start();
        }
    }
}

