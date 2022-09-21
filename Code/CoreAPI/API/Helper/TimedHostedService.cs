using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using API.Context;

namespace API.Helper
{
    #region snippet1
    public class TimedHostedService : IHostedService, IDisposable
    {
       
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;
        
        public TimedHostedService(ILogger<TimedHostedService> logger)
        {
            _logger = logger;
          
        }
       

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            DBContext db = new DBContext();
            //executionCount++;
            TimedTasks timedTasks = new TimedTasks(db);
            //_logger.LogInformation(
            //    "Timed Hosted Service is working. Count: {Count}", executionCount);
            if (DateTime.Now.ToString("hh:mm tt") == "12:00 AM")
            {               
                timedTasks.applicationInfoMail();
            }

            if (DateTime.Now.ToString("hh:mm tt") == "06:00 AM")
            {
                //timedTasks.productRateNotification();
            }
            

        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
    #endregion
}
