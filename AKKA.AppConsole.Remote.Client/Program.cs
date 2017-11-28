using Akka.Actor;
using Akka.Configuration;
using AKKA.Library.Demo;
using System;

namespace AKKA.AppConsole.Remote.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(@"
				akka {
					actor.provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""

					remote {
						dot-netty.tcp {
							port = 0
							hostname = localhost
		}
					}
				}
			");

            Console.WriteLine("press enter to select remote server");
            Console.ReadLine();
            using (var system = ActorSystem.Create("MyRemoteClient", config))
            {
                //get a reference to the remote actor
                var greeter = system
                    .ActorSelection("akka.tcp://MyRemoteServer@127.0.0.1:9099/user/SimpleActor");
                //send a message to the remote actor
                int i = 0;
                while (true)
                {
                    Console.WriteLine("press enter to send simplemessage");
                    Console.ReadLine();
                    greeter.Tell(new SimpleMessage() { Value = i++.ToString() });
                }
            }
        }
    }
}