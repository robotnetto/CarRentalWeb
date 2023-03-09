using Biluthyrning.Models;
using Microsoft.EntityFrameworkCore;

namespace Biluthyrning.Data
{
    public class UserRepository : IUser
    {
        private readonly CarRentalContext context;

        public UserRepository(CarRentalContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int? id)
        {
            var userToDelete = await GetByIdAsync(id);
            if (userToDelete != null)
            {
                context.Users.Remove(userToDelete);
                await context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await context.Users.OrderBy(x => x.UserId).ToListAsync();
        }
        public async Task<IEnumerable<User>> GetSearchedAsync(string search)
        {
            return await context.Users.Where(x=>x.UserName.Contains(search))
                .OrderBy(x => x.UserId).ToListAsync();
        }
        public async Task<User> GetByIdAsync(int? id)
        {
            var tempUser = await context.Users.FirstOrDefaultAsync(x => x.UserId == id);
            return tempUser;
        }
        public async Task UpdateAsync(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
    }
}
