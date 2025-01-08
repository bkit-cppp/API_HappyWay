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
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var product = await _unitOfWork.Products.GetAllAsync();
            return Ok(product);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var productId = await _unitOfWork.Products.GetByIdAsync(id);
            if (productId == null)
            {
                return BadRequest("Id Not Found");
            }
            return Ok(productId);
        }
        [HttpPost]
        public async Task<IActionResult> Addproduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Invalid Data");

            }
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveAsync();
            return Ok(product);
        }
        [HttpPut]
        public async Task<IActionResult> Editproduct(int id, [FromBody] Product product)
        {
            var existingproduct = await _unitOfWork.Products.GetByIdAsync(id);
            if (existingproduct == null)
            {
                return BadRequest("Not Found");
            }
            existingproduct.Price = product.Price;
            existingproduct.ImageUrl = product.ImageUrl;
            existingproduct.Name = product.Name;
            existingproduct.Description = product.Description;
            existingproduct.UpdatedAt = DateTime.Now;
            existingproduct.CreatedAt = DateTime.Now;
            _unitOfWork.Products.Update(existingproduct);
            await _unitOfWork.SaveAsync();
            return Ok(existingproduct);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteproduct(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return BadRequest(" Id Not Found");
            }
            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveAsync();
            return Ok("Deleted item");
        }
    }
}
