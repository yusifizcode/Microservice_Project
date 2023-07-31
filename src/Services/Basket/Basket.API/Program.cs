using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Redis Configuraion

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

#endregion

#region General Configration

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
//builder.Services.AddAutoMapper(typeof(Program));

#endregion

#region Grpc Configuration

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]);
});
builder.Services.AddScoped<DiscountGrpcService>();

#endregion

#region MassTransit-RabbitMq Configuration

//builder.Services.AddMassTransit(config =>
//{
//    config.UsingRabbitMq((ctx, cfg) =>
//    {
//        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
//    });
//});

////builder.Services.AddMassTransitHostedService();
//builder.Services.AddOptions<MassTransitHostedService>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
