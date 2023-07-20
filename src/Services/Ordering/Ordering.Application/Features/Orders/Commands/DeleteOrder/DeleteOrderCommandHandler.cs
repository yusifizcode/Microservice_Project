using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IMapper mapper,
                                     IOrderRepository orderRepository,
                                     ILogger<DeleteOrderCommandHandler> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _orderRepository = orderRepository;
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var existOrder = await _orderRepository.GetByIdAsync(request.Id);
        if (existOrder == null) throw new NotFoundException(nameof(Order), request.Id);

        await _orderRepository.DeleteAsync(existOrder);
        _logger.LogError($"Order {existOrder.Id} is successfully deleted.");

        return Unit.Value;
    }
}
