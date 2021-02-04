using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogModule.Application.Enums;
using LogModule.Application.Interfaces;
using LogModule.Domain.Settings;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LogModule.Infrastructure.Repositories
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<LogModule.Domain.Entities.Log> where T : class
    {
        //private readonly ApplicationDbContext _dbContext;
        private readonly IMongoCollection<LogModule.Domain.Entities.Log> _logs;

        public GenericRepositoryAsync(ILogDatabaseSettings settings)
        {
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);

            Console.WriteLine("LogModule stablished MongoDB connection");
            //_logger.LogInformation("LogModule stablished MongoDB connection");

            _logs = database.GetCollection<LogModule.Domain.Entities.Log> (settings.CollectionName);

        }

        public virtual async Task<LogModule.Domain.Entities.Log> GetByIdAsync(string id)
        {
            return (LogModule.Domain.Entities.Log)await _logs.FindAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<LogModule.Domain.Entities.Log>> GetPagedReponseAsync(int pageNumber, int pageSize, LogLevels logLevel = LogLevels.ALL, string sorOrder = "desc")
        {
            IMongoQueryable<LogModule.Domain.Entities.Log> query = _logs.AsQueryable();

            switch (logLevel)
            {
                case LogLevels.ALL:
                    break;
                case LogLevels.Information:
                    query = query.Where(x => x.Level == "Information");
                    break;
                case LogLevels.Error:
                    query = query.Where(x => x.Level == "Error");
                    break;
                case LogLevels.Warning:
                    query = query.Where(x => x.Level == "Warning");
                    break;
                case LogLevels.Debug:
                    query = query.Where(x => x.Level == "Debug");
                    break;
                case LogLevels.Fatal:
                    query = query.Where(x => x.Level == "Fatal");
                    break;
                case LogLevels.Critical:
                    query = query.Where(x => x.Level == "Critical");
                    break;
                case LogLevels.Trace:
                    query = query.Where(x => x.Level == "Trace");
                    break;
                case LogLevels.Verbose:
                    query = query.Where(x => x.Level == "Verbose");
                    break;
                default:
                    break;
            }

            switch (sorOrder)
            {
                default:
                    query = query.OrderByDescending(x => x.Id);//.ThenByDescending(t => t.Timestamp);
                    break;
            }
            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<LogModule.Domain.Entities.Log> AddAsync(LogModule.Domain.Entities.Log entity)
        {
            await _logs.InsertOneAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(LogModule.Domain.Entities.Log entity)
        {
            await _logs.FindOneAndDeleteAsync(e => e.Id == entity.Id);
        }

        public async Task<IReadOnlyList<LogModule.Domain.Entities.Log>> GetAllAsync()
        {
            return await _logs
                 .AsQueryable()
                 .ToListAsync();
        }
    }
}
