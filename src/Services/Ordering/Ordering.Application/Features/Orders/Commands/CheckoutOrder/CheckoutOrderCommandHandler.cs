using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandler(
  IOrderRepository repository,
  IMapper mapper,
  IEmailService emailService,
  ILogger<CheckoutOrderCommandHandler> logger)
  : IRequestHandler<CheckoutOrderCommand, int>
{
  public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
  {
    var orderEntity = mapper.Map<Order>(request);

    var newOrder = await repository.AddAsync(orderEntity);

    logger.LogInformation($"Order {newOrder.Id} is successfully created.");
    await SendMail(newOrder);

    return newOrder.Id;
  }

  private async Task SendMail(Order order)
  {
    var email = new Email()
    {
      To = "fdzk100@gmail.com",
      Body = $"The order was created.",
      Subject = "Order created."
    };

    try
    {
      await emailService.SendEmail(email);
    }
    catch (Exception ex)
    {
      logger.LogError($"Mailing about order {order.Id} failed due to an error with the mail service: {ex.Message}");
    }
  }
}