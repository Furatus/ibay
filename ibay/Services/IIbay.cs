using ibay.Model;

namespace ibay.Services
{

    public interface IIbay
    {
        public Guid CreateUser(User user);
        public List<User> GetUsers(int limit);
        public User GetUserById(Guid id);
        public void UpdateUser(Guid id, User user);
        public void DeleteUser(Guid id);
        Guid CreateProduct(Product product);
        Product GetProductById(Guid id);
        public void UpdateProduct(Guid id, Product product);
        public void DeleteProduct(Guid id);
    }
}