using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Ambalajliyo.DAL.Entities
{
    /// <summary>
    /// Represents a user in the Ambalajliyo application with additional properties.
    /// </summary>
    public class AmbalajliyoUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public string AmbalajliyoRoleId { get; set; }
        public AmbalajliyoRole AmbalajliyoRole { get; set; }
    }
}
