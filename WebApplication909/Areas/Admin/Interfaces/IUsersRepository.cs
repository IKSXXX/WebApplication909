using WebApplication909.Models;

namespace WebApplication909.Areas.Admin.Interfaces
{
    public interface IUsersRepository
    {
        List<User> GetAll();

        User? TryGetByLogin(string userName);

        User? TryGetById(Guid userId);

        void Add(User user);

        void Delete(Guid userId);
        void Update(User user);
    }
}
