using RestApiServer.Core.ApiResponses;
using RestApiServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RestApiServer.Controllers
{
    [ApiController]
    [Route("v1/introduction")]
    public class GreetingController : ControllerBase
    {
        [HttpGet("greeting")]
        public async Task<ApiSuccessResponse<string>> GreetUser()
        {
            var res = await GreetingService.GreetUser();
            return ApiSuccessResponses.WithData("Greet user successful", res);
        }
    }
}