using Biluthyrning.Models;

namespace Biluthyrning.Data
{
    public class UserRepository : IUser
    {
        private readonly CarRentalContext context;

        public UserRepository(CarRentalContext context)
        {
            this.context = context;
        }

        public void Add(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void Delete(int? id)
        {
            var userToDelete = GetById(id);
            if (userToDelete != null)
            {
                context.Users.Remove(userToDelete);
                context.SaveChanges();
            }
        }

        public IEnumerable<User> GetAll()
        {
            return context.Users.OrderBy(x => x.UserId);
        }

        public User GetById(int? id)
        {
            return context.Users.First(x => x.UserId == id);
        }

        public void Update(User user)
        {
            context.Users.Update(user);
            context.SaveChanges();
        }
    }
}
