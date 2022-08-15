namespace APIContagem;

public static class RateLimitExtensions
{
    private static int _limit;
    public static int Limit { get => _limit; }
    
    private static string? _period;
    public static string? Period { get => _period; }
    
    private static string? _quotaExceededMessage;
    public static string? QuotaExceededMessage { get => _quotaExceededMessage; }

    private static string? _clientIdHeaderName;
    public static string? ClientIdHeaderName { get => _clientIdHeaderName; }

    public static void Initialize(IConfiguration configuration)
    {
        _limit = Convert.ToInt32(configuration["AspNetCoreRateLimit:RequestsLimit"]);
        _period = configuration["AspNetCoreRateLimit:Period"];
        _quotaExceededMessage =
            $"Limite de utilização atingido: {_limit} requisições a cada {_period} para cada Subscription! Aguarde e tente novamente...";
        _clientIdHeaderName = configuration["AspNetCoreRateLimit:ClientIdHeader"];
    }
}