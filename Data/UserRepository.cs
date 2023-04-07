using Biluthyrning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Biluthyrning.Data
{
    public class UserRepository : IUser
    {
        private readonly HttpClient client;

        public UserRepository(HttpClient client)
        {
            this.client = client;
            //client.BaseAddress = new Uri("https://localhost:7203/");
        }
        public async Task AddAsync(User user)
        {
            var postResponse = await client.PostAsJsonAsync("/api/User", user);
            postResponse.EnsureSuccessStatusCode();
          
        }
        public async Task DeleteAsync(int? id)
        {
            var deleteResponse = await client.DeleteAsync($"/api/User/{id}");
            deleteResponse.EnsureSuccessStatusCode();
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var getResponse = await client.GetAsync("/api/User");
            getResponse.EnsureSuccessStatusCode();
            var result = await getResponse.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<User>>(result);
            return users;
        }
        public async Task<IEnumerable<User>> GetSearchedAsync(string search)
        {
            var serachResponse = await client.GetAsync($"/api/User/search?search={search}");
            serachResponse.EnsureSuccessStatusCode();
            var result = await serachResponse.Content.ReadAsStringAsync();
            var searchResult = JsonConvert.DeserializeObject<IEnumerable<User>>(result);
            return searchResult;
        }
        public async Task<User> GetByIdAsync(int? id)
        {
            var getResponse = await client.GetAsync($"/api/User/{id}");
            getResponse.EnsureSuccessStatusCode();
            var result = await getResponse.Content.ReadAsStringAsync();
            var idResult = JsonConvert.DeserializeObject<User>(result);
            return idResult;
            
        }
        public async Task UpdateAsync(User user)
        {
            var userId = user.UserId;
            var updateResponse = await client.PutAsJsonAsync($"/api/User/{userId}", user);
            updateResponse.EnsureSuccessStatusCode();

        }
    }
}
