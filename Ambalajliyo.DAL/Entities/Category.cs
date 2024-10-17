using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Ambalajliyo.DAL.Entities
{
    /// <summary>
    /// Represents a category in the Ambalajliyo application.
    /// </summary>
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
