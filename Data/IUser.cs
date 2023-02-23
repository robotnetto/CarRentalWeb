using Biluthyrning.Models;

namespace Biluthyrning.Data
{
    public interface IUser
    {
        public Task<User> GetById(int? id);
        public Task<IEnumerable<User>> GetAll();
        public Task Add(User user);
        public Task Update(User user);
        public Task Delete(int? id);

    }
}
