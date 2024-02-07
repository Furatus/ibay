namespace ibay.Model;

public class ProductSearch
{
    public int Limit { get; set; } = 10;
    public string? Name { get; set; }
    public int? Price { get; set; }
    public bool? Available { get; set; }
    
}