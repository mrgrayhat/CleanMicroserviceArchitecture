using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SaeedRezayi.LogModule.Models;

namespace SaeedRezayi.LogModule.Services
{
    public class LogService : ILogService//, IDisposable
    {
        private readonly IMongoCollection<LogInfo> _informationLogs;
        private readonly IMongoCollection<LogInfo> _errorLogs;
        private ILogger<LogService> _logger;

        public LogService(ILogger<LogService> logger, ILogDatabaseSettings settings)
        {
            _logger = logger;
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);

            Console.WriteLine("LogModule stablished MongoDB connection");
            _logger.LogInformation("LogModule stablished MongoDB connection");

            _informationLogs = database
                .GetCollection<LogInfo>(settings.InformationLogCollectionName);
            _errorLogs = database
                .GetCollection<LogInfo>(settings.ErrorLogCollectionName);
        }

        //public string GetLogs()
        //{
        //    var result = _informationLogs.Find(new BsonDocument())
        //        .Project<BsonDocument>(Builders<LogInfo>.Projection.Exclude("_Id")).ToList();
        //    //var test = json[0].Properties;
        //    return result.ToJson();
        //}
        public async Task<PagedLogsListViewModel> FilterLogs(string term, string field, int pageNumber, string sortOrder = "Desc", int recordsPerPage = 10, LogLevels logLevel = LogLevels.ALL)
        {
            var skipRecords = pageNumber * recordsPerPage;
            IMongoQueryable<LogInfo> query;
            var filter = Builders<LogInfo>.Filter.Eq(x => x.Properties[field], term);

            if (logLevel == LogLevels.Information)
            {
                query = _informationLogs.AsQueryable();
                query = (from c in query
                         orderby c.Id descending
                         select c);
                return new PagedLogsListViewModel
                {
                    Paging = { TotalItems = await query.CountAsync() },
                    Logs = await _informationLogs
                   .Find(filter)
                   // .Project(x => (LogViewModel)x)
                   .Skip(skipRecords)
                   .Limit(recordsPerPage)
                   .ToListAsync()
                };
            }
            else // (logLevel == LogLevels.Error)
            {
                query = _errorLogs.AsQueryable();
                query = (from c in query
                         orderby c.Id descending
                         select c);
                return new PagedLogsListViewModel
                {
                    Paging = { TotalItems = await query.CountAsync() },
                    Logs = await _errorLogs
                    .Find(filter)
                    //.Project(x => (LogViewModel)x)
                    .Skip(skipRecords)
                    .Limit(recordsPerPage)
                    .ToListAsync()
                };
            }
            //var result = await _informationLogs
            //    .Find(filter)
            //    .ToListAsync();

        }
        public async Task<PagedLogsListViewModel> GetLogs(int pageNumber, string sortByField,
            string sortOrder, int recordsPerPage = 10,
            LogLevels logLevel = LogLevels.ALL)
        {
            var skipRecords = pageNumber * recordsPerPage;
            IMongoQueryable<LogInfo> query;

            if (logLevel == LogLevels.Information)
            {
                _logger.LogInformation("Making Query for Get Information Logs!");
                query = _informationLogs.AsQueryable();
            }
            else if (logLevel == LogLevels.Error)
            {
                _logger.LogInformation("Making Query for Get Error Logs!");
                query = _errorLogs.AsQueryable();
            }
            else
            {
                _logger.LogInformation("Making Query for Get Error Logs!");
                query = _errorLogs.AsQueryable();
            }

            query = sortByField switch
            {
                // descending by date time
                "Time" => sortOrder == "Desc" ? query.OrderByDescending(c => c.Timestamp).ThenByDescending(u => u.Timestamp) : query.OrderBy(c => c.Timestamp).ThenBy(u => u.Timestamp),

                _ => (from c in query
                      orderby c.Id descending
                      select c)
            };
            //PagedLogsListViewModel paged = null;
            _logger.LogInformation($"Query Executed Successfully for {recordsPerPage} Logs, with {sortByField} field filter.");
            return new PagedLogsListViewModel
            {
                Paging =
                {
                    TotalItems = await query.CountAsync()
                },
                Logs = await query
                //.Select(x => (LogViewModel)x)
                .Skip(skipRecords)
                .Take(recordsPerPage)
                .ToListAsync()
            };
        }
        public async Task<LogInfo> Get(string id, LogLevels logLevels = LogLevels.ALL)
        {
            LogInfo query = logLevels switch
            {

                LogLevels.Error => await _errorLogs.Find<LogInfo>(log => log.Id == id).FirstOrDefaultAsync(),

                LogLevels.Information => await _informationLogs.Find<LogInfo>(log => log.Id == id).FirstOrDefaultAsync(),

                _ => await _errorLogs.Find<LogInfo>(log => log.Id == id)
                    .FirstOrDefaultAsync() == null ?
                    await _informationLogs.Find<LogInfo>(log => log.Id == id).FirstOrDefaultAsync() : null
            };
            return query;
        }
        public async Task<LogInfo> Remove(string id, LogLevels logLevels = LogLevels.ALL)
        {
            LogInfo query = logLevels switch
            {
                LogLevels.Error => await _errorLogs
                .FindOneAndDeleteAsync<LogInfo>(log => log.Id == id),

                LogLevels.Information => await _informationLogs
                .FindOneAndDeleteAsync<LogInfo>(log => log.Id == id),

                _ => await _errorLogs
                .FindOneAndDeleteAsync<LogInfo>(log => log.Id == id) == null ?
                    await _informationLogs
                    .FindOneAndDeleteAsync<LogInfo>(log => log.Id == id) : null
            };

            return query;
        }
        /// <summary>
        /// add custom log async
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task Add(LogInfo log, LogLevels logLevel)
        {
            switch (logLevel)
            {
                case LogLevels.Error:
                    await _errorLogs.InsertOneAsync(log);
                    break;
                case LogLevels.Information:
                    await _informationLogs.InsertOneAsync(log);
                    break;
                default:
                    await _informationLogs.InsertOneAsync(log);
                    break;
            }
        }

        //private bool disposedValue = false;
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!disposedValue)
        //    {
        //        if (disposing)
        //        {

        //        }

        //        disposedValue = true;
        //    }
        //}
        // override finalizer only to free unmanaged resources
        //~BlogService()
        //{
        //    Dispose(disposing: false);
        //}
        //public void Dispose()
        //{
        //    Dispose(disposing: true);
        //    GC.SuppressFinalize(this);
        //}
    }
}
