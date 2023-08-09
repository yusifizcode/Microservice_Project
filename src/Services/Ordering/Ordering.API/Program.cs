using EventBus.Messages.Common;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var requireAuthorize = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter(requireAuthorize));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

#region MassTransit-RabbitMq Configuration

builder.Services.AddMassTransit(config =>
{
    //config.AddConsumer<BasketCheckoutConsumer>();
    config.AddConsumer<PaymentCheckoutConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

        //cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        //{
        //    c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        //});

        cfg.ReceiveEndpoint(EventBusConstants.PaymentCheckoutQueue, c =>
        {
            c.ConfigureConsumer<PaymentCheckoutConsumer>(ctx);
        });
    });
});

//builder.Services.AddMassTransitHostedService();
builder.Services.AddOptions<MassTransitHostedService>();

#endregion

#region General Configuration

builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddScoped<BasketCheckoutConsumer>();
builder.Services.AddScoped<PaymentCheckoutConsumer>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opitons =>
{
    opitons.Authority = builder.Configuration["IdentityServerUrl"];
    opitons.Audience = "resource_ordering";
    opitons.RequireHttpsMetadata = false;
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase<OrderContext>((context, services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger).Wait();
});

app.Run();
