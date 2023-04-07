using Biluthyrning.Models;

namespace Biluthyrning.Data
{
    public interface IUser
    {
        public Task<User> GetByIdAsync(int? id);
        public Task<IEnumerable<User>> GetAllAsync();
        public Task <IEnumerable<User>> GetSearchedAsync(string search);
        public Task AddAsync(User user);
        public Task UpdateAsync(User user);
        public Task DeleteAsync(int? id);
    }
}
