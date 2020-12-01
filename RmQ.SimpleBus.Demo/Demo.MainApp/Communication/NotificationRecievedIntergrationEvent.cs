using System;

namespace Demo.MainApp.Communication
{
	public sealed class NotificationRecievedIntergrationEvent : IIntegrationEvent
	{
		public Guid Id { get; }

		public string MessageText { get; }

		public DateTimeOffset CreatedDate { get; }

		public NotificationRecievedIntergrationEvent(string messageText)
		{
			Id = Guid.NewGuid();
			MessageText = messageText;
			CreatedDate = DateTimeOffset.Now;
		}
	}
}
