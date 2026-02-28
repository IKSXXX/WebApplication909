using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Models;

namespace WebApplication909.Areas.Admin.Repositories
{
    public class InMemoryUsersRepository : IUsersRepository
    {
        private List<User> users { get; set; }

        public InMemoryUsersRepository()
        {
            users = new List<User>();
        }

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

        public void Update(User user)
        {
            var existingUser = TryGetById(user.Id);
            if (existingUser != null)
            {
                existingUser.Login = user.Login;
                if (!string.IsNullOrWhiteSpace(user.Password))
                    existingUser.Password = user.Password;
                existingUser.Email = user.Email;
            }
        }


        public List<User> GetAll() => users;

        public User? TryGetById(Guid userId) => 
            users.FirstOrDefault(u => u.Id == userId);

        public User? TryGetByLogin(string userLogin) =>
            users.FirstOrDefault(u => u.Login == userLogin);
    }
}