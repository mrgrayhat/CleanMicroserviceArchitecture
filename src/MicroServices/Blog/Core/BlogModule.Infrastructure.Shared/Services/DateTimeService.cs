using System;
using BlogModule.Application.Interfaces;

namespace BlogModule.Infrastructure.Shared.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
