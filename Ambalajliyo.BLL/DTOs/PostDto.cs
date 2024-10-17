using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.DTOs
{
    /// <summary>
    /// Represents a data transfer object for a blog post.
    /// </summary>
    public class PostDto
    {
        
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Başlık gereklidir.")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter uzunluğunda olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Bilgi gereklidir.")]
        public string Info { get; set; }

        public string? Image { get; set; }

        public bool IsPublished { get; set; }
    }
}
