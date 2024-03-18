namespace Petricords.API.Workers;

/// <summary>
/// Base class for all time based workers.
/// </summary>
public abstract class TimerService : BackgroundService
{
    protected readonly ILogger Logger;
    private readonly PeriodicTimer _timer;

    protected TimerService(TimeSpan period, ILogger logger)
    {
        Logger = logger;
        _timer = new PeriodicTimer(period);
    }

    protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            try
            {
                await DoWork(stoppingToken);
            }
            catch (Exception e)
            {
                Logger.LogCritical(e, "Error while executing {WorkerName}", GetType().Name);
            }
        } while (await _timer.WaitForNextTickAsync(stoppingToken));
    }

    public abstract Task DoWork(CancellationToken stoppingToken);
}