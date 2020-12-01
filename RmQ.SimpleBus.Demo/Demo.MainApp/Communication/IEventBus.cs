namespace Demo.MainApp.Communication
{
	public interface IEventBus
	{
		public void Publish(IIntegrationEvent @integrationEvent);
	}
}
