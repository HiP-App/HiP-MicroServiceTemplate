﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5000")
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();
    }
}
