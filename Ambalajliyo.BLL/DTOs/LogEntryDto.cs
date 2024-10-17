using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.DTOs
{
    /// <summary>
    /// Represents a log entry with details such as timestamp, log level, message, and any associated exception.
    /// </summary>
    public class LogEntryDto
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Level { get; set; }
        public string MessageTemplate { get; set; }
        public string Message { get; set; }
        public string? Exception { get; set; }
        public string Properties { get; set; }
    }
}
