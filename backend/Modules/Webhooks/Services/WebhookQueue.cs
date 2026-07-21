using System.Threading.Channels;

namespace backend.Modules.Webhooks.Services;

public class WebhookQueue
{
    private readonly Channel<Guid> _queue;

    public WebhookQueue(int capacity = 10000)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<Guid>(options);
    }

    public async ValueTask EnqueueAsync(Guid deliveryId, CancellationToken cancellationToken = default)
    {
        await _queue.Writer.WriteAsync(deliveryId, cancellationToken);
    }

    public async ValueTask<Guid> DequeueAsync(CancellationToken cancellationToken = default)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}
