using AutoMapper;
using EventBus.Messages.Events;
using Payment.API.Entities;

namespace Payment.API.Mapper;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<PaymentCheckout, PaymentCheckoutEvent>().ReverseMap();
    }
}
