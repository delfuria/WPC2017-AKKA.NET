using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Configuration;
using AKKA.Demo.Library;

namespace AKKA.AppConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Demo1Simple();
            //Demo2Props();
            Demo3SimpleActorSystem();
            Console.ReadLine();
        }

        private static void Demo3SimpleActorSystem()
        {
            SimpleActorSystem.Name = "Demo-Akka";
            SimpleActorSystem.Config = ConfigurationFactory.ParseString(@"
                    akka {
                        serializers {
                          hyperion = ""Akka.Serialization.HyperionSerializer, Akka.Serialization.Hyperion""
                        }
                            serialization-bindings {
                          ""System.Object"" = hyperion
                        }
                    }
                    }");

            IActorRef firstReceivedActor = SimpleActorSystem.Instance.ActorOf(Props.Create<FirstReceivedActor>(), "FirstReceivedActor");
            IActorRef firstUntypedActor = SimpleActorSystem.Instance.ActorOf(Props.Create(() => new FirstUntypedActor("World")), "FirstUntypedActor");
            IActorRef senderUntypedActor = SimpleActorSystem.Instance.ActorOf(Props.Create(() => new SenderUntypedActor(firstUntypedActor)), "SenderUntypedActor");

            firstReceivedActor.Tell("My 1st Message");
            firstReceivedActor.Tell("1st");

            firstUntypedActor.Tell("My 1st Message");
            firstUntypedActor.Tell("1st");

            Console.ReadLine();

            senderUntypedActor.Tell(new SimpleMessage("To forward"));


        }

        static void Demo1Simple()
        {
            ActorSystem system = ActorSystem.Create("MySystem");

            IActorRef firstReceivedActor = system.ActorOf<FirstReceivedActor>("FirstReceivedActor");
            IActorRef firstUntypedActor = system.ActorOf<FirstReceivedActor>("FirstUntypedActor");

            firstReceivedActor.Tell("My 1st Message");
            firstReceivedActor.Tell("1st");
            firstReceivedActor.Tell(new SimpleMessage("Bye"));

            firstUntypedActor.Tell("My 1st Message");
            firstUntypedActor.Tell("1st");
            firstUntypedActor.Tell(new SimpleMessage("Bye"));
        }
        static void Demo2Props()
        {
            ActorSystem system = ActorSystem.Create("MySystem");

            IActorRef firstReceivedActor = system.ActorOf(Props.Create<FirstReceivedActor>(), "FirstReceivedActor");
            IActorRef firstUntypedActor = system.ActorOf(Props.Create(() => new FirstUntypedActor("World")), "FirstUntypedActor");

            firstReceivedActor.Tell("My 1st Message");
            firstReceivedActor.Tell("1st");

            firstUntypedActor.Tell("My 1st Message");
            firstUntypedActor.Tell("1st");
        }
    }
}
