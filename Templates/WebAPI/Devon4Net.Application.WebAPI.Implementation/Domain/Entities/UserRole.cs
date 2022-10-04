using System.Collections.Generic;

namespace Devon4Net.Application.WebAPI.Implementation.Domain.Entities

{
    public partial class UserRole
    {
        public UserRole()
        {
            User = new HashSet<User>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }

        public ICollection<User> User { get; set; }
    }
}
