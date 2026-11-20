using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Truant.Shared.Models;
using Truant.Shared.Utils;
using Truant.Strategies;

namespace Truant.Core;

public class SemesterWorker(
    SemesterManager semesterManager,
    IRandomProvider random,
    ISkipStrategy strategy,
    ILogger<SemesterWorker> logger,
    IHostApplicationLifetime appLifetime) : BackgroundService
{
    private const int Delay = 500; 

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var args = Environment.GetCommandLineArgs();
        long? resumeId = null;
        
        for (int i = 1; i < args.Length - 1; i++)
        {
            if (args[i] == "--resume" && long.TryParse(args[i + 1], out var id))
            {
                resumeId = id;
                break;
            }
        }

        SemesterContext context;
        try
        {
            if (resumeId.HasValue)
            {
                logger.LogInformation("Возобновление семестра {Id}...", resumeId.Value);
                context = await semesterManager.ResumeAsync(resumeId.Value);
            }
            else
            {
                logger.LogInformation("Создание нового семестра...");
                context = await semesterManager.CreateNewAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка инициализации семестра");
            appLifetime.StopApplication();
            return;
        }

        var simulator = new SemesterSimulator(
            random,
            strategy,
            context.History,
            context.Professors,
            context.StartDay,
            context.StartPleasure,
            context.BaseRandomSeed);

        logger.LogInformation(
            "Симуляция запущена! (ID: {Id}, День: {Day}, Удовольствие: {Pleasure}). Ctrl+C для сохранения и выхода.",
            context.SemesterId, context.StartDay, context.StartPleasure);

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = simulator.SimulateDay();

            await semesterManager.RecordDayStatisticsAsync(
                context.SemesterId, result.Day, result.TotalPleasure, result.Outcome);

            var avg = (double)result.TotalPleasure / result.Day;

            switch (result.Outcome)
            {
                case DayOutcome.Continued:
                    logger.LogInformation(
                        "День {Day}: Удовольствие = {Total}, среднее = {avg:F3}",
                        result.Day, result.TotalPleasure, avg);
                    break;

                case DayOutcome.Expelled:
                    logger.LogWarning("Студент отчислен на {Day} день!", result.Day);
                    await semesterManager.UpdateStatusAsync(context.SemesterId, 2);
                    appLifetime.StopApplication();
                    return;

                case DayOutcome.SemesterCompleted:
                    logger.LogInformation(
                        "Семестр завершён! Итоговое удовольствие = {Total}, среднее = {avg:F3}",
                        result.TotalPleasure, avg);
                    await semesterManager.UpdateStatusAsync(context.SemesterId, 1);
                    appLifetime.StopApplication();
                    return;
            }

            try
            {
                await Task.Delay(Delay, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                logger.LogInformation(
                    "Симуляция прервана. Состояние (День {Day}) сохранено для будущего resume.",
                    result.Day);
                return;
            }
        }
    }
}