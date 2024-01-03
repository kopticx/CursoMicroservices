using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler(IOrderRepository repository, IMapper mapper) : IRequestHandler<UpdateOrderCommand>
{
  public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
  {
    var orderToUpdate = await repository.GetByIdAsync(request.Id);

    if (orderToUpdate is null)
    {
      throw new NotFoundException(nameof(Order), request.Id);
    }

    mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));

    await repository.UpdateAsync(orderToUpdate);
  }
}