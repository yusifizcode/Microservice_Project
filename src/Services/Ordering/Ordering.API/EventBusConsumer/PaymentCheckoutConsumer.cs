using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumer;

public class PaymentCheckoutConsumer : IConsumer<PaymentCheckoutEvent>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<PaymentCheckoutConsumer> _logger;
    public PaymentCheckoutConsumer(IMapper mapper,
                                  IMediator mediator,
                                  ILogger<PaymentCheckoutConsumer> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<PaymentCheckoutEvent> context)
    {
        var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
        var result = await _mediator.Send(command);

        _logger.LogInformation($"PaymentCheckoutEvent consumed successfully. Create Order Id: {result}");
    }
}
