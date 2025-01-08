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
    public class KnowledgeBaseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public KnowledgeBaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var KnowledgeBase = await _unitOfWork.KnowledgeBases.GetAllAsync();
            return Ok(KnowledgeBase);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var KnowledgeBaseId = await _unitOfWork.KnowledgeBases.GetByIdAsync(id);
            if (KnowledgeBaseId == null)
            {
                return BadRequest("Id Not Found");
            }
            return Ok(KnowledgeBaseId);
        }
        [HttpPost]
        public async Task<IActionResult> AddKnowledgeBase([FromBody] KnowledgeBase KnowledgeBase)
        {
            if (KnowledgeBase == null)
            {
                return BadRequest("Invalid Data");

            }
            await _unitOfWork.KnowledgeBases.AddAsync(KnowledgeBase);
            await _unitOfWork.SaveAsync();
            return Ok(KnowledgeBase);
        }
        [HttpPut]
        public async Task<IActionResult> EditKnowledgeBase(int id, [FromBody] KnowledgeBase KnowledgeBase)
        {
            var existingKnowledgeBase = await _unitOfWork.KnowledgeBases.GetByIdAsync(id);
            if (existingKnowledgeBase == null)
            {
                return BadRequest("Not Found");
            }
            existingKnowledgeBase.Title = KnowledgeBase.Title;
            existingKnowledgeBase.Content = KnowledgeBase.Content;
     
            existingKnowledgeBase.UpdatedAt = DateTime.Now;
            existingKnowledgeBase.CreatedAt = DateTime.Now;
            _unitOfWork.KnowledgeBases.Update(existingKnowledgeBase);
            await _unitOfWork.SaveAsync();
            return Ok(existingKnowledgeBase);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKnowledgeBase(int id)
        {
            var KnowledgeBase = await _unitOfWork.KnowledgeBases.GetByIdAsync(id);
            if (KnowledgeBase == null)
            {
                return BadRequest(" Id Not Found");
            }
            _unitOfWork.KnowledgeBases.Delete(KnowledgeBase);
            await _unitOfWork.SaveAsync();
            return Ok("Deleted item");
        }
    }
}
