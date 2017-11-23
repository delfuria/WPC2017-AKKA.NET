using Akka.Actor;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Configuration;
using Akka.Pattern;
using Akka.Routing;
using AKKA.Library.Demo;

namespace AKKA.AppConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Demo1Simple();
            //Demo2Props();
            //Demo3SimpleActorSystem();
            //Demo4FullSystem();
            //Demo5Supervision();
            //Demo6BackSupervision();
            Demo7Persistence();
            //Demo8Router();
            //Demo10Ask();
            Console.ReadLine();
        }

        private static async void Demo10Ask()
        {
            Console.WriteLine("Press enter to ask response");
            Console.ReadLine();
            var responseActor = ActorsSystem.Instance.ActorOf(ResponseActor.CreateProps(), "ResponseActor");
            try
            {
                //var res = await responseActor.Ask(new RequestMessage() { Number = -10 });
                var res = await responseActor.Ask(new RequestMessage() { Number = -10 }, TimeSpan.FromSeconds(2));
                var responseNumber = ((ResponseMessage)res).Number;
                Console.WriteLine($"Response1:{responseNumber}");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
        private static void Demo8Router()
        {
            var props = SimpleActor.CreateProps().WithRouter(new RoundRobinPool(2));
            var simple = ActorsSystem.Instance.ActorOf(props, "SimpleActor");

            Console.WriteLine("Press enter to start");
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
            simple.Tell(new ActorMessage(20));
            Console.ReadLine();
            simple.Tell(new ActorMessage(30));
            Console.ReadLine();
            simple.Tell(new ActorMessage(40));
            Console.ReadLine();
        }
        private static void Demo7Persistence()
        {
            var simple = ActorsSystem.Instance.ActorOf(SimplePersistentActor.CreateProps(), "SimplePersistentActor");
            Console.WriteLine("Press enter to start");
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
            simple.Tell(new RaiseExceptionMessage(new Exception()));
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
        }
        private static void Demo6BackSupervision()
        {
            var childProps = SimpleActor.CreateProps();

            var supervisor = BackoffSupervisor.Props(
                Backoff.OnFailure(
                        childProps: childProps,
                        childName: "SimpleActor",
                        minBackoff: TimeSpan.FromSeconds(5),
                        maxBackoff: TimeSpan.FromSeconds(60),
                        randomFactor: 0.2)
                    .WithAutoReset(TimeSpan.FromSeconds(160)));

            var simple = ActorsSystem.Instance.ActorOf(supervisor, "SimpleActor:Supervisor");

            Console.WriteLine("Press enter to start");
            Console.ReadLine();
            simple.Tell(new ExceptionMessage());
            Console.WriteLine("Exception Raised");
            Console.ReadLine();
            simple.Tell(new ExceptionMessage());
            Console.WriteLine("Exception Raised");
            Console.ReadLine();
            simple.Tell(new ExceptionMessage());
            Console.WriteLine("Exception Raised");
            Console.ReadLine();

        }
        private static void Demo5Supervision()
        {
            var simple = ActorsSystem.Instance.ActorOf(SimpleActor.CreateProps(), "SimpleActor");


            Console.WriteLine("Press enter to start");
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
            simple.Tell(new RaiseExceptionMessage(new FormatException()));
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
            simple.Tell(new RaiseExceptionMessage(new NullReferenceException()));
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
            simple.Tell(new RaiseExceptionMessage(new Exception()));
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
            simple.Tell(new RaiseExceptionMessage(new Exception()));
            Console.ReadLine();

        }
        private static void Demo4FullSystem()
        {
            var simple = ActorsSystem.Instance.ActorOf(SimpleActor.CreateProps(), "SimpleActor");
            Console.WriteLine("Press enter to start");
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
            simple.Tell(new ExceptionMessage());
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
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
            //IActorRef senderUntypedActor = SimpleActorSystem.Instance.ActorOf(Props.Create(() => new SenderUntypedActor(firstUntypedActor)), "SenderUntypedActor");
            IActorRef senderUntypedActor = SimpleActorSystem.Instance.ActorOf(Props.Create<SenderUntypedActor>(), "SenderUntypedActor");

            firstReceivedActor.Tell("My 1st Message");
            firstReceivedActor.Tell("1st");

            firstUntypedActor.Tell("My 1st Message");
            firstUntypedActor.Tell("1st");

            senderUntypedActor.Tell(new SimpleMessage() { Value = "To forward" });
            Console.ReadLine();


        }
        private static void Demo2Props()
        {
            ActorSystem system = ActorSystem.Create("MyActorSystem");

            IActorRef firstReceivedActor = system.ActorOf(Props.Create<FirstReceivedActor>("WPC2017"), "FirstReceivedActor");
            IActorRef firstUntypedActor = system.ActorOf(Props.Create(() => new FirstUntypedActor("WPC2017")), "FirstUntypedActor");

            firstReceivedActor.Tell("My 1st Message");
            firstReceivedActor.Tell("Ok");
            firstReceivedActor.Tell(new SimpleMessage() { Value = "Hello from WPC2017" });


            firstUntypedActor.Tell("My 1st Message");
            firstUntypedActor.Tell("Ok");
            firstUntypedActor.Tell(new SimpleMessage() { Value = "Hello from WPC2017" });

        }
        private static void Demo1Simple()
        {
            ActorSystem system = ActorSystem.Create("MyActorSystem");

            IActorRef firstReceivedActor = system.ActorOf<FirstReceivedActor>("FirstReceivedActor");
            IActorRef firstUntypedActor = system.ActorOf<FirstUntypedActor>("FirstUntypedActor");

            firstReceivedActor.Tell("My 1st Message");
            firstReceivedActor.Tell("Ok");
            firstReceivedActor.Tell(new SimpleMessage() { Value = "Hello from WPC2017" });

            firstUntypedActor.Tell("My 1st Message");
            firstUntypedActor.Tell("Ok");
            firstUntypedActor.Tell(new SimpleMessage() { Value = "Hello from WPC2017" });
        }
    }
}
