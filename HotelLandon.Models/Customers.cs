using System;

namespace HotelLandon.Models
{
    public class EntityBase
    {
        public int Id { get; set; }
    }

    public class Customer : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class Reservation : EntityBase
    {
        public DateTime Start       { get; set; }
        public DateTime End         { get; set; }
        public Room     Room        { get; set; }
        public Customer Customer    { get; set; }
    }

    public class Room : EntityBase
    {
        public int Number { get; set; }
    }
}
