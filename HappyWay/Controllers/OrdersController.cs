using HappyWay.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HappyWay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("user-orders")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value);
            var orders = await _unitOfWork.Orders.GetAllAsync();
            var userOrders = orders.Where(o => o.UserId == userId);

            return Ok(userOrders);
        }
        //[HttpGet]
        //public async Task<IActionResult>
        //[HttpGet]
        //public async Task<IActionResult> GetOrderById()
        //{
            
        //}
    }
}
