//using Microsoft.AspNetCore.Mvc;
//using System.Configuration;
//using WebApplication909.Areas.Admin.Interfaces;
//using WebApplication909.Areas.Admin.Models;
//using WebApplication909.Models;

//namespace WebApplication909.Areas.Admin.Repositories
//{
//    public class InMemoryUsersRepository : IUsersRepository
//    {
//        private readonly List<UserViewModel> _users = [];


//        public void Add(UserViewModel user)
//        {
//            user.Id = Guid.NewGuid();
//            user.CreationDateTime = DateTime.Now;

//            _users.Add(user);
//        }


//        public UserViewModel? TryGetByLogin(string login) =>
//            _users.FirstOrDefault(user => user.Login == login);


//        public List<UserViewModel> GetAll() => _users;


//        public UserViewModel? TryGetById(Guid userId) =>
//            _users.FirstOrDefault(user => user.Id == userId);


//        public void Delete(Guid userId)
//        {
//            var existingUser = TryGetById(userId);

//            if (existingUser != null)
//            {
//                _users.Remove(existingUser);
//            }
//        }

//        public void Update(UserViewModel user)
//        {
//            var existingUser = TryGetById(user.Id);

//            if (existingUser != null)
//            {
//                existingUser.FirstName = user.FirstName;
//                existingUser.LastName = user.LastName;
//                existingUser.Phone = user.Phone;
//            }
//        }


//        public void ChangePassword(string login, string newPassword)
//        {
//            var existingUser = TryGetByLogin(login);

//            if (existingUser != null)
//            {
//                existingUser.Password = newPassword;
//            }
//        }

//        public void ChangeRole(string login, Role? newRole)
//        {
//            var existingUser = TryGetByLogin(login);

//            if (existingUser != null)
//            {
//                existingUser.Role = newRole;
//            }
//        }
//    }
//}