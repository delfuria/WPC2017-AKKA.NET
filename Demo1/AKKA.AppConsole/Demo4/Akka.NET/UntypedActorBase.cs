using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;

namespace AKKA.Demo.Library
{
    public abstract class UntypedActorBase : UntypedActor
    {

        protected readonly ILoggingAdapter logger = Logging.GetLogger(Context);
        protected Guid _id = Guid.Empty;
        public abstract string Alias { get; }
        protected UntypedActorBase()
        {
            logger.Info($"Actor Created:{GetType()}");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            base.PreRestart(reason, message);
            logger.Info($"Actor PreRestart:{GetType()}");
        }

        protected override void PreStart()
        {
            base.PreStart();
            ActorsSystem.Add(new ActorReference(Alias, _id), Self);
            logger.Info($"Actor PreStart:{GetType()}");
            ActorInitialize();
        }

        protected override void PostRestart(Exception reason)
        {
            base.PostRestart(reason);
            ActorsSystem.Add(new ActorReference(Alias, _id), Self);
            logger.Error($"Actor PostRestart:{reason} - {GetType()}");
            //ActorInitialize();
        }

        protected override void Unhandled(object message)
        {
            base.Unhandled(message);
            logger.Debug($"Actor:{Alias} Unhandled message of type:{GetType()} - Content:{message?.ToString()}");
        }

        protected override void PostStop()
        {
            base.PostStop();
            ActorsSystem.Remove(new ActorReference(Alias, _id), Self);
            logger.Info($"Actor PostStop::{GetType()}");
        }

        protected override void OnReceive(object message)
        {
            logger.Info($"Actor:{Self.Path} received Message:{message?.ToString()}");
        }

        protected abstract void ActorInitialize();
    }
}
