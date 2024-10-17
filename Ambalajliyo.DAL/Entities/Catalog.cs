using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambalajliyo.DAL.Entities
{
    /// <summary>
    /// Represents a catalog entry in the Ambalajliyo application.
    /// </summary>
    public class Catalog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? PdfName { get; set; }
    }
}