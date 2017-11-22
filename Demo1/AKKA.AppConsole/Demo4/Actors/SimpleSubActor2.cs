using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace AKKA.Demo.Library
{
    public class SimpleSubActor2 : UntypedActorBase
    {
        public override string Alias => "SimpleSubActor2";

        public SimpleSubActor2(Guid id)
        {
            _id = id;
        }

        public static Props CreateProps(Guid id = new Guid())
        {
            //if (id == Guid.Empty)
            //    id = Guid.NewGuid();
            return Props.Create(() => new SimpleSubActor2(id));
        }

        protected override void ActorInitialize()
        {
        }
    }
}
