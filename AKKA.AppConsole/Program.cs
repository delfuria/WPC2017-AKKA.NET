using Akka.Actor;
using Akka.Configuration;
using Akka.Pattern;
using Akka.Routing;
using AKKA.Library.Demo;
using System;

namespace AKKA.AppConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // All logs goes to %appdata% (Local Folder)

            //Demo1Simple();
            //Demo2Props();
            //Demo3SimpleActorSystem();
            //Demo4FullSystem();
            //Demo5Supervision();
            //Demo6BackSupervision();
            //Demo7Persistence();
            //Demo7_5();
            //Demo8Router();
            //Demo10Ask();

            Console.ReadLine();
        }

        private static void Demo7_5()
        {
            var actorSystem = ActorSystem.Create("AtLeastOnceDeliveryDemo");

            var recipientActor = actorSystem.ActorOf(Props.Create(() => new RecipientActor()), "RecipientActor");
            var atLeastOnceDeliveryActor =
                actorSystem.ActorOf(AtLeastOnceDeliveryActor.CreateProps(recipientActor), "AtLeastOnceDeliveryActor");
            //actorSystem.ActorOf(Props.Create(() => new AtLeastOnceDeliveryActor(recipientActor)), "AtLeastOnceDeliveryActor");

            actorSystem.WhenTerminated.Wait();

        }

        private static async void Demo10Ask()
        {
            Console.WriteLine("Press enter to ask response");
            Console.ReadLine();
            var responseActor = ActorsSystem.Instance.ActorOf(ResponseActor.CreateProps(), "ResponseActor");
            try
            {
                var res = await responseActor.Ask(new RequestMessage() { Number = -10 });
                //var res = await responseActor.Ask(new RequestMessage() { Number = -10 }, TimeSpan.FromSeconds(2));
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
            Console.WriteLine("Creating SimpleActor with Router (RoundRobin policy)");
            var props = SimpleActor.CreateProps().WithRouter(new RoundRobinPool(2));
            var simple = ActorsSystem.Instance.ActorOf(props, "SimpleActor");

            Console.WriteLine("Press enter to start");
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
        }

        private static void Demo7Persistence()
        {
            Console.WriteLine("Creating SimplepersistentActor");
            var simple = ActorsSystem.Instance.ActorOf(SimplePersistentActor.CreateProps(), "SimplePersistentActor");
            Console.WriteLine("Press enter to start send messages");
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));

            Console.WriteLine("Press enter to send messages that cause Exception");
            Console.ReadLine();
            simple.Tell(new RaiseExceptionMessage(new Exception()));
            Console.WriteLine("Exception raised, state retrived from snapshot");
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
        }

        private static void Demo6BackSupervision()
        {
            Console.WriteLine("Creating SimpleActor Props with BackOff Supervisor");
            var childProps = SimpleActor.CreateProps();

            var supervisor = BackoffSupervisor.Props(
                Backoff.OnFailure(
                        childProps: childProps,
                        childName: "SimpleActor",
                        minBackoff: TimeSpan.FromSeconds(3),
                        maxBackoff: TimeSpan.FromSeconds(60),
                        randomFactor: 0.2)
                    .WithAutoReset(TimeSpan.FromSeconds(160)));

            var simple = ActorsSystem.Instance.ActorOf(supervisor, "SimpleActor:Supervisor");

            Console.WriteLine("Press enter to start raise exception");
            Console.ReadLine();
            simple.Tell(new ExceptionMessage());
            Console.WriteLine("Exception Raised");
            Console.ReadLine();
            simple.Tell(new ExceptionMessage());
            Console.WriteLine("Exception Raised");
            Console.ReadLine();
            simple.Tell(new ExceptionMessage());
            Console.WriteLine("Exception Raised");
        }

        private static void Demo5Supervision()
        {
            Console.WriteLine("Get Actor System from singleton Instance");
            Console.WriteLine("and creating SimpleActor");
            var simple = ActorsSystem.Instance.ActorOf(SimpleActor.CreateProps(), "SimpleActor");

            Console.WriteLine("Press enter to start sending message");
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.WriteLine("Press enter to send message that cause resume (actor will not restart)");
            Console.ReadLine();

            simple.Tell(new RaiseExceptionMessage(new FormatException()));
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));

            Console.WriteLine("Press enter to send message that cause restart (actor will restart)");

            Console.ReadLine();
            simple.Tell(new RaiseExceptionMessage(new NullReferenceException()));
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));

            Console.WriteLine("Press enter to send message that cause Stop (actor will stop)");
            Console.ReadLine();
            simple.Tell(new RaiseExceptionMessage(new Exception()));
            //Console.ReadLine();
            //simple.Tell(new ActorMessage(10));
        }

        private static void Demo4FullSystem()
        {
            Console.WriteLine("Get Actor System from singleton Instance");
            var simple = ActorsSystem.Instance.ActorOf(SimpleActor.CreateProps(), "SimpleActor");
            Console.WriteLine("Press enter to start sending messages");
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));

            Console.WriteLine("Now will send a message that raise and exception");
            Console.ReadLine();
            simple.Tell(new ExceptionMessage());
            Console.WriteLine("Exception raised, the actor will restart");
            Console.ReadLine();
            simple.Tell(new ActorMessage(10));
        }

        private static void Demo3SimpleActorSystem()
        {
            Console.WriteLine("Get Actor System from singleton Instance");
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

            Console.WriteLine("Creating Actors");
            IActorRef firstReceivedActor = SimpleActorSystem.Instance.ActorOf(Props.Create<FirstReceivedActor>(), "FirstReceivedActor");
            IActorRef firstUntypedActor = SimpleActorSystem.Instance.ActorOf(Props.Create(() => new FirstUntypedActor("World")), "FirstUntypedActor");
            IActorRef senderUntypedActor = SimpleActorSystem.Instance.ActorOf(Props.Create<SenderUntypedActor>(), "SenderUntypedActor");

            Console.WriteLine("Send messages to Actos");
            firstReceivedActor.Tell("My 1st Message");
            firstReceivedActor.Tell("1st");

            firstUntypedActor.Tell("My 1st Message");
            firstUntypedActor.Tell("1st");

            senderUntypedActor.Tell(new SimpleMessage() { Value = "To forward" });
        }

        private static void Demo2Props()
        {
            Console.WriteLine("Creating Actor System");
            ActorSystem system = ActorSystem.Create("MyActorSystem");

            Console.WriteLine("Creating Actors using Props");
            IActorRef firstReceivedActor = system.ActorOf(Props.Create<FirstReceivedActor>("WPC2017"), "FirstReceivedActor");
            IActorRef firstUntypedActor = system.ActorOf(Props.Create(() => new FirstUntypedActor("WPC2017")), "FirstUntypedActor");

            Console.WriteLine("Send messages to actors");
            firstReceivedActor.Tell("My 1st Message");
            firstReceivedActor.Tell("Ok");
            firstReceivedActor.Tell(new SimpleMessage() { Value = "Hello from WPC2017" });

            firstUntypedActor.Tell("My 1st Message");
            firstUntypedActor.Tell("Ok");
            firstUntypedActor.Tell(new SimpleMessage() { Value = "Hello from WPC2017" });
        }

        private static void Demo1Simple()
        {
            Console.WriteLine("Creating Actor System");
            ActorSystem system = ActorSystem.Create("MyActorSystem");

            Console.WriteLine("Creating Actors ");
            IActorRef firstReceivedActor = system.ActorOf<FirstReceivedActor>("FirstReceivedActor");
            IActorRef firstUntypedActor = system.ActorOf<FirstUntypedActor>("FirstUntypedActor");

            Console.WriteLine("Send messages to actors");
            firstReceivedActor.Tell("My 1st Message");
            firstReceivedActor.Tell("Ok");
            firstReceivedActor.Tell(new SimpleMessage() { Value = "Hello from WPC2017" });

            firstUntypedActor.Tell("My 1st Message");
            firstUntypedActor.Tell("Ok");
            firstUntypedActor.Tell(new SimpleMessage() { Value = "Hello from WPC2017" });
        }
    }
}