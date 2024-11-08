using ECO.CORE.DTO.UserDTO;
using ECO.CORE.Interface;
using ECO.EF.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly StripeSettings _stripeSettings;

        public UserController(IUserRepository userRepository, StripeSettings stripeSettings)
        {
            _userRepository = userRepository;
            _stripeSettings = stripeSettings;
        }
        [HttpPatch]
        public ActionResult Edit(EditUserDTO user, string id)
        {
            var result = _userRepository.Edit(user, id);
            if (string.IsNullOrEmpty(result.Id))
            {
                return BadRequest(result.Massage);
            }
            return StatusCode(StatusCodes.Status204NoContent, "Updated");
        }
        [HttpGet]
        public ActionResult Get(string id)
        {
            var check=_userRepository.Check(id);
            if (!check)
            {
                return BadRequest("No User has this Id");
            }
            var result=_userRepository.OrderUserDo(id);
            if(result is null)
            {
                return BadRequest("The User Do not has any Order");
            }
            return Ok(result);
        }
    }
}
