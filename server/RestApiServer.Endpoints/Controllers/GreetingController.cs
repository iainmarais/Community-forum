using RestApiServer.Endpoints.ApiResponses;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Endpoints.Services;

namespace RestApiServer.Endpoints.Controllers
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