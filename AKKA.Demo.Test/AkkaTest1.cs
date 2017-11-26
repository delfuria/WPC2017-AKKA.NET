using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.VsTest;
using AKKA.Library.Demo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AKKA.Demo.Test
{
    [TestClass]
    public class AkkaTest1: TestKit
    {
        [TestMethod]
        public void TestInternalState()
        {

            var actor = ActorOfAsTestActorRef<SimpleActor>(SimpleActor.CreateProps());
            var underlyingActor = actor.UnderlyingActor;
            actor.Tell(new ChangeStateMessage(10));
            Assert.AreEqual(10, underlyingActor.State);
        }

        [TestMethod]
        public void TestParentCreateChild()
        {
            // verify child has been created by sending parent a message
            // that is forwarded to child, and which child replies to sender with
            //var parentProps = Props.Create(() => new SimpleActor());
            var first = ActorOfAsTestActorRef<FirstUntypedActor>(Props.Create<FirstUntypedActor>(), TestActor, "FirstUntypedActor");
            var sender = ActorOfAsTestActorRef<SenderUntypedActor>(Props.Create<SenderUntypedActor>(), TestActor);
            sender.Tell(new ForwardMessage());
            ExpectMsg("Ok");
        }
    }
}
