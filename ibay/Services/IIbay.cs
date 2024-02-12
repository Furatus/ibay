using ibay.Model;

namespace ibay.Services
{

    public interface IIbay
    {
        public Guid CreateUser(User user);
        public User GetUserById(Guid id);
        public void UpdateUser(Guid id, User user);
        public void DeleteUser(Guid id);
        Guid CreateProduct(Product product);
        Product GetProductById(Guid id);
        public void UpdateProduct(Guid id, Product product);
        public void DeleteProduct(Guid id);
        public IQueryable<Product>? GetProducts(ProductSorting sorting);
        public IQueryable<Product>? SearchProducts(ProductSearch search);
        public Guid AddToCart(Cart cart);
        public void RemoveFromCart(string userId, Guid productId);
        public void EmptyCart(string userId);
        public void PayCart(string userId);

        public List<Product> GetCartItems(string userId);

        public void UpdateUserToSeller(Guid id, User user);
    }
}