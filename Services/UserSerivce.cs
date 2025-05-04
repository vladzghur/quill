
using Microsoft.AspNetCore.Identity;
using quill.Data;
using quill.Entities;
using quill.Interfaces;
using Newtonsoft.Json;
using quill.Resolvers;

namespace quill.Services;

public class UserService
{
    private readonly IHttpContextAccessor _httpContext;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IBoardService _boardService;
    private const string BoardsSessionKey = "SavedBoards";

    public UserService(
        IHttpContextAccessor httpContext,
        ApplicationDbContext context,
        UserManager<User> userManager,
        BoardsServiceResolver boardsServiceResolver)
    {
        _httpContext = httpContext;
        _context = context;
        _userManager = userManager;
        _boardService = boardsServiceResolver.GetStorage();
    }

    /// <summary>
    /// Transfers any boards created in session to the user's account
    /// </summary>
    public async Task TransferSessionBoardsToUserAsync(User user)
    {
        // Check if there are any boards in session
        var sessionBoardsJson = _httpContext.HttpContext.Session.GetString(BoardsSessionKey);
        if (string.IsNullOrEmpty(sessionBoardsJson))
            return;

        var sessionBoards = JsonConvert.DeserializeObject<List<Board>>(sessionBoardsJson);
        if (sessionBoards == null || !sessionBoards.Any())
            return;

        // Associate each board with the user and save to DB
        foreach (var board in sessionBoards)
        {
            board.Id = null;
            board.UserId = user.Id;
            board.User = user;
            
            _context.Boards.Add(board);
        }

        await _context.SaveChangesAsync();

        // Clear session boards after transfer
        _httpContext.HttpContext.Session.Remove(BoardsSessionKey);
    }
}