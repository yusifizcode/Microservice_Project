using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountGrpcService _discountGrpcService;

    public BasketController(IBasketRepository basketRepository,
                            DiscountGrpcService discountGrpcService)
    {
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
    }


    [HttpGet("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
    {
        var basket = await _basketRepository.GetBasketAsync(userName);
        return Ok(basket ?? new ShoppingCart(userName));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart cart)
    {
        foreach (var item in cart.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }

        return Ok(await _basketRepository.UpdateBasketAsync(cart));
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _basketRepository.DeleteBasketAsync(userName);
        return Ok();
    }

    //[HttpPost]
    //[Route("[action]")]
    //[ProducesResponseType((int)HttpStatusCode.Accepted)]
    //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
    //public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    //{
    //    var basket = await _basketRepository.GetBasketAsync(basketCheckout.UserName);
    //    if (basket == null)
    //        return BadRequest();

    //    var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
    //    eventMessage.TotalPrice = basketCheckout.TotalPrice;

    //    await _publishEndpoint.Publish(eventMessage);
    //    await _basketRepository.DeleteBasketAsync(basket.UserName);

    //    return Accepted();
    //}
}
