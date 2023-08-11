using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotApiGw.DelegateHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<TokenExchangeDelegateHandler>();

builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options =>
{
    options.Authority = builder.Configuration["IdentityServerUrl"];
    options.Audience = "resource_gateway";
    options.RequireHttpsMetadata = false;
});

builder.Services.AddOcelot(builder.Configuration)
                .AddCacheManager(settings => settings.WithDictionaryHandle())
                .AddDelegatingHandler<TokenExchangeDelegateHandler>();

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);


var app = builder.Build();

await app.UseOcelot();

app.Run();
