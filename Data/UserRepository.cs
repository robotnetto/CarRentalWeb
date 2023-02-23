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

        public async Task Add(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int? id)
        {
            var userToDelete = await GetById(id);
            if (userToDelete != null)
            {
                context.Users.Remove(userToDelete);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await context.Users.OrderBy(x => x.UserId).ToListAsync();
        }

        public async Task<User> GetById(int? id)
        {
            var tempUser = await context.Users.FirstOrDefaultAsync(x => x.UserId == id);
            return tempUser;
        }

        public async Task Update(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
    }
}
