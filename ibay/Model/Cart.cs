using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ibay.Model;

public class Cart
{
    [Key] 
    public Guid id { get; set; }
    
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}