using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ibay.Model;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }
    
    [JsonIgnore]
    public string? Role { get; set; }
    
    
}