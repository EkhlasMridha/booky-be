using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace DataManagers.Configurations
{
    class BookedRoomConfig : IEntityTypeConfiguration<BookedRoom>
    {
        public void Configure(EntityTypeBuilder<BookedRoom> builder)
        {
            builder.HasKey(bk => new { bk.RoomId, bk.BookingId });
            builder.HasOne(b => b.Room)
                .WithMany(r => r.BookedRooms)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(book => book.Booking)
                .WithMany(Booking => Booking.BookedRoom)
                .HasForeignKey(b => b.BookingId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
