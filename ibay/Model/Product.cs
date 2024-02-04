﻿using System.ComponentModel.DataAnnotations;

namespace ibay.Model;

public class Product
{
    [Key]
    public Guid Id { get; set; }
    
    public string? Name { get; set; }
    public string? Image { get; set; }
    public int Price { get; set; }
    public bool Available { get; set; }
    public DateTime AddedTime { get; set; }
}