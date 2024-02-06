using System.ComponentModel.DataAnnotations;

namespace ibay.Model;

public class ItemId
{
    [Key]
    [Required]
    public Guid Id { get; set; }
}