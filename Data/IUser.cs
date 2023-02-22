using Biluthyrning.Models;

namespace Biluthyrning.Data
{
    public interface IUser
    {
        public User GetById(int? id);
        public IEnumerable<User> GetAll();
        public void Add(User user);
        public void Update(User user);
        public void Delete(int? id);

    }
}
