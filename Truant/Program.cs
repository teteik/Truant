using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Truant.Core;
using Truant.Strategies;
using Truant.Strategy;
using Truant.Utils;

var builder = Host.CreateDefaultBuilder();
builder.ConfigureServices((hostContext, services) =>
{
   services.AddSingleton<IRandomProvider, DefaultRandomProvider>();
   services.AddSingleton<ISkipStrategy, FirstSkipStrategy>();
   services.AddSingleton<SemesterSimulator>();
   services.AddHostedService<SemesterWorker>();
});

var host = builder.Build();

Console.CancelKeyPress += (sender, eventArgs) =>
{
   eventArgs.Cancel = true;
   Console.WriteLine("Получен сигнал прерывания. Завершение работы.");
};

await host.RunAsync();