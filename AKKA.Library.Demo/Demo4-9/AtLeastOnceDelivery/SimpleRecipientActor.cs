using System;
using Akka.Actor;

namespace AKKA.Library.Demo
{
    public class SimpleRecipientActor : ReceiveActor
    {
        public SimpleRecipientActor()
        {
            Receive<EnvelopeMessage<WriteMessage>>(write =>
            {
                Console.WriteLine($"Received message {write.Message.Content} [id: {write.MessageId}] " +
                                  $"\nat {DateTime.Now}"+
                                  $"\nfrom {Sender}" +
                                  $"\n - accept?");
                var response = Console.ReadLine()?.ToLowerInvariant();
                if (!string.IsNullOrEmpty(response) && (response.Equals("yes") || response.Equals("y")))
                {
                    // confirm delivery only if the user explicitly agrees
                    Sender.Tell(new AckMessage(write.MessageId));
                    Console.WriteLine("Confirmed delivery of message ID {0}", write.MessageId);
                }
                else
                {
                    Console.WriteLine("Did not confirm delivery of message ID {0}", write.MessageId);
                }
            });
        }
    }
}