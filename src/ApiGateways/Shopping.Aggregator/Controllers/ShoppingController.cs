using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System.Net;

namespace Shopping.Aggregator.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IBasketService _basketService;
    private readonly ICatalogService _catalogService;

    public ShoppingController(IOrderService orderService,
                              IBasketService basketService,
                              ICatalogService catalogService)
    {
        _orderService = orderService;
        _basketService = basketService;
        _catalogService = catalogService;
    }

    [HttpGet("{userName}", Name = "GetShopping")]
    [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
    {
        var basket = await _basketService.GetBasket(userName);

        foreach (var item in basket.Items)
        {
            var product = await _catalogService.GetCatalog(item.ProductId);

            item.Summary = product.Summary;
            item.ProductName = product.Name;
            item.Category = product.Category;
            item.ImageFile = product.ImageFile;
            item.Description = product.Description;
        }

        var orders = await _orderService.GetOrdersByUserName(userName);

        var shoppingModel = new ShoppingModel
        {
            UserName = userName,
            BasketWithProducts = basket,
            Orders = orders
        };

        return Ok(shoppingModel);
    }
}
