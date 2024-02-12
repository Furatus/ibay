using System.ComponentModel.DataAnnotations;

namespace ibay.Model;

public class Username
{
    [Key]
    [Required]
    public string Name { get; init; }
}