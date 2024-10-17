using Ambalajliyo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.DTOs
{
    /// <summary>
    /// Represents a data transfer object for a product.
    /// </summary>
    public class ProductDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Ad gereklidir.")]
        [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter uzunluğunda olabilir.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Açıklama gereklidir.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Fiyat gereklidir.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat pozitif bir değer olmalıdır.")]
        public decimal Price { get; set; }

        public string? Image { get; set; }


        public Guid? CategoryId { get; set; }
    }
}
