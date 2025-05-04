using Microsoft.AspNetCore.Identity;

namespace quill.Entities;

public class User : IdentityUser
{
   public DateTime Birthdate { get; set; }
   public ICollection<Board> Boards { get; set; }
}