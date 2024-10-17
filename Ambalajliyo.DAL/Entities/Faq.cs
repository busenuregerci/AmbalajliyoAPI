using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambalajliyo.DAL.Entities
{
    /// <summary>
    /// Represents a frequently asked question (FAQ) in the Ambalajliyo application.
    /// </summary>
    public class Faq
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
