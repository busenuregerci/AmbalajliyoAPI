using Ambalajliyo.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.Interfaces
{
    /// <summary>
    /// Defines methods for interacting with log entries.
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// Retrieves all log entries asynchronously within the specified date range.
        /// </summary>
        /// <param name="startDate">The start date for filtering logs. If null, no lower bound is applied.</param>
        /// <param name="endDate">The end date for filtering logs. If null, no upper bound is applied.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of log entries.</returns>
        Task<List<LogEntryDto>> GetAllLogs(DateTime? startDate, DateTime? endDate);
    }
}
