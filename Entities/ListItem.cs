namespace quill.Entities;

public class ListItem
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public bool IsFinished { get; set; }
    
    public int ListId {get; set;}
    public QuillList QList { get; set; } = null!;
}

