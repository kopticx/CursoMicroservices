using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler(IOrderRepository repository) : IRequestHandler<DeleteOrderCommand>
{
  public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
  {
    var orderToDelete = await repository.GetByIdAsync(request.Id);
    if (orderToDelete == null)
    {
      throw new NotFoundException(nameof(Order), request.Id);
    }

    await repository.DeleteAsync(orderToDelete);
  }
}