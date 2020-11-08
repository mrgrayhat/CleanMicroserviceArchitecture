using System;
using Serilog.Core;
using Serilog.Events;

namespace SaeedRezayi.LogModule.Configuration
{
    public class RemovePropertyEnrich : ILogEventEnricher
    {

        public void Enrich(LogEvent le, ILogEventPropertyFactory lepf)
        {
            le.RemovePropertyIfPresent("MessageTemplate");
            //var username = context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "anonymous";
        }

    }
}
