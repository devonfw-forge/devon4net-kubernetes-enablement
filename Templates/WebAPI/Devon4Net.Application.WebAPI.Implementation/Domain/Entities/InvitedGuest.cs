using System;
using System.Collections.Generic;

namespace Devon4Net.Application.WebAPI.Implementation.Domain.Entities

{
    public partial class InvitedGuest
    {
        public InvitedGuest()
        {
            Order = new HashSet<Order>();
        }

        public long Id { get; set; }
        public long IdBooking { get; set; }
        public string GuestToken { get; set; }
        public string Email { get; set; }
        public bool? Accepted { get; set; }
        public DateTime? ModificationDate { get; set; }

        public Booking IdBookingNavigation { get; set; }
        public ICollection<Order> Order { get; set; }
    }
}
