﻿using Basket.API.Entities;

namespace Basket.API.Repositories;

public interface IBasketRepository
{
    Task DeleteBasketAsync(string userName);
    Task<ShoppingCart> GetBasketAsync(string userName);
    Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket);
}
