using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;

    public UpdateOrderCommandHandler(IMapper mapper,
                                     IOrderRepository orderRepository,
                                     ILogger<UpdateOrderCommandHandler> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _orderRepository = orderRepository;
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var existOrder = await _orderRepository.GetByIdAsync(request.Id);

        if (existOrder == null)
            _logger.LogError("Order not exist on database.");

        _mapper.Map(request, existOrder, typeof(UpdateOrderCommand), typeof(Order));

        await _orderRepository.UpdateAsync(existOrder);

        _logger.LogError($"Order {existOrder.Id} is successfully  updated.");

        return Unit.Value;
    }
}
