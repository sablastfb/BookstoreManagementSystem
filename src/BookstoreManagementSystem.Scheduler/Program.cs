using BookstoreManagementSystem.Scheduler.Jobs;
using BookstoreManagementSystem.Scheduler.Services;
using BookstoreManagementSystem.WebApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

var builder = Host.CreateDefaultBuilder(args)
  .ConfigureServices((hostContext, services) =>
  {
    services.AddDbContext<BookstoreDbContext>(options =>
      options.UseNpgsql(hostContext.Configuration.GetConnectionString("BookstoreDb")));
    services.AddQuartz(q =>
    {
      var jobKey = new JobKey("BookImportJob");
      q.AddJob<BookImportJob>(opts => opts.WithIdentity(jobKey));
            
      q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("BookImportTrigger")
        .WithCronSchedule(hostContext.Configuration["Quartz:BookImportSchedule"] ?? "0 * * * * ?"));
    });

    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    services.AddScoped<IBookImportService, BookImportService>();
    services.AddScoped<IGetFakeDataService, GetFakeDataService>();
  });

await builder.Build().RunAsync();
