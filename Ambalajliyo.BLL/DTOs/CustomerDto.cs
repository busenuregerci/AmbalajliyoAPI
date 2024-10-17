using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.DTOs
{
    /// <summary>
    /// Represents a customer with details including personal information, contact details, and related product information.
    /// </summary>
    public class CustomerDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Başlık 100 karakterden uzun olamaz.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "İsim alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "İsim 50 karakterden uzun olamaz.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyisim alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "Soyisim 50 karakterden uzun olamaz.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mesaj alanı zorunludur.")]
        [StringLength(500, ErrorMessage = "Mesaj 500 karakterden uzun olamaz.")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Adres alanı zorunludur.")]
        [StringLength(200, ErrorMessage = "Adres 200 karakterden uzun olamaz.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Şehir alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Şehir 100 karakterden uzun olamaz.")]
        public string City { get; set; }

        public bool IsItAnswered { get; set; } = false;

        [Required(ErrorMessage = "Oluşturulma tarihi zorunludur.")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        [Required(ErrorMessage = "Ürün ID alanı zorunludur.")]
        public Guid ProductId { get; set; }

        public ProductDto? Product { get; set; }
    }
}
