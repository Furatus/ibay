using ibay.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq.Dynamic.Core;

namespace ibay.Services;

public class IbayEfService : IIbay
{
    private IbayContext ibayContext;

    public IbayEfService()
    {
        this.ibayContext = new IbayContext();
        this.ibayContext.Database.EnsureCreated();
        
    }

    public Guid CreateProduct(Product product)
    {
        try
        {
            product.Id = Guid.NewGuid();
            product.AddedTime = product.AddedTime.ToUniversalTime();
            this.ibayContext.Products.Add(product);
            this.ibayContext.SaveChanges();

            return product.Id;
        }
        catch (Exception e)
        {
            throw e.InnerException;
        }
    }

    public Product GetProductById(Guid id)
    {
        return this.ibayContext.Products.FirstOrDefault(x => x.Id == id);
    }

    public void UpdateProduct(Guid id, Product product)
    {
        var tempProduct = this.ibayContext.Products.FirstOrDefault(x => x.Id == id);
        if (tempProduct == null) throw new Exception("The Product does not exists");
        product.Id = id;

        this.ibayContext.Products.Remove(tempProduct);
        this.ibayContext.Products.Add(product);
        this.ibayContext.SaveChanges();
    }

    public void DeleteProduct(Guid id) {
        var product = this.ibayContext.Products.FirstOrDefault(x => x.Id == id);
        if (product == null) throw new Exception("The Product does not exists");

        this.ibayContext.Remove(product);
        this.ibayContext.SaveChanges();
    }
    
    public Guid CreateUser(User user)
    {
            user.Id = Guid.NewGuid();
            user.Role = "user";

            this.ibayContext.Users.Add(user);
            this.ibayContext.SaveChanges();

            return user.Id;
    }

    public User GetUserById(Guid id)
    {
        return this.ibayContext.Users.FirstOrDefault(x => x.Id == id);
    }

    public void UpdateUser(Guid id, User user)
    {
        var tempUser = this.ibayContext.Users.FirstOrDefault(x => x.Id == id);
        if (tempUser == null) throw new Exception("The User does not exists");
        user.Role = tempUser.Role;

        this.ibayContext.Users.Remove(tempUser);
        this.ibayContext.Users.Add(user);
        this.ibayContext.SaveChanges();
    }

    public void DeleteUser(Guid id) {
        var user = this.ibayContext.Users.FirstOrDefault(x => x.Id == id);
        if (user == null) throw new Exception("The User does not exists");

        this.ibayContext.Remove(user);
        this.ibayContext.SaveChanges();
    }

    public IQueryable<Product> GetProducts(ProductSorting sorting)
    {
        if (sorting.SortBy == null || sorting.SortBy == "") return this.ibayContext.Products.Take(sorting.Limit);
        
        List<string> validColumns = new List<string> { "Date", "Name", "Price" };
        
        if (!validColumns.Contains(sorting.SortBy))
        {
            throw new Exception("Invalid column for sorting");
        }
        
        string orderByExpression = $"{sorting.SortBy} {(sorting.Order == 1 ? "ascending" : "descending")}";
        
        var query = this.ibayContext.Products.AsQueryable().OrderBy(orderByExpression).Take(sorting.Limit);

        return query;
        
    }

    public IQueryable<Product> SearchProducts(ProductSearch search)
    {
        return null;
    }

    public void addToCart(Guid userId, Guid productId) {
        
    }
    
}