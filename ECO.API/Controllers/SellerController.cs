using ECO.CORE.DTO.SellerDTO;
using ECO.CORE.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        ISellerRepository _sellerRepository;
        public SellerController(ISellerRepository sellerRepository)
        {
            _sellerRepository = sellerRepository;
        }
        [HttpPatch]
        public ActionResult Edit(EditSellerDTO seller, string id)
        {
            var result=_sellerRepository.Edit(seller, id);
            if (! string.IsNullOrEmpty(result.Id)) 
            {
                return StatusCode(StatusCodes.Status204NoContent, "Edited"); 
            }
            return BadRequest(result.Massage);
        }
        [HttpGet("QuantityOneProductNeeded")]
        public ActionResult Get(string id,int productId)
        {
            var check=_sellerRepository.CheckProductOwnership(id, productId);
            if (check.Id != 1)
            {
                return BadRequest(check.Massage);
            }
            return Ok(_sellerRepository.HowManyQuantityNeededFromProduct(productId));
        }
        [HttpGet("QuantityProductsNeeded")]
        public ActionResult GetAll(string id) 
        { 
            if(!_sellerRepository.Check(id))
            {
                return BadRequest("No Seller has this Id");
            }
            var result=_sellerRepository.HowManyQuantityNeededFromSellerInEachProduct(id);
            if(result is null) 
            {
                return BadRequest("this Seller has No Product");
            }
            return Ok(result);
        }
    }
}
