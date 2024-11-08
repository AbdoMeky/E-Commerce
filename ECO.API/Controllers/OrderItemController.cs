using ECO.CORE.DTO.OrderItemDTO;
using ECO.CORE.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;
        public OrderItemController(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }
        [HttpGet]
        public IActionResult GetById(int Id)
        {
            var result = _orderItemRepository.GetById(Id);
            if (result is null)
            {
                return BadRequest("No Order Item has this Id");
            }
            return Ok(result);
        }
        [HttpPost("InSpacificOrder")]
        public ActionResult AddOrderItemInSpacificOrder(AddOrderItemDTO item)
        {
            if (ModelState.IsValid)
            {
                var result = _orderItemRepository.AddOrderItemInSpacificOrder(item);
                if (result.Id == 0)
                {
                    return BadRequest(result.Massage);
                }
                string url = Url.Action(nameof(GetById), new { Id = result.Id });
                return Created(url, _orderItemRepository.GetById(result.Id));
            }
            return BadRequest(ModelState);
        }
        [HttpPost("InNotReseavedOrNewOrder")]
        public ActionResult AddOrderItemInNotReseavedOrNewOrder(AddOrderItemInOrderDTO item, string userId)
        {
            if (ModelState.IsValid)
            {
                var result = _orderItemRepository.AddOrderItemInOrderDidnotReseived(item, userId);
                if (result.Id == 0)
                {
                    return BadRequest(result.Massage);
                }
                string url = Url.Action(nameof(GetById), new { Id = result.Id });
                return Created(url, _orderItemRepository.GetById(result.Id));
            }
            return BadRequest(ModelState);
        }
        [HttpPatch]
        public ActionResult Update(EditOrderItemDTO item, int id)
        {
            if(ModelState.IsValid)
            {
                var result=_orderItemRepository.UpdateOrderItem(item, id);
                if(result.Id == 0)
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
            var result=_orderItemRepository.DeleteOrderItem(id);
            if(result.Id == 1)
            {
                return StatusCode(StatusCodes.Status204NoContent, "Deleted");
            }
            return BadRequest(result.Massage);
        }
    }
}
