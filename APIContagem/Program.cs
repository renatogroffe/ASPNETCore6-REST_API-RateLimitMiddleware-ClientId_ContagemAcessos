using AspNetCoreRateLimit;
using APIContagem;

var builder = WebApplication.CreateBuilder(args);

#region  Configurações de Rate Limit

var useAspNetCoreRateLimit = Convert.ToBoolean(builder.Configuration["UseAspNetCoreRateLimit"]);
if (useAspNetCoreRateLimit)
{
    builder.Services.AddMemoryCache();
    RateLimitExtensions.Initialize(builder.Configuration);

    builder.Services.Configure<ClientRateLimitOptions>(options =>
    {
        options.EnableEndpointRateLimiting = true;
        options.StackBlockedRequests = false;
        options.HttpStatusCode = 429;
        options.ClientIdHeader = RateLimitExtensions.ClientIdHeaderName;
        options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "GET:/contador",
                    Period = RateLimitExtensions.Period,
                    Limit = RateLimitExtensions.Limit,
                    QuotaExceededResponse = new ()
                    {
                        ContentType = "application/text",
                        Content = RateLimitExtensions.QuotaExceededMessage
                    }
                }
            };
    });

    builder.Services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
    builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
    builder.Services.AddInMemoryRateLimiting();
}

#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

#region Ativando o middleware de Rate Limit
if (useAspNetCoreRateLimit)
    app.UseClientRateLimiting();
#endregion

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();