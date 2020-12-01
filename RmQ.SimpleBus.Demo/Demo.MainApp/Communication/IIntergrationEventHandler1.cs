namespace Demo.MainApp.Communication
{
	public interface IIntergrationEventHandler<in TEvent> : IIntergrationEventHandler
		where TEvent : IIntegrationEvent
	{
		public void Handle(TEvent @event);
	}
}
