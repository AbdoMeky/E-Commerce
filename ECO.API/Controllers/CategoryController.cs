using ECO.CORE.DTO.CategoryDTO;
using ECO.CORE.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;//it Chacked
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            var result=_categoryRepository.GetAll();
            if(result is null)
            {
                return BadRequest("No Category yet");
            }
            return Ok(result);
        }
        [HttpGet("ById/{id:int}")]
        public ActionResult Get(int id)
        {
            var result = _categoryRepository.GetById(id);
            if(result is null)
            {
                return BadRequest("No Category has this Id");
            }
            return Ok(result);
        }
        [HttpPost]
        public ActionResult Add(ShowCategoryDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {
                var result = _categoryRepository.Add(categoryDTO);
                if (result.Id == 0)
                {
                    return BadRequest(result.Massage);
                }
                string url = Url.Action(nameof(Get), new { id = result.Id });
                return Created(url, _categoryRepository.GetById(result.Id));
            }
            return BadRequest(ModelState);
        }
        [HttpPut]
        public ActionResult Update(ShowCategoryDTO categoryDTO, int id) 
        {
            if (ModelState.IsValid)
            {
                var result = _categoryRepository.Update(categoryDTO, id);
                if (result.Id == 0)
                {
                    return BadRequest(result.Massage);
                }
                return StatusCode(StatusCodes.Status204NoContent, "Updated");
            }
            return BadRequest(ModelState);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var result=_categoryRepository.Delete(id);
            if(result.Id == 0)
            {
                return BadRequest(result.Massage);
            }
            return StatusCode(StatusCodes.Status204NoContent, "Deleted");
        }
    }
}
