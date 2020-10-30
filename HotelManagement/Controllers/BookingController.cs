using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataManagers.Managers.interfaces;
using Models.Entities;
using Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HelperServices;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelManagement.Controllers
{
    [Route("v1/booking")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private IBookingDataManager _bookingDataManager;
        private IMapper _mapper;
        public BookingController(IBookingDataManager bookingDataManager,IMapper mapper)
        {
            _bookingDataManager = bookingDataManager;
            _mapper = mapper;
        }
        // GET: vi/booking/states
        [HttpGet("states")]
        public async Task<IActionResult> GetAllStates()
        {
            var states = await _bookingDataManager.GetAllStates();

            return Ok(states);
        }

        // POST v1/booking/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDto bookingDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string userID = identity.FindFirst(ZeroClaims.UserId)?.Value;
            int id = Int32.Parse(userID);

            if(bookingDto.Book_From == bookingDto.Leave_At)
            {
                return BadRequest("SAME_DATE");
            }
            var booking = _mapper.Map<Booking>(bookingDto);
            var result = await _bookingDataManager.CalculateTax(booking, id);
            var booked =  await _bookingDataManager.CreateBooking(result);

            if (booked == null)
            {
                return BadRequest("BOOKING_EXIST_IN_THIS_RANGE");
            }

            return Ok(booked);
        }

        [HttpPost("validity")]
        public async Task<IActionResult> CheckBookingValidity([FromBody] BookingValidityModel bookingValidityModel)
        {
            var isExist = await _bookingDataManager.CheckBooking(bookingValidityModel.roomId, bookingValidityModel.FromDate, bookingValidityModel.ToDate, bookingValidityModel.BookingId);

            return Ok(isExist);
        }

        // PUT v1/booking/updatestate
        [HttpPut("updatestate")]
        public async Task Put([FromBody] Booking booking)
        {
            var b = await _bookingDataManager.GetBookingById(booking.Id);
            b.StateId = booking.StateId;
            await _bookingDataManager.UpdateBooking(b);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id,[FromBody] BookingDto bookingDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string rawId = identity.FindFirst(ZeroClaims.UserId)?.Value;
            int userId = Int32.Parse(rawId);

            if (bookingDto.Book_From == bookingDto.Leave_At)
            {
                return BadRequest("SAME_DATE");
            }
            var booking = _mapper.Map<Booking>(bookingDto);
            var isExists = await _bookingDataManager.CheckBooking(id, bookingDto.Book_From, bookingDto.Leave_At,bookingDto.Id);
            if (isExists)
            {
                return BadRequest("BOOKING_EXIST_IN_RANGE");
            }
            var withTax = await _bookingDataManager.CalculateTax(booking, userId);
            var update= await _bookingDataManager.UpdateBooking(withTax);

            return Ok(update);
        }

        // DELETE api/<BookingController>/5
        [HttpPost]
        public async Task DeleteBooking([FromBody]BookedRoomDto bookedRoomDto)
        {
            var booking = _mapper.Map<BookedRoom>(bookedRoomDto);
            await _bookingDataManager.DeleteBookedRoom(booking);
        }

        [HttpPost("tax")]
        public async Task<IActionResult> SetBookingTax([FromBody] TaxContract taxContract)
        {
            var tax = await _bookingDataManager.Settax(taxContract);
            return Ok(tax);
        }

        [HttpGet("tax")]
        public async Task<IActionResult> GetBookingTax()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string userID = identity.FindFirst(ZeroClaims.UserId)?.Value;
            int id = Int32.Parse(userID);

            var tax = await _bookingDataManager.GetBookingTax(id);
            return Ok(tax);
        }

        [HttpGet("guest/{id}")]
        public async Task<IActionResult> GetGuestByBookingId(int id)
        {
            var guest = await _bookingDataManager.GetGuestByBookingId(id);

            return Ok(guest);
        } 
    }
}
