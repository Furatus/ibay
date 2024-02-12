using System.ComponentModel.DataAnnotations;

namespace ibay.Model;

public class LoginModel
{
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }
}