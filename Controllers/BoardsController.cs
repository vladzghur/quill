using Microsoft.AspNetCore.Mvc;
using quill.Data;
using quill.Resolvers;
using quill.Entities;

namespace quill.Controllers;

public class BoardsController : Controller
{
    private readonly BoardsServiceResolver _resolver;

    public BoardsController(BoardsServiceResolver resolver)
    {
        _resolver = resolver;
    }
    
    public IActionResult Index()
    {
        var storage = _resolver.GetStorage();
        return View(storage.LoadAllBoardsAsync().Result);
    }

    public async Task<IActionResult> Create()
    {
        var storage = _resolver.GetStorage();
        Board newBoard = await storage.CreateBoardAsync();
        return RedirectToAction("Edit", new { id = newBoard.Id });
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var storage = _resolver.GetStorage();
        Board? board = await storage.LoadBoardAsync(id);
        
        if (board == null)
        {
            return NotFound();
        }
        
        return View(board);
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveBoard(Board board)
    {
        if (ModelState.IsValid)
        {
            var storage = _resolver.GetStorage();
            await storage.SaveBoardAsync(board);
            return RedirectToAction("Index");
        }
        
        return View("Edit", board);
    }
    
    public async Task<IActionResult> Details(int id)
    {
        var storage = _resolver.GetStorage();
        Board? board = await storage.LoadBoardAsync(id);
        
        if (board == null)
        {
            return NotFound();
        }
        
        return View(board);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var storage = _resolver.GetStorage();
        await storage.DeleteBoardAsync(id);
        
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int id, bool confirm = false)
    {
        if (confirm)
        {
            var storage = _resolver.GetStorage();
            Board? board = await storage.LoadBoardAsync(id);
            
            if (board == null)
            {
                return NotFound();
            }
            
            return View(board);
        }
        else
        {
            // If not confirming, redirect to confirmation
            return RedirectToAction("Delete", new { id, confirm = true });
        }
    }
}