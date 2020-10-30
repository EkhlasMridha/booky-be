using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Models.Dtos;
using Models.Entities;

namespace HotelManagement
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<BookedRoomDto, BookedRoom>();
            CreateMap<GuestDto, Guest>();
            CreateMap<BookingDto, Booking>();
            CreateMap<RoomDto, Room>();
        }
    }
}
