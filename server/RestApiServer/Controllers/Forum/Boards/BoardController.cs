using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Common.Services;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Dto.Forum;
using RestApiServer.Services.Forum.Boards;

namespace RestApiServer.Controllers.Forum.Boards
{
    [ApiController]
    [Route("v1/forum")]
    [Authorize]
    public class BoardController : ControllerBase
    {
        [HttpGet("boards")]
        public async Task<ApiSuccessResponse<List<BoardBasicInfo>>> GetBoards()
        {
            var user = AuthService.GetForumUserContext(User);
            var res = await BoardService.GetForumBoardsAsync();
            return ApiSuccessResponses.WithData("Get boards successful", res);
        }

        [HttpPost("boards/create")]
        public async Task<ApiSuccessResponse<List<BoardBasicInfo>>> CreateBoard(CreateBoardRequest request)
        {
            var user = AuthService.GetForumUserContext(User);
            var res = await BoardService.CreateForumBoardAsync(user.UserId, request);
            return ApiSuccessResponses.WithData("Create board successful", res);
        }

        [HttpGet("boards/{boardId}/fullinfo")]
        public async Task<ApiSuccessResponse<BoardFullInfo>> GetSelectedBoard(string boardId)
        {
            var user = AuthService.GetForumUserContext(User);
            var res = await BoardService.GetSelectedBoardAsync(boardId);
            return ApiSuccessResponses.WithData("Get selected board successful", res);
        }

        [HttpGet("boards/{boardId}/topics")]
        public async Task<ApiSuccessResponse<PaginatedData<List<TopicBasicInfo>, TopicSummary>>> GetTopicsForBoard(string boardId, [FromQuery] int pageNumber, [FromQuery] int rowsPerPage, [FromQuery] string? searchTerm)
        {
            var user = AuthService.GetForumUserContext(User);
            var res = await BoardService.GetTopicsForBoardAsync(boardId, pageNumber, rowsPerPage, searchTerm);
            return ApiSuccessResponses.WithData("Get selected board successful", res);
        }        
        
    }
}