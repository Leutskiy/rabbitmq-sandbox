using System;

namespace Demo.MainApp.PriorityQueue
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
