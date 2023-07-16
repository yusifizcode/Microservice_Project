using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _cache;
    public BasketRepository(IDistributedCache cache)
        => _cache = cache ?? throw new ArgumentNullException(nameof(cache));

    public async Task DeleteBasketAsync(string userName)
        => await _cache.RemoveAsync(userName);

    public async Task<ShoppingCart> GetBasketAsync(string userName)
    {
        var basket = await _cache.GetStringAsync(userName);
        if (String.IsNullOrEmpty(basket)) return null;

        return JsonConvert.DeserializeObject<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
    {
        await _cache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
        return await GetBasketAsync(basket.UserName);
    }
}
