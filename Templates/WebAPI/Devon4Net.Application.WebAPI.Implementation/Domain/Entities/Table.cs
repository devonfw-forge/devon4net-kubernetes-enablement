using System.Collections.Generic;

namespace Devon4Net.Application.WebAPI.Implementation.Domain.Entities

{
    public partial class Table
    {
        public Table()
        {
            Booking = new HashSet<Booking>();
        }

        public long Id { get; set; }
        public int SeatsNumber { get; set; }

        public ICollection<Booking> Booking { get; set; }
    }
}
