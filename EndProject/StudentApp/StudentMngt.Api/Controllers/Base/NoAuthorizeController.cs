using Microsoft.AspNetCore.Mvc;

namespace StudentMngt.Api.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class NoAuthorizeController : ControllerBase
    {

    }
}
