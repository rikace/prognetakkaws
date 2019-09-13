using System;

using Akka.Actor;

using AkkaFractal.Core;

namespace AkkaFractal.Remote
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO Lab 3 (c)
            // Create and Actor System to run this console app
            // as Remoting Actor deployment endpoint.
            //    - complete first the TODO lab 3 in the "AkkaFractal.Web" project
            //    - you should load and use the local "akka.conf" file to configure the Actor System
            //    - the name of the Actor System should match the name specified 
            //      in the remote configuration of the "akka.conf" file in the "AkkaFractal.Web" project.
            //      (if you choose the code approach to configure the remote deployment in the "AkkaFractal.Web"
            //       project, then you should use that name

            // TODO Lab 6 (a)
            // change the name of the Actor System to be consistent across the 
            // Actor System(s) in the application.
            // To cluster the Actor Systems together, all must have the same name
            // (check all the reference in the "akka.conf" files)
            var config = ConfigurationLoader.Load();
            using (var system = ActorSystem.Create("fractal", config))
            {
                Console.Title = $"Remote Worker - {system.Name}";
                Console.ForegroundColor = ConsoleColor.Green;
                
                Console.WriteLine("Press [ENTER] to exit.");
                Console.ReadLine();
            }
        }
    }
}