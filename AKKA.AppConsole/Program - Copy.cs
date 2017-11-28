//using Akka.Actor;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AKKA.AppConsole
//{
//    class Program
//    {
//        static void  Main(string[] args)
//        {
//            var simple = ActorsSystem.Instance.ActorOf(SimpleActor.CreateProps(Guid.NewGuid()), "SimpleActor");
//            var response = ActorsSystem.Instance.ActorOf(ResponseActor.CreateProps(Guid.NewGuid()), "ResponseActor");
//            //var rec = ActorsSystem.Instance.ActorOf(RecActor.CreateProps(Guid.NewGuid()), "RecActor");
//            Console.WriteLine("Premere per avviare");
//            Console.ReadLine();
//            Test(response);
//            Console.WriteLine("Dopo TEst");
//            while (true)
//            {
//                simple.Tell(new DummyMessage());
//                Console.ReadLine();

//            }
//            Console.ReadLine();
//        }

//        static async void Test(IActorRef response)
//        {
//            var res = await ((ICanTell)response).Ask(new RequestMessage(){Number = -10});//,TimeSpan.FromSeconds(2));
//            Console.WriteLine(((ResponseMessage)res).Number);

//            //var result = ActorModel.UsersManagerActorRef.Ask(new GetUserContentQueryMessage(userName), enquirer.AcceptableTimeout).Result as GetUserContentQueryResultMessage;

//        }
//    }
//}