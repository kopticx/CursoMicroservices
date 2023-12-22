using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrdersListQuery(string userName) : IRequest<List<OrderDto>>
{
  public string UserName { get; set; } = userName;
}