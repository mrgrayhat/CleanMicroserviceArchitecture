using System;
using System.Threading.Tasks;
using LogModule.Application.Interfaces.Repositories;
using LogModule.Domain.Settings;
using MongoDB.Driver;

namespace LogModule.Infrastructure.Repositories
{
    public class LogRepositoryAsync : GenericRepositoryAsync<LogModule.Domain.Entities.Log>, ILogRepositoryAsync
    {
        private readonly IMongoCollection<LogModule.Domain.Entities.Log> _logs;

        public LogRepositoryAsync(ILogDatabaseSettings settings) : base(settings)
        {
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);

            Console.WriteLine("LogModule stablished MongoDB connection");
            //_logger.LogInformation("LogModule stablished MongoDB connection");

            _logs = database.GetCollection<LogModule.Domain.Entities.Log>(settings.CollectionName);
        }

        public Task<bool> IsUniqueBarcodeAsync(string barcode)
        {
            return _logs.Find(x => x.Id != barcode).AnyAsync();
        }

    }
}