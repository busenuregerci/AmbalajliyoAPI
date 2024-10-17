using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambalajliyo.DAL.Entities
{
    /// <summary>
    /// Represents a product in the Ambalajliyo application.
    /// </summary>
    [Index(nameof(Name), IsUnique = true)]
    public class Product
    {
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public Guid? CategoryId { get; set; }

        public Category? Category { get; set; }
        public List<Customer>? Customers { get; set; }
    }
}
