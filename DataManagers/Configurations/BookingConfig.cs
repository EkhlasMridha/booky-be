using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataManagers.Configurations
{
    class BookingConfig : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.state)
                .WithMany(c => c.booking)
                .HasForeignKey(e => e.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.BookedRoom)
                .WithOne(b => b.Booking)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.Guest)
                   .WithOne(c => c.Booking)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
