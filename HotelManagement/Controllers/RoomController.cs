using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataManagers.Managers.interfaces;
using Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Models.Dtos;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelManagement.Controllers
{
    [Route("v1/room")]
    [ApiController]
    [Authorize]
    public class RoomController : ControllerBase
    {
        private IRoomDataManager _roomDataManager;
        private IMapper _mapper;
        public RoomController(IRoomDataManager roomDataManager,IMapper mapper)
        {
            _roomDataManager = roomDataManager;
            _mapper = mapper;
        }

        // GET v1/room/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var room = await _roomDataManager.GetRoomById(id);
            return Ok(room);
        }

        // POST api/room
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoomDto roomDto)
        {
            var room = _mapper.Map<Room>(roomDto);
            var cRoom = await _roomDataManager.CreateRoom(room);
            return Ok(cRoom);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoom([FromBody] RoomDto roomDto)
        {
            var room = _mapper.Map<Room>(roomDto);

            var response = await _roomDataManager.UpdateRoomAsync(room);

            return Ok(response);
        }

        [HttpPost("delete")]
        public async Task DeleteRoom([FromBody] RoomDto roomDto)
        {
            var room = _mapper.Map<Room>(roomDto);

            await _roomDataManager.DeleteRoomAsync(room);
        }

        [HttpPost("bymonth")]
        public async Task<IActionResult> GetRoomDataByMonth([FromBody] AppDataFilter appDataFilter)
        {
            var room = await _roomDataManager.GetDataByMonth(appDataFilter.Query);

            return Ok(room);
        }
    }
}
