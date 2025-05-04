using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using quill.Data;
using quill.Entities;
using quill.Interfaces;

namespace quill.Services;

public class BoardServiceDb : IBoardService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContext;

    public BoardServiceDb(ApplicationDbContext context, UserManager<User> userManager, IHttpContextAccessor httpContext)
    {
        _context = context;
        _userManager = userManager;
        _httpContext = httpContext;
    }

    public async Task<Board?> LoadBoardAsync(int? boardId = null)
    {
        if (boardId == null)
            return null;

        var userId = _userManager.GetUserId(_httpContext.HttpContext.User);

        return await _context.Boards
            .Include(b => b.QLists)
            .ThenInclude(l => l.Items)
            .FirstOrDefaultAsync(b => b.Id == boardId && b.UserId == userId);
    }
    
    public async Task<List<Board>> LoadAllBoardsAsync()
    {
        var userId = _userManager.GetUserId(_httpContext.HttpContext.User);

        return await _context.Boards
            .Include(b => b.QLists)
            .ThenInclude(l => l.Items)
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }

    public async Task<Board> CreateBoardAsync()
    {
        var user = await _userManager.GetUserAsync(_httpContext.HttpContext.User);

        var newBoard = new Board()
        {
            Name = "Untitled Board",
            UserId = user.Id,
            User = user,
            BG_URL = "/images/default-board-bg.jpg"
        };
        
        _context.Boards.Add(newBoard);
        await _context.SaveChangesAsync();

        return newBoard;
    }

    public async Task SaveBoardAsync(Board board)
    {
        var userId = _userManager.GetUserId(_httpContext.HttpContext.User);
        board.UserId = userId;

        if (_context.Boards.Any(b => b.Id == board.Id))
            _context.Boards.Update(board);
        else
            _context.Boards.Add(board);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteBoardAsync(int boardId)
    {
        var userId = _userManager.GetUserId(_httpContext.HttpContext.User);
        var board = await _context.Boards
            .Include(b => b.QLists)
            .ThenInclude(l => l.Items)
            .FirstOrDefaultAsync(b => b.Id == boardId && b.UserId == userId);
            
        if (board != null)
        {
            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();
        }
    }
}
