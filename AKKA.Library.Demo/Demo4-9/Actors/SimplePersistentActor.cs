using Akka.Actor;
using Akka.Persistence;
using System;

namespace AKKA.Library.Demo
{
    public class SimplePersistentActor : UntypedPersistenActorBase
    {
        private int _value;
        public override string Alias => "SimplePersistentActor";

        public SimplePersistentActor()
        {
            //DeleteSnapshots(SnapshotSelectionCriteria.Latest);
        }

        public static Props CreateProps()
        {
            return Props.Create(() => new SimplePersistentActor());
        }

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
                    HandleSaveSnapshotSuccess(s);
                    break;

                case SaveSnapshotFailure f:
                    HandleSaveSnapshotFailure(f);
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
        }

        private void HandleSaveSnapshotSuccess(SaveSnapshotSuccess s)
        {
            logger.Info($"SnapShot Success:{s}");
        }

        private void HandleSaveSnapshotFailure(SaveSnapshotFailure f)
        {
            logger.Info($"SnapShot failed:{f}");
        }

        private void HandleRaiseExceptionMessage()
        {
            throw new Exception();
        }

        private void UpdateState(int e)
        {
            state = e;
        }

        private void HandleActorMessage(ActorMessage msg)
        {
            Console.WriteLine($"The previous state was {_value}");
            _value += msg.Value;
            Console.WriteLine($"The current state is now {_value}");
            Console.WriteLine($"path {Self.Path}");
            Console.WriteLine($"randomUID {Self.Path.Uid}");
            Console.WriteLine($"sender {Sender.Path}\n");
            //SaveSnapshot(_value);
        }

        protected override void ActorInitialize()
        {
        }
    }
}