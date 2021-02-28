// using Machete.Web.Helpers.Api;
// using Microsoft.AspNetCore.Mvc;

// namespace Machete.Web.Controllers.Api.Abstracts
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public abstract class MacheteApiController : ControllerBase
//     {
//         protected string UserSubject => User?.FindFirst(CAType.nameidentifier)?.Value;
//         protected string UserEmail => User?.FindFirst(CAType.email)?.Value;

//         protected void Initialize(ControllerContext controllerContext)
//         {
//             ControllerContext = controllerContext;
//         }
//     }
// }
