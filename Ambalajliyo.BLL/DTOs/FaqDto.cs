using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.DTOs
{
    /// <summary>
    /// Represents a Frequently Asked Question (FAQ) with a question and its corresponding answer.
    /// </summary>
    public class FaqDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Soru alanı zorunludur.")]
        [StringLength(500, ErrorMessage = "Soru 500 karakterden uzun olamaz.")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Cevap alanı zorunludur.")]
        [StringLength(1000, ErrorMessage = "Cevap 1000 karakterden uzun olamaz.")]
        public string Answer { get; set; }
    }
}
