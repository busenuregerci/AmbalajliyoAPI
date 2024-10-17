using System;

namespace Ambalajliyo.BLL.DTOs
{
    /// <summary>
    /// Data transfer object for catalog information.
    /// </summary>
    public class CatalogDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the catalog.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the PDF associated with the catalog.
        /// </summary>
        public string? PdfName { get; set; }
    }
}
