using System;

namespace HotelLandon.Models
{
    public class Reservation : EntityBase
    {
        public DateTime Start       { get; set; }
        public DateTime End         { get; set; }
        public Room     Room        { get; set; }
        public Customer Customer    { get; set; }
    }
}
