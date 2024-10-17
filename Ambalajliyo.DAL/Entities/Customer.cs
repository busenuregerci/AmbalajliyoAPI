using System;

namespace Ambalajliyo.DAL.Entities
{
    /// <summary>
    /// Represents a customer in the Ambalajliyo application.
    /// </summary>
    public class Customer
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public bool IsItAnswered { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
