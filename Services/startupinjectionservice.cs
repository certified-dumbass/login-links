using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dreamstreaming.TermsPlugin.Services;

public class StartupInjectionService : BackgroundService
{
    private readonly LoginPageInjectionService _injectionService;
    private readonly ILogger<StartupInjectionService> _logger;

    public StartupInjectionService(
        LoginPageInjectionService injectionService,
        ILogger<StartupInjectionService> logger)
    {
        _injectionService =
            injectionService;

        _logger =
            logger;
    }


    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Dreamstreaming Terms startup service started."
        );


        // Give Jellyfin and the other plugins a little time
        // to finish loading before the first registration attempt.
        try
        {
            await Task.Delay(
                TimeSpan.FromSeconds(2),
                stoppingToken
            );
        }
        catch (OperationCanceledException)
        {
            return;
        }


        const int maximumAttempts = 15;

        for (var attempt = 1;
             attempt <= maximumAttempts;
             attempt++)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                return;
            }


            _logger.LogDebug(
                "Attempt {Attempt}/{MaximumAttempts} to register Dreamstreaming Terms login script.",
                attempt,
                maximumAttempts
            );


            var registered =
                _injectionService.RegisterScript();


            if (registered)
            {
                _logger.LogInformation(
                    "Dreamstreaming Terms is ready. Login links were registered successfully."
                );

                return;
            }


            if (attempt >= maximumAttempts)
            {
                break;
            }


            try
            {
                await Task.Delay(
                    TimeSpan.FromSeconds(2),
                    stoppingToken
                );
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }


        _logger.LogWarning(
            "Dreamstreaming Terms could not register its login script after {Attempts} attempts. " +
            "Make sure JavaScript Injector is installed, enabled and compatible with this Jellyfin version.",
            maximumAttempts
        );
    }


    public override Task StopAsync(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Dreamstreaming Terms startup service is stopping."
        );


        _injectionService.UnregisterScript();


        return base.StopAsync(
            cancellationToken
        );
    }
}