using ibay.Model;
namespace ibay.Services;

public interface IIbay
{
    public List<User> GetUsers();
    public User GetUserById(int id);
    public int CreateUser(User user);
    public void UpdateUser(int id, User user);
    public void RemoveUser(int id);

}