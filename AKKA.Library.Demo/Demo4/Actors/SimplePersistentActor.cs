using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Persistence;

namespace AKKA.Library.Demo
{
    public class SimplePersistentActor : UntypedPersistenActorBase
    {
        private int _value;
        public override string Alias => "SimplePersistentActor";

        public SimplePersistentActor()
        {
        }

        public static Props CreateProps()
        {
            return Props.Create(() => new SimplePersistentActor());
        }

        //protected override bool Receive(object message)
        //{
        //    base.Receive(message);
        //    switch (message)
        //    {
        //        case ActorMessage msg:
        //            HandleActorMessage(msg);
        //            break;

        //    }
        //    return true;
        //}

        protected override void OnRecover(object message)
        {
            logger.Info($"Actor OnRecovery:{GetType()} - Message:{message}");

            // handle recovery here
            try
            {
                SnapshotOffer snapshot = message as SnapshotOffer;
                _value = Convert.ToInt32(snapshot.Snapshot);
                logger.Info($"Value recovered:{_value}");
            }
            catch (Exception)
            {
            }
        }

        protected override void OnCommand(object message)
        {
            logger.Info($"Actor:{Self.Path} received Message:{message?.ToString()}");
            switch (message)
            {
                case SaveSnapshotSuccess s:
                    break;
                case SaveSnapshotFailure f:
                    break;
                case RaiseExceptionMessage msg:
                    HandleRaiseExceptionMessage();
                    break;
                case ActorMessage msg:
                    HandleActorMessage(msg);
                    Persist(_value, e =>
                            {
                                UpdateState(e);
                                if (LastSequenceNr % SnapShotInterval == 0 && LastSequenceNr != 0)
                                {
                                    SaveSnapshot(state);
                                }
                            });

                    break;

            }

            //if (message is SaveSnapshotSuccess s)
            //{
            //    // ...
            //}
            //else if (message is SaveSnapshotFailure f)
            //{
            //    // ...
            //}
            //else if (message is string cmd)
            //{
            //    Persist($"evt-{cmd}", e =>
            //    {
            //        UpdateState(e);
            //        if (LastSequenceNr % SnapShotInterval == 0 && LastSequenceNr != 0)
            //        {
            //            SaveSnapshot(state);
            //        }
            //    });
            //}
        }

        private void HandleRaiseExceptionMessage()
        {
            throw new Exception();
        }

        private void UpdateState(int e)
        {
            state = e;
        }

        //protected override bool Receive(object message)
        //{
        //    logger.Info($"Actor:{Self.Path} received Message:{message?.ToString()}");
        //    return true;
        //}

        private void HandleActorMessage(ActorMessage msg)
        {
            Console.WriteLine($"The previous state was {_value}");
            _value += msg.Value;
            Console.WriteLine($"The current state is now {_value}");
            Console.WriteLine($"path {Self.Path}");
            Console.WriteLine($"randomID {_id}");
            Console.WriteLine($"sender {Sender.Path}\n");
            //SaveSnapshot(_value);
        }

        protected override void ActorInitialize()
        {
        }
    }
}
