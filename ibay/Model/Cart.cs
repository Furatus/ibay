using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ibay.Model;

public class Cart
{
    [Key] 
    public Guid id { get; set; }
    
    [JsonIgnore]
    public Guid UserId { get; set; }
    
    [Required]
    public Guid ProductId { get; set; }
}