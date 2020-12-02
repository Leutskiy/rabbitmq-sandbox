using System;

namespace Demo.ClientNotifier.Demo.Controllers
{
    public sealed class Notifaction
    {
        public Guid Id { get; }

        public string MessageText { get; }

        public Notifaction(string messageText)
        {
            Id = Guid.NewGuid();
            MessageText = messageText;
        }
    }
}
