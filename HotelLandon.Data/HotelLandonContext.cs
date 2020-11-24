using HotelLandon.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelLandon.Data
{
    public class HotelLandonContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=tcp:softeamandrestalavera.database.windows.net,1433;Initial Catalog=HotelLandon;Persist Security Info=False;User ID=Andres;Password=@NDRESmdp*032669;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}
