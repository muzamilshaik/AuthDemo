using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemo.Model
{
    public class ApplicationUser : IdentityUser<string> //, IMapTo<User>, IMapFrom<User>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Guid? StorageAreaId { get; set; }
        //public StorageArea StorageArea { get; set; }

        //public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        //public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        //public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        //public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
