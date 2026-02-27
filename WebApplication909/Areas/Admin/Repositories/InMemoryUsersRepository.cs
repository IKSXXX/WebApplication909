using System.Configuration;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Areas.Admin.Models;

namespace WebApplication909.Areas.Admin.Repositories
{
    public class InMemoryUsersRepository : IUsersRepository
    {
        public List<User> users { get; set; }

        public void Add(User user)
        {
            user.Id = Guid.NewGuid();
            users.Add(user);
        }

        public void Delete(Guid userId)
        {
            var user = TryGetById(userId);

            if (user != null)
            {
                users.Remove(user);
            }
        }

        public List<User> GetAll() => users;

        public User? TryGetById(Guid userId) => 
            users.FirstOrDefault(u => u.Id == userId);

        public User? TryGetByLogin(string userLogin) =>
            users.FirstOrDefault(u => u.Login == userLogin);
    }
}
