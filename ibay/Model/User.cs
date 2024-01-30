using System.ComponentModel.DataAnnotations;

namespace ibay.Model;

public class User
{
    [Key]
    public int Id { get; set; }
    
    
    [Required]
    public string Name { get; set; }
}