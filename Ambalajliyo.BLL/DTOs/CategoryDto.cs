using Ambalajliyo.DAL.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ambalajliyo.BLL.DTOs
{
    /// <summary>
    /// Data transfer object for category information.
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the category.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// Must be provided and can be up to 100 characters long.
        /// </summary>
        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; }
    }
}
