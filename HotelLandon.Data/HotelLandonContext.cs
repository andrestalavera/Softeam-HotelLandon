﻿using HotelLandon.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelLandon.Data
{
    public class HotelLandonContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
    }
}
