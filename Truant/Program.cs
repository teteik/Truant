using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Truant;
using Truant.Core;
using Truant.Shared.Utils;
using Truant.Strategies;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((hostContext, services) =>
{
    services.AddDbContext<TruantDbContext>(options =>
            options.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection")),
        ServiceLifetime.Singleton); 

    services.AddSingleton<IRandomProvider, DefaultRandomProvider>();
    services.AddSingleton<ISkipStrategy, FirstSkipStrategy>();
    services.AddSingleton<SemesterManager>();
    
    services.AddHostedService<SemesterWorker>();
});

var host = builder.Build();

Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    Console.WriteLine("Получен сигнал прерывания. Завершение работы.");
};

await host.RunAsync();