using System.Security.Cryptography;
using System.Text;
using backend.Interfaces;
using backend.Modules.Webhooks.Interfaces;

namespace backend.Modules.Webhooks.Services;

public class WebhookBackgroundService : BackgroundService
{
    private readonly WebhookQueue _queue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WebhookBackgroundService> _logger;
    private readonly HttpClient _httpClient;

    public WebhookBackgroundService(
        WebhookQueue queue,
        IServiceProvider serviceProvider,
        ILogger<WebhookBackgroundService> logger,
        HttpClient httpClient)
    {
        _queue = queue;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _httpClient = httpClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var deliveryId = await _queue.DequeueAsync(stoppingToken);

                // Process in background, don't block queue reading heavily
                _ = ProcessDeliveryAsync(deliveryId, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while dequeuing webhook.");
            }
        }
    }

    private async Task ProcessDeliveryAsync(Guid deliveryId, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var webhookRepository = scope.ServiceProvider.GetRequiredService<IWebhookRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var delivery = await webhookRepository.GetDeliveryByIdAsync(deliveryId, cancellationToken);
        if (delivery == null || delivery.Status == "Success") return;

        // Need subscription to get URL and Secret. 
        // Our repo GetByIdAsync doesn't include it. We should use a method that includes it or load it manually.
        // Let's assume generic GetById doesn't include navigational props, so we load subscription directly.
        var endpoint = await webhookRepository.GetByIdAsync(delivery.WebhookEndpointId, cancellationToken);
        if (endpoint == null || endpoint.Status != "Active") return;

        delivery.RetryCount++;
        delivery.DeliveredAt = DateTime.UtcNow;

        try
        {
            var content = new StringContent(delivery.Payload, Encoding.UTF8, "application/json");

            // Add signature if secret exists
            if (!string.IsNullOrWhiteSpace(endpoint.Secret))
            {
                var signature = GenerateSignature(delivery.Payload, endpoint.Secret);
                content.Headers.Add("X-Hub-Signature-256", $"sha256={signature}");
            }

            var response = await _httpClient.PostAsync(endpoint.EndpointUrl, content, cancellationToken);

            delivery.StatusCode = (int)response.StatusCode;
            delivery.ResponseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            delivery.ResponseHeaders = response.Headers.ToString();

            if (response.IsSuccessStatusCode)
            {
                delivery.Status = "Success";
            }
            else
            {
                delivery.Status = delivery.RetryCount >= 3 ? "Failed" : "Pending";
                
                // If it failed and we can retry, re-queue after delay (simplified)
                if (delivery.Status == "Pending")
                {
                    _ = Task.Run(async () =>
                    {
                        await Task.Delay(TimeSpan.FromMinutes(1 * delivery.RetryCount), cancellationToken);
                        await _queue.EnqueueAsync(delivery.Id, cancellationToken);
                    }, cancellationToken);
                }
            }
        }
        catch (Exception ex)
        {
            delivery.Status = delivery.RetryCount >= 3 ? "Failed" : "Pending";
            delivery.ResponseBody = ex.Message;
            
            if (delivery.Status == "Pending")
            {
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMinutes(1 * delivery.RetryCount), cancellationToken);
                    await _queue.EnqueueAsync(delivery.Id, cancellationToken);
                }, cancellationToken);
            }
        }

        webhookRepository.UpdateDeliveryAsync(delivery);
        await unitOfWork.CompleteAsync(cancellationToken);
    }

    private static string GenerateSignature(string payload, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
