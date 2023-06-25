using AuthIdentity.BLL.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthIdentity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserAuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Login")]
        public async Task<ActionResult<string>> Login(string loginId)
        {
            var response = await _mediator.Send(new UserLoginQuery {UserLoginId = loginId }, new());  
            return Ok(response);
        }

        [HttpGet]
        [Route("GetLoggedInUser")]
        [Authorize]
        public async Task<ActionResult<string>> GetLoggedInUser()
        {
            return new ObjectResult("ok");
        }
    }
}
