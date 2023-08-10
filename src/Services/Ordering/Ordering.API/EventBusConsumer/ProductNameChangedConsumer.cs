using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;

namespace Ordering.API.EventBusConsumer;

public class ProductNameChangedConsumer : IConsumer<ProductNameChangedEvent>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<ProductNameChangedEvent> _logger;
    public ProductNameChangedConsumer(IMapper mapper,
                                      IMediator mediator,
                                      ILogger<ProductNameChangedEvent> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ProductNameChangedEvent> context)
    {
        var command = _mapper.Map<UpdateOrderCommand>(context.Message);
        var result = await _mediator.Send(command);

        _logger.LogInformation($"PaymentCheckoutEvent consumed successfully. Create Order Id: {result}");
    }
}
