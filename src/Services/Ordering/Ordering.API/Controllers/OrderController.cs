using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
}
