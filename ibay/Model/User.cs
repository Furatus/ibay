﻿using System.ComponentModel.DataAnnotations;

namespace ibay.Model;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    
    
}