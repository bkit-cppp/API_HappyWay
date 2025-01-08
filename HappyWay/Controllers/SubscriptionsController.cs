using HappyWay.Domain;
using HappyWay.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyWay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public SubscriptionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Subscriptions = await _unitOfWork.Subscriptions.GetAllAsync();
            return Ok(Subscriptions);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var SubscriptionsId = await _unitOfWork.Subscriptions.GetByIdAsync(id);
            if (SubscriptionsId == null)
            {
                return BadRequest("Id Not Found");
            }
            return Ok(SubscriptionsId);
        }
        [HttpPost]
        public async Task<IActionResult> AddSubscriptions([FromBody] Subscription Subscriptions)
        {
            if (Subscriptions == null)
            {
                return BadRequest("Invalid Data");

            }
            await _unitOfWork.Subscriptions.AddAsync(Subscriptions);
            await _unitOfWork.SaveAsync();
            return Ok(Subscriptions);
        }
        [HttpPut]
        public async Task<IActionResult> EditSubscriptions(int id, [FromBody] Subscription Subscriptions)
        {
            var existingSubscriptions = await _unitOfWork.Subscriptions.GetByIdAsync(id);
            if (existingSubscriptions == null)
            {
                return BadRequest("Not Found");
            }
            existingSubscriptions.Email = Subscriptions.Email;
            existingSubscriptions.CreatedAt = DateTime.Now;
            _unitOfWork.Subscriptions.Update(existingSubscriptions);
            await _unitOfWork.SaveAsync();
            return Ok(existingSubscriptions);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscriptions(int id)
        {
            var Subscriptions = await _unitOfWork.Subscriptions.GetByIdAsync(id);
            if (Subscriptions == null)
            {
                return BadRequest(" Id Not Found");
            }
            _unitOfWork.Subscriptions.Delete(Subscriptions);
            await _unitOfWork.SaveAsync();
            return Ok("Deleted item");
        }
    }
}
