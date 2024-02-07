namespace ibay.Model;

public class ProductSorting
{
    public int Limit { get; set; } = 10;
    public string? SortBy { get; set; }
    public sbyte Order { get; set; } = 1;
}