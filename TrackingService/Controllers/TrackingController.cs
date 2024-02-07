using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TrackingService.Data;
using TrackingService.DTOs;
using TrackingService.Models;

namespace TrackingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingRepository _repository;
        private readonly IMapper _mapper;

        public TrackingController(
            ITrackingRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<Courier>> Get() => await _repository.GetAsync();

        [HttpGet("CourierId/{id:length(24)}", Name = "GetById")]
        public async Task<ActionResult<Courier>> GetById(string id)
        {
            var courier = await _repository.GetByIdAsync(id);

            if (courier == null)
            {
                return NotFound();
            }

            return Ok(courier);
        }

        [HttpGet("BookingId/{bookingId}")]
        public async Task<ActionResult<Courier>> GetByBookingId(int bookingId)
        {
            var courier = await _repository.GetByBookingIdAsync(bookingId);

            if (courier == null)
            {
                return NotFound();
            }

            return Ok(courier);
        }

        #region EVENT ENDPOINTS
        [HttpPost]
        public async Task<ActionResult<CourierReadDto>> Create(CourierCreateDto courierCreateDto)
        {
            var newCourier = _mapper.Map<Courier>(courierCreateDto);

            await _repository.CreateAsync(newCourier);

            var courierReadDto = _mapper.Map<CourierReadDto>(newCourier);

            return Ok(courierReadDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourierCreateDto courierCreateDto)
        {
            var courier = await _repository.GetByBookingIdAsync(_mapper.Map<Courier>(courierCreateDto).BookingId);

            if (courier == null)
            {
                return NotFound();
            }

            await _repository.UpdateAsync(courier.Id, courier);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var courier = await _repository.GetByIdAsync(id);

            if (courier == null)
            {
                return NotFound();
            }

            await _repository.RemoveAsync(id);

            return NoContent();
        }
        #endregion
    }
}
