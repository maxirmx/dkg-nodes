﻿using dkgNode.Services;
using static dkgCommon.Constants.NStatus;
using dkgNode.Models;

namespace dkgNode
{
    public class DkgNodeWorker(DkgNodeConfig config, ILogger<DkgNodeService> logger, bool dos2 = false, bool dos3 = false) : BackgroundService
    {
        internal DkgNodeService Service = new(config, logger, dos2, dos3);
        internal int PollingInterval = config.PollingInterval;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var httpClient = new HttpClient();
            while (!stoppingToken.IsCancellationRequested)
            {
                if (Service.GetStatus() == NotRegistered)
                {
                    await Service.Register(httpClient);
                }

                var statusResponse = await Service.ReportStatus(httpClient, null);

                if (Service.GetStatus() == RunningStepOne)
                {
                    await Service.RunDkg(httpClient, statusResponse.Data, stoppingToken);
                    Service.UpdateKeys();
                }
                else
                {
                    Thread.Sleep(PollingInterval);
                }
            }
        }

    }
}
