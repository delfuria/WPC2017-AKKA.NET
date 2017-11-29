using Akka.Actor;
using Akka.Event;
using System;

namespace AKKA.Library.Demo
{
    public abstract class UntypedActorBase : UntypedActor
    {
        protected readonly ILoggingAdapter logger = Logging.GetLogger(Context);
        public abstract string Alias { get; }

        protected UntypedActorBase()
        {
            logger.Info($"Actor Created:{GetType()} - UID:{Context.Self.Path.Uid}");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            base.PreRestart(reason, message);
            logger.Info($"Actor PreRestart:{GetType()}");
        }

        protected override void PreStart()
        {
            base.PreStart();
            ActorsSystem.Add(Alias, Self);
            logger.Info($"Actor PreStart:{GetType()}");
            ActorInitialize();
        }

        protected override void PostRestart(Exception reason)
        {
            base.PostRestart(reason);
            ActorsSystem.Add(Alias, Self);
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
            ActorsSystem.Remove(Alias, Self);
            logger.Info($"Actor PostStop::{GetType()}");
        }

        protected override void OnReceive(object message)
        {
            logger.Info($"Actor:{Self.Path} received Message:{message?.ToString()}");
        }

        protected abstract void ActorInitialize();
    }
}