using Dapper;
using Discount.Grpc.Entities;
using Npgsql;

namespace Discount.Grpc.Repositories;

public class DiscountRepository(IConfiguration configuration) : IDiscountRepository
{
  public async Task<Coupon> GetDiscount(string productName)
  {
    await using var connection = new NpgsqlConnection(configuration.GetConnectionString("PostgresConnection"));

    var coupon =
      await connection.QueryFirstOrDefaultAsync<Coupon>
        ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

    return coupon;
  }

  public async Task<bool> CreateDiscount(Coupon coupon)
  {
    await using var connection = new NpgsqlConnection(configuration.GetConnectionString("PostgresConnection"));

    var affected =
      await connection.ExecuteAsync
      ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
        coupon);

    return affected != 0;
  }

  public async Task<bool> UpdateDiscount(Coupon coupon)
  {
    await using var connection = new NpgsqlConnection(configuration.GetConnectionString("PostgresConnection"));

    var affected =
      await connection.ExecuteAsync
      ("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
        coupon);

    return affected != 0;
  }

  public async Task<bool> DeleteDiscount(string productName)
  {
    await using var connection = new NpgsqlConnection(configuration.GetConnectionString("PostgresConnection"));

    var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

    return affected != 0;
  }
}