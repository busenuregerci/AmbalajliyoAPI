using Ambalajliyo.BLL.DTOs;
using Ambalajliyo.BLL.Interfaces;
using Ambalajliyo.DAL.AbstractRepository;
using Ambalajliyo.DAL.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.Services
{
    /// <summary>
    /// Provides implementation for log-related operations defined in the <see cref="ILogService"/> interface.
    /// </summary>
    public class LogService : ILogService
    {
        private readonly IRepository<LogEntry> _logRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogService"/> class.
        /// </summary>
        /// <param name="logRepository">The repository to handle log data access.</param>
        public LogService(IRepository<LogEntry> logRepository)
        {
            _logRepository = logRepository;
        }

        /// <summary>
        /// Retrieves all log entries within a specified date range.
        /// </summary>
        /// <param name="startDate">The start date of the range (inclusive).</param>
        /// <param name="endDate">The end date of the range (inclusive).</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="LogEntryDto"/> objects.</returns>
        public async Task<List<LogEntryDto>> GetAllLogs(DateTime? startDate, DateTime? endDate)
        {
            var logs = await _logRepository.GetAllLogAsync(startDate, endDate);
            // Map list of entities to list of DTOs
            return logs.Adapt<List<LogEntryDto>>();
        }
    }
}
