using Akka.Actor;

namespace AKKA.Library.Demo
{
    public class SimpleSubActor2 : UntypedActorBase
    {
        public override string Alias => "SimpleSubActor2";

        public SimpleSubActor2()
        {
        }

        public static Props CreateProps()
        {
            return Props.Create(() => new SimpleSubActor2());
        }

        protected override void ActorInitialize()
        {
        }
    }
}