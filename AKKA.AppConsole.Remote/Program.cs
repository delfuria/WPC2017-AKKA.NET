using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using AKKA.Library.Demo;

namespace AKKA.AppConsole.Remote
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(@"
                akka {
                    actor.provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""

                    remote {
                        dot-netty.tcp {
                            port = 9099
                            hostname = 0.0.0.0
                            public-hostname = 127.0.0.1
                        }
                    }
                }
            ");

            Console.WriteLine("Waiting for Messages");
            using (ActorSystem system = ActorSystem.Create("MyRemoteServer", config))
            {
                system.ActorOf<SimpleActor>("SimpleActor");
                Console.ReadLine();
            }

        }
    }
}
