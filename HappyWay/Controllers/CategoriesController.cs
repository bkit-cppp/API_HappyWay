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
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Categories = await _unitOfWork.Categories.GetAllAsync();
            return Ok(Categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var CategoriesId = await _unitOfWork.Categories.GetByIdAsync(id);
            if (CategoriesId == null)
            {
                return BadRequest("Id Not Found");
            }
            return Ok(CategoriesId);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategories([FromBody] Category Categories)
        {
            if (Categories == null)
            {
                return BadRequest("Invalid Data");

            }
            await _unitOfWork.Categories.AddAsync(Categories);
            await _unitOfWork.SaveAsync();
            return Ok(Categories);
        }
        [HttpPut]
        public async Task<IActionResult> EditCategories(int id, [FromBody] Category Categories)
        {
            var existingCategories = await _unitOfWork.Categories.GetByIdAsync(id);
            if (existingCategories == null)
            {
                return BadRequest("Not Found");
            }
            existingCategories.Description = Categories.Description;
            existingCategories.Name = Categories.Name;
            existingCategories.UpdatedAt = DateTime.Now;
            existingCategories.CreatedAt = DateTime.Now;
            _unitOfWork.Categories.Update(existingCategories);
            await _unitOfWork.SaveAsync();
            return Ok(existingCategories);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategories(int id)
        {
            var Categories = await _unitOfWork.Categories.GetByIdAsync(id);
            if (Categories == null)
            {
                return BadRequest(" Id Not Found");
            }
            _unitOfWork.Categories.Delete(Categories);
            await _unitOfWork.SaveAsync();
            return Ok("Deleted item");
        }
    }
}
