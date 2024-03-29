﻿using ibay.Model;
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
            product.AddedTime = DateTime.Now.ToUniversalTime();//product.AddedTime.ToUniversalTime();
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
        var existingProduct = this.ibayContext.Products.FirstOrDefault(x => x.Id == id);
        if (existingProduct == null) throw new Exception("The Product does not exists");

        if (product.Name != null) existingProduct.Name = product.Name;
        if (product.Image != null) existingProduct.Image = product.Image;
        if (product.Price != null) existingProduct.Price = product.Price;
        if (product.Available != null) existingProduct.Available = product.Available;

        this.ibayContext.Products.Update(existingProduct);
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
        if (GetUserByName(user.Username) != null) throw new Exception("The User Already Exists");
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
    
    public User GetUserByName(string username)
    {
        return this.ibayContext.Users.FirstOrDefault(u => u.Username == username);
    }

    public void UpdateUser(Guid id, User user)
    {
        var tempUser = this.ibayContext.Users.FirstOrDefault(x => x.Id == id);
        if (tempUser == null) throw new Exception("The User does not exists");

        if (user.Username != null)
        {
            if (GetUserByName(user.Username) != null) throw new Exception("The username already exists");
            tempUser.Username = user.Username;
        }

        if (user.Password != null) tempUser.Password = user.Password;
        if (user.Email != null) tempUser.Email = user.Email;

        this.ibayContext.Users.Update(tempUser);
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
        if (search.Available == null && search.Name == null && search.Price == null)
        {
            return this.ibayContext.Products.Take(search.Limit);
        }

        IQueryable<Product> query = this.ibayContext.Products;
        
        if (search.Available.HasValue) query = query.Where(p => p.Available == search.Available.Value);

        if (!string.IsNullOrEmpty(search.Name)) query = query.Where(p => p.Name.Contains(search.Name));

        if (search.Price.HasValue) query = query.Where(p => p.Price == search.Price.Value);

        return query;
    }

    public Guid AddToCart(Cart cart)
    {
        cart.id = new Guid();

        this.ibayContext.Carts.Add(cart);
        this.ibayContext.SaveChanges();

        return cart.id;
    }

    public void RemoveFromCart(string userId, Guid productId)
    {
        var cartItem = this.ibayContext.Carts.SingleOrDefault(c => c.UserId == new Guid(userId) && c.ProductId == productId);
        if (cartItem == null) throw new Exception("The product is not in your cart");

        this.ibayContext.Remove(cartItem);
        this.ibayContext.SaveChanges();
    }

    public void EmptyCart(string userId)
    {
        var cartItems = this.ibayContext.Carts.Where(c => c.UserId == new Guid(userId)).ToList();

        if (cartItems.Any())
        {
            this.ibayContext.Carts.RemoveRange(cartItems);
            this.ibayContext.SaveChanges();
        }
    }
    
    public void PayCart(string userId)
    {
        var cartItems = this.ibayContext.Carts.Where(c => c.UserId == new Guid(userId)).ToList();

        if (cartItems.Any())
        {
            this.ibayContext.Carts.RemoveRange(cartItems);
            this.ibayContext.SaveChanges();
        }
    }
    
    public List<Product> GetCartItems(string userId)
    {
        var cartItems = this.ibayContext.Carts
            .Where(c => c.UserId == new Guid(userId))
            .Select(c => c.ProductId)
            .ToList();

        var productsInCart = this.ibayContext.Products
            .Where(p => cartItems.Contains(p.Id))
            .ToList();

        return productsInCart;
    }
    
    public void UpdateUserToSeller(Guid id, User user)
    {
        var tempUser = this.ibayContext.Users.FirstOrDefault(x => x.Id == id);
        if (tempUser == null) throw new Exception("The User does not exists");
        user.Id = id;

        this.ibayContext.Users.Update(user);
        this.ibayContext.SaveChanges();
    }
}