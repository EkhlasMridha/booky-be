using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DataManagers.Configurations;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace DataManagers
{
    public class HotelDbContext:DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

        public DbSet<Admin> Admin { get; set; }
        public DbSet<TaxContract> TaxContract { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Guest> Guest { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<BookedRoom> BookedRoom { get; set; }
        public DbSet<Session> Session { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
