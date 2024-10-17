using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambalajliyo.DAL.Entities
{
    /// <summary>
    /// Represents a blog post in the Ambalajliyo application.
    /// </summary>
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public string? Image { get; set; }

        public bool IsPublished{ get; set; }
    }
}
