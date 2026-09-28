using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Truant.Core;

public class SemesterWorker(SemesterSimulator simulator, ILogger<SemesterWorker> logger, IHostApplicationLifetime appLifetime) 
    : BackgroundService
{
    private const int Delay = 10;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Симуляция запущена! (Ctrl+C для остановки)");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var result = simulator.SimulateDay();
            
            var avg = (double)result.TotalPleasure / result.Day;

            switch (result.Outcome)
            {
                case DayOutcome.Continued:
                    logger.LogInformation("День {Day}: Удовольствие = {Total}, среднее удовольствие в день = {avg:F3}",  result.Day, result.TotalPleasure, avg);
                    break;
                case DayOutcome.Expelled:
                    logger.LogWarning("Студент отчислен на {Day} день! Удовольствие обнулено.", result.Day);
                    appLifetime.StopApplication();
                    return;
                case DayOutcome.SemesterCompleted:
                    logger.LogInformation("Семестр успешно завершён! Итоговое удовольствие = {Total}, среднее удовольствие в день = {avg:F3}",  result.TotalPleasure, avg);
                    appLifetime.StopApplication();
                    return;
                default:
                    return;
            }


            try
            {
                await Task.Delay(Delay, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                logger.LogInformation("Симуляция прервана пользователем");
                return;
            }
        }
    }
}