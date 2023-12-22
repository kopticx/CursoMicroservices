using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrdersListQueryHandler(IOrderRepository repository, IMapper mapper) : IRequestHandler<GetOrdersListQuery, List<OrderDto>>
{
  public async Task<List<OrderDto>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
  {
    var orderList = await repository.GetOrdersByUserName(request.UserName);

    return mapper.Map<List<OrderDto>>(orderList);
  }
}