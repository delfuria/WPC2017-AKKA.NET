using Akka.Actor;
using System;
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
			//Demo7Router();
			//Demo8Persistence();
			Console.ReadLine();
		}


		private static void Demo8Persistence()
		{
			var simple = ActorsSystem.Instance.ActorOf(SimplePersistentActor.CreateProps(), "SimplePersistentActor");
			Console.WriteLine("Press enter to start");
			Console.ReadLine();
			simple.Tell(new ActorMessage(10));
			Console.ReadLine();
			simple.Tell(new ActorMessage(10));
			Console.ReadLine();
			simple.Tell(new RaiseExceptionMessage());
			Console.ReadLine();
			simple.Tell(new ActorMessage(10));
			Console.ReadLine();
		}
		private static void Demo7Router()
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
			simple.Tell(new BackOfficeRaiseExceptionMessage());
			Console.WriteLine("Exception Raised");
			Console.ReadLine();
			simple.Tell(new BackOfficeRaiseExceptionMessage());
			Console.WriteLine("Exception Raised");
			Console.ReadLine();
			simple.Tell(new BackOfficeRaiseExceptionMessage());
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
			simple.Tell(new RaiseExceptionMessage());
			Console.ReadLine();
			simple.Tell(new ActorMessage(20));
			Console.ReadLine();
			simple.Tell(new RaiseExceptionMessage());
			Console.ReadLine();
			simple.Tell(new ActorMessage(30));
			Console.ReadLine();
			simple.Tell(new RaiseExceptionMessage());
			Console.ReadLine();
			simple.Tell(new ActorMessage(40));
			Console.ReadLine();
			simple.Tell(new RaiseExceptionMessage());
			Console.ReadLine();

		}
		private static void Demo4FullSystem()
		{
			var simple = ActorsSystem.Instance.ActorOf(SimpleActor.CreateProps(), "SimpleActor");
			Console.WriteLine("Press enter to start");
			Console.ReadLine();
			simple.Tell(new ActorMessage(10));
			Console.ReadLine();
			simple.Tell(new ActorMessage(20));
			Console.ReadLine();
			simple.Tell(new RaiseExceptionMessage());
			Console.ReadLine();
			simple.Tell(new ActorMessage(30));
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

			Console.ReadLine();

			senderUntypedActor.Tell(new SimpleMessage() { Value = "To forward" });


		}
		private static void Demo2Props()
		{
			ActorSystem system = ActorSystem.Create("MySystem");

			IActorRef firstReceivedActor = system.ActorOf(Props.Create<FirstReceivedActor>(), "FirstReceivedActor");
			IActorRef firstUntypedActor = system.ActorOf(Props.Create(() => new FirstUntypedActor("World")), "FirstUntypedActor");

			firstReceivedActor.Tell("My 1st Message");
			firstReceivedActor.Tell("1st");

			firstUntypedActor.Tell("My 1st Message");
			firstUntypedActor.Tell("1st");
		}
		private static void Demo1Simple()
		{
			ActorSystem system = ActorSystem.Create("MySystem");

			IActorRef firstReceivedActor = system.ActorOf<FirstReceivedActor>("FirstReceivedActor");
			IActorRef firstUntypedActor = system.ActorOf<FirstReceivedActor>("FirstUntypedActor");

			firstReceivedActor.Tell("My 1st Message");
			firstReceivedActor.Tell("1st");
			firstReceivedActor.Tell(new SimpleMessage() { Value = "bye" });

			firstUntypedActor.Tell("My 1st Message");
			firstUntypedActor.Tell("1st");
			firstUntypedActor.Tell(new SimpleMessage() { Value = "Bye" });
		}
	}
}
