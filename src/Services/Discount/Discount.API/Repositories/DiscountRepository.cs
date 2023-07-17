using Dapper;
using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
        => _configuration = configuration;

    public async Task<bool> CreateDiscountAsync(Coupon coupon)
    {
        using var connection = new NpgsqlConnection(_configuration
                                                    .GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected =
            await connection.ExecuteAsync("INSERT INTO Coupon (ProductName, Description, Amount)" +
                                          $"VALUES ('{coupon.ProductName}', '{coupon.Description}', {coupon.Amount})");

        if (affected == 0)
            return false;

        return true;
    }

    public async Task<bool> DeleteDiscountAsync(string productName)
    {
        using var connection = new NpgsqlConnection(_configuration
                                                    .GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected =
            await connection.ExecuteAsync($"DELETE FROM Coupon WHERE ProductName = '{productName}'");

        if (affected == 0)
            return false;

        return true;
    }

    public async Task<Coupon> GetDiscountAsync(string productName)
    {
        using var connection = new NpgsqlConnection(_configuration
                                                    .GetValue<string>("DatabaseSettings:ConnectionString"));

        var coupon = await connection
                           .QueryFirstOrDefaultAsync<Coupon>($"SELECT * FROM Coupon " +
                                                             $"WHERE ProductName = '{productName}'");

        if (coupon == null)
            return new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Description"
            };

        return coupon;
    }

    public async Task<bool> UpdateDiscountAsync(Coupon coupon)
    {
        using var connection = new NpgsqlConnection(_configuration
                                                    .GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected =
            await connection.ExecuteAsync($"UPDATE Coupon SET " +
                                          $"ProductName='{coupon.ProductName}', " +
                                          $"Description='{coupon.Description}', " +
                                          $"Amount={coupon.Amount} " +
                                          $"WHERE Id={coupon.Id}");

        if (affected == 0)
            return false;

        return true;
    }
}
