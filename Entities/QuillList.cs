namespace quill.Entities;

public class QuillList
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Color { get; set; }
    
    public ICollection<ListItem> Items { get; } = new List<ListItem>();
    
    public int BoardId { get; set; }
    public Board Board { get; set; } = null!;
}