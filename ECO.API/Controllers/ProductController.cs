using ECO.CORE.DTO.ProductDTO;
using ECO.CORE.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;//it Chacked
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet("NoCategory")]
        public ActionResult GetProductNoCategory(int page ,int size)
        {
            var result=_productRepository.GetProductsInPaginationsNoCategory(page, size);
            if(result.Count==0)
            {
                return BadRequest("there is no products or page or size is wrong");
            }
            return Ok(result);
        }
        [HttpGet("WithCategory")]
        public ActionResult GetProductWithCategory(int page, int size,int categoryId)
        {
            var result = _productRepository.GetProductsInPaginationsWithCategory(page, size, categoryId);
            if (result.Count == 0)
            {
                return BadRequest("there is no products or page or size is wrong or Category");
            }
            return Ok(result);
        }
        [HttpGet("ById")]
        public ActionResult GetProduct(int id)
        {
            var result = _productRepository.GetProductById(id);
            if (result is null)
            {
                return BadRequest("No Product has this Id");
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> Add(AddProductDTO product)
        {
            if (ModelState.IsValid)
            {
                var result=await _productRepository.Add(product);
                if(result.Id==0)
                {
                    return BadRequest(result.Massage);
                }
                string url=Url.Action(nameof(GetProduct),new {id=result.Id});
                return Created(url, _productRepository.GetProductById(result.Id));
            }
            return BadRequest(ModelState);
        }
        [HttpPatch]
        public async Task<ActionResult> Update(EditProductDTO product, int id)
        {
            if (ModelState.IsValid)
            {
                var result =await _productRepository.Update(product,id);
                if (result.Id == 0)
                {
                    return BadRequest(result.Id);
                }
                return StatusCode(StatusCodes.Status204NoContent,"Updated");
            }
            return BadRequest(ModelState);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var result=_productRepository.Delete(id);
            if(result.Id == 0)
            {
                return BadRequest(result.Massage);
            }
            return StatusCode(StatusCodes.Status204NoContent, "Deleted");
        }
    }
}
