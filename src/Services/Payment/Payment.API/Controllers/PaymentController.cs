using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Payment.API.Entities;
using Stripe;

namespace Payment.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    //private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public PaymentController(IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException();
        _mapper = mapper ?? throw new ArgumentNullException();
    }

    [HttpPost]
    public async Task<IActionResult> RecievePayment(PaymentCheckout paymentCheckout)
    {
        //var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));
        try
        {
            StripeConfiguration.ApiKey = "sk_test_51MsRiLGVwWWuLvrTdJpuEObgwZOdlfB6mCymd4Nro1JqeJMbSNphwHY85eHbaq54jGKS2i8fVBWJiJwjRoOQie6B00sTkyhmz1";

            var optionsToken = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Name = paymentCheckout.CardName,
                    Number = paymentCheckout.CardNumber,
                    ExpYear = paymentCheckout.Expiration.Trim().ToLower().Split("/")[1],
                    ExpMonth = paymentCheckout.Expiration.Trim().ToLower().Split("/")[0],
                    Cvc = paymentCheckout.CVV
                }
            };

            var serviceToken = new TokenService();
            Token stripeToken = await serviceToken.CreateAsync(optionsToken);

            var options = new ChargeCreateOptions
            {
                Amount = (long)paymentCheckout.TotalPrice * 100,
                Currency = "usd",
                Description = "test",
                Source = stripeToken.Id
            };

            var service = new ChargeService();
            Charge charge = await service.CreateAsync(options);

            if (charge.Paid)
            {
                var eventMessage = _mapper.Map<PaymentCheckoutEvent>(paymentCheckout);
                await _publishEndpoint.Publish(eventMessage);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        catch (System.Exception e)
        {
            return BadRequest(e.Message);
        }

        //return CreateActionResultInstance(Shared.DTOs.Response<NoContent>.Success(200));
    }
}
