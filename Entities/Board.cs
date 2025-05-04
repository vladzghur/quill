namespace quill.Entities;

public class Board
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? BG_URL { get; set; }
    
    public string? UserId { get; set; } 
    public User User { get; set; } = null!;
    
    public ICollection<QuillList> QLists { get; } = new List<QuillList>();
}