using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;

namespace AKKA.Demo.Library
{
    public abstract class ReceiveActorBase : ReceiveActor
    {
        private readonly ILoggingAdapter logger = Logging.GetLogger(Context);
        protected readonly Guid _id;
        public abstract string Alias { get; }
        protected ReceiveActorBase()
        {
            _id = Guid.NewGuid();
            logger.Info("Actor Created:{0}", GetType());
        }

        protected override void PreRestart(Exception reason, object message)
        {
            base.PreRestart(reason, message);
            logger.Info("Actor PreRestart:{0}", GetType());
        }

        protected override void PreStart()
        {
            base.PreStart();
            ActorsSystem.Add(Alias, Self);
            logger.Info("Actor PreStart:{0}", GetType());
            ActorInitialize();
        }

        protected override void PostRestart(Exception reason)
        {
            base.PostRestart(reason);
            ActorsSystem.Add(Alias, Self);
            logger.Error(string.Format("Actor PostRestart:{0} - {1}",reason.ToString(), GetType()));
            //ActorInitialize();
        }

        protected override void Unhandled(object message)
        {
            base.Unhandled(message);
            logger.Debug("Actor:{0} Unhandled message of type:{1} - Content:{2}", Alias, GetType(), message?.ToString());
        }

        protected override void PostStop()
        {
            base.PostStop();
            ActorsSystem.Remove(Alias, Self);
            logger.Info("Actor PostStop::{0}", GetType());
        }

        protected abstract void ActorInitialize();
    }
}
