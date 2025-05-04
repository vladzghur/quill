using quill.Entities;
using quill.Interfaces;
using Newtonsoft.Json;

namespace quill.Services;

public class BoardServiceLocal : IBoardService
{
    private readonly IHttpContextAccessor _httpContext;
    private const string BoardsSessionKey = "SavedBoards";

    public BoardServiceLocal(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
        // Ensure the boards collection exists in session
        if (_httpContext.HttpContext.Session.GetString(BoardsSessionKey) == null)
        {
            _httpContext.HttpContext.Session.SetString(BoardsSessionKey, JsonConvert.SerializeObject(new List<Board>()));
        }
    }

    public Task<Board?> LoadBoardAsync(int? boardId = null)
    {
        if (boardId == null)
            return Task.FromResult<Board?>(null);

        var boards = GetBoardsFromSession();
        var board = boards.FirstOrDefault(b => b.Id == boardId);
        
        return Task.FromResult(board);
    }
    
    public Task<List<Board>> LoadAllBoardsAsync()
    {
        var boards = GetBoardsFromSession();
        return Task.FromResult(boards);
    }
    
    public Task<Board> CreateBoardAsync()
    {
        var boards = GetBoardsFromSession();
        
        // Generate a new ID (max + 1)
        int newId = boards.Any() ? boards.Max(b => b.Id) ?? 0 + 1 : 1;
        
        var newBoard = new Board
        {
            Id = newId,
            Name = "Untitled Board",
            BG_URL = "/images/default-board-bg.jpg"
        };
        
        boards.Add(newBoard);
        SaveBoardsToSession(boards);

        return Task.FromResult(newBoard);
    }

    public Task SaveBoardAsync(Board board)
    {
        var boards = GetBoardsFromSession();
        
        // Find and update or add the board
        var existingBoard = boards.FirstOrDefault(b => b.Id == board.Id);
        if (existingBoard != null)
        {
            // Remove the old board
            boards.Remove(existingBoard);
        }
        
        // Add the updated/new board
        boards.Add(board);
        
        SaveBoardsToSession(boards);
        return Task.CompletedTask;
    }

    public Task DeleteBoardAsync(int boardId)
    {
        var boards = GetBoardsFromSession();
        var boardToDelete = boards.FirstOrDefault(b => b.Id == boardId);
        
        if (boardToDelete != null)
        {
            boards.Remove(boardToDelete);
            SaveBoardsToSession(boards);
        }
        
        return Task.CompletedTask;
    }
    
    // Helper methods
    private List<Board> GetBoardsFromSession()
    {
        var json = _httpContext.HttpContext.Session.GetString(BoardsSessionKey);
        if (string.IsNullOrEmpty(json))
            return new List<Board>();
            
        return JsonConvert.DeserializeObject<List<Board>>(json) ?? new List<Board>();
    }
    
    private void SaveBoardsToSession(List<Board> boards)
    {
        var json = JsonConvert.SerializeObject(boards);
        _httpContext.HttpContext.Session.SetString(BoardsSessionKey, json);
    }
}