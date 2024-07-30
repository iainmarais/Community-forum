using Microsoft.AspNetCore.Mvc;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Services.Boards;
using RestApiServer.Utils;

namespace RestApiServer.Controllers.Boards
{
    [ApiController]
    [Route("v1/forum")]
    public class BoardController : ControllerBase
    {
        [HttpGet("boards")]
        public async Task<ApiSuccessResponse<List<BoardBasicInfo>>> GetBoards()
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await BoardService.GetForumBoardsAsync();
            return ApiSuccessResponses.WithData("Get boards successful", res);
        }

        [HttpPost("boards/create")]
        public async Task<ApiSuccessResponse<List<BoardBasicInfo>>> CreateBoard(CreateBoardRequest request)
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await BoardService.CreateForumBoardAsync(user.UserId, request);
            return ApiSuccessResponses.WithData("Create board successful", res);
        }

        [HttpGet("boards/{boardId}/fullinfo")]
        public async Task<ApiSuccessResponse<BoardFullInfo>> GetSelectedBoard(string boardId)
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await BoardService.GetSelectedBoardAsync(boardId);
            return ApiSuccessResponses.WithData("Get selected board successful", res);
        }
        
    }
}