using ECO.CORE.DTO.OrderDTO;
using ECO.CORE.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        [HttpGet("ById")]
        public ActionResult GetById(int id)
        {
            var result=_orderRepository.Get(id);
            if(result == null)
            {
                return BadRequest("No Order has this Id"); ;
            }
            return Ok(result);
        }
        [HttpGet("OrdersForUser")]
        public ActionResult GetOrdersForUser(string id)
        {
            var result = _orderRepository.GetOrdersForUser(id);
            if (result == null)
            {
                return BadRequest("No User has this Id or No Order this user take");
            }
            return Ok(result);
        }
        [HttpPost]
        public ActionResult Add(AddOrderDTO order)
        {
            if (ModelState.IsValid)
            {
                var result = _orderRepository.Add(order);
                if (result.Id == 0)
                {
                    return BadRequest(result.Massage);
                }
                string url = Url.Action(nameof(GetById), new { id = result.Id });
                return Created(url, _orderRepository.Get(result.Id));
            }
            return BadRequest(ModelState);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var result=_orderRepository.Delete(id);
            if(result.Id == 0)
            {
                return BadRequest(result.Massage);
            }
            return StatusCode(StatusCodes.Status204NoContent, "Deleted");
        }
    }
}
