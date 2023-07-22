using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext context, ILogger<OrderContextSeed> logger)
    {
        if (!context.Orders.Any())
        {
            context.Orders.AddRange(GetPreconfiguredOrders());
            await context.SaveChangesAsync();
            logger.LogInformation($"Seed database associated with context {typeof(OrderContext).Name}");
        }
    }

    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
        return new List<Order>
            {
                new Order() {UserName = "yusifiz", FirstName = "Yusif", LastName = "Osmanov", EmailAddress = "yusifizz01@gmail.com", AddressLine = "Sumgait", Country = "Azerbaijan", TotalPrice = 1350 }
            };
    }
}
