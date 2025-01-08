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
    public class BlogController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public BlogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
       
        public async Task<IActionResult> GetAll()
        {
            var blog = await _unitOfWork.Blogs.GetAllAsync();
            return Ok(blog);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult>GetById(int id)
        {
            var blogId = await _unitOfWork.Blogs.GetByIdAsync(id);
            if (blogId == null)
            {
                return BadRequest("Id Not Found");
            }
            return Ok(blogId);
        }
        [HttpPost]
        public async Task<IActionResult> AddBlog([FromBody] Blog blog)
        {
            if (blog == null)
            {
                return BadRequest("Invalid Data");

            }
            await _unitOfWork.Blogs.AddAsync(blog);
            await _unitOfWork.SaveAsync();
            return Ok(blog);
        }
        [HttpPut]
        public async Task<IActionResult> EditBlog(int id, [FromBody]Blog blog)
        {
            var existingBlog = await _unitOfWork.Blogs.GetByIdAsync(id);
            if (existingBlog == null)
            {
                return BadRequest("Not Found");
            }
            existingBlog.Title = blog.Title;
            existingBlog.ImageUrl = blog.ImageUrl;
            existingBlog.Content = blog.Content;
            existingBlog.UpdatedAt = DateTime.Now;
            existingBlog.CreatedAt = DateTime.Now;
            _unitOfWork.Blogs.Update(existingBlog);
            await _unitOfWork.SaveAsync();
            return Ok(existingBlog);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _unitOfWork.Blogs.GetByIdAsync(id);
            if (blog == null)
            {
                return BadRequest(" Id Not Found");
            }
            _unitOfWork.Blogs.Delete(blog);
            await _unitOfWork.SaveAsync();
            return Ok("Deleted item");
        }
        
    }
}
