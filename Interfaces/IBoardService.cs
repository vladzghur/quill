using quill.Entities;

namespace quill.Interfaces;

public interface IBoardService
{
    Task<Board?> LoadBoardAsync(int? boardId = null);
    Task<List<Board>> LoadAllBoardsAsync();
    Task SaveBoardAsync(Board board);
    Task<Board> CreateBoardAsync();
    Task DeleteBoardAsync(int boardId);
}