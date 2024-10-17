using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Ambalajliyo.DAL.Entities
{
    /// <summary>
    /// Represents a role in the Ambalajliyo application with additional properties.
    /// </summary>
    public class AmbalajliyoRole : IdentityRole
    {
        public bool IsDeleted { get; set; } = false;
        public List<AmbalajliyoUser> AmbalajliyoUsers { get; set; }
    }
}
