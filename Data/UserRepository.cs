using Biluthyrning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Net;

namespace Biluthyrning.Data
{
    public class UserRepository : IUser
    {
        private readonly HttpClient client;

        public UserRepository(IHttpClientFactory httpClientFactory )
        {
            client = httpClientFactory.CreateClient("RemoteApi");
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
            var result = await getResponse.Content.ReadFromJsonAsync<IEnumerable<User>>();
            return result;
        }
        public async Task<IEnumerable<User>> GetSearchedAsync(string search)
        {
            var searchResponse = await client.GetAsync($"/api/User/search?search={search}");
            searchResponse.EnsureSuccessStatusCode();
            var result = await searchResponse.Content.ReadFromJsonAsync<IEnumerable<User>>();
            return result;
        }
        public async Task<User> GetByIdAsync(int? id)
        {
            var getResponse = await client.GetAsync($"/api/User/{id}");
            getResponse.EnsureSuccessStatusCode();
            var result = await getResponse.Content.ReadFromJsonAsync<User>();
            return result;
            
        }
        public async Task UpdateAsync(User user)
        {
            var updateResponse = await client.PutAsJsonAsync($"/api/User/{user.UserId}", user);
            updateResponse.EnsureSuccessStatusCode();

        }
    }
}
