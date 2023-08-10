namespace EventBus.Messages.Events;

public class ProductNameChangedEvent : IntegrationBaseEvent
{
    public string ProductId { get; set; }
    public string UpdatedName { get; set; }
}
