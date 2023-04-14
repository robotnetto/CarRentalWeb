using Biluthyrning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Biluthyrning.Data
{
    public class CarCategoryRepository : ICarCategory
    {
        private readonly HttpClient client;


        public CarCategoryRepository(IHttpClientFactory httpClientFactory )
        {
            //this.client = client;
            //client.BaseAddress = new Uri("https://localhost:7203/");
            client = httpClientFactory.CreateClient("RemoteApi");
        }

        public async Task CreateAsync(CarCategory carCategory)
        {
            var postResponse = await client.PostAsJsonAsync("/api/CarCategory", carCategory);
            postResponse.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var deleResponse = await client.DeleteAsync($"/api/CarCategory/{id}");
            deleResponse.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<CarCategory>> GetAllAsync()
        {
            var getResponse = await client.GetAsync("/api/CarCategory");
            getResponse.EnsureSuccessStatusCode();
            var result = await getResponse.Content.ReadFromJsonAsync<IEnumerable<CarCategory>>();
            return result;
        }
        public async Task<IEnumerable<CarCategory>> GetSearchedAsync(string search)
        {
            var searchResponse = await client.GetAsync($"/api/CarCategory/search?search={search}");
            searchResponse.EnsureSuccessStatusCode();
            var result = await searchResponse.Content.ReadFromJsonAsync<IEnumerable<CarCategory>>();
            return result;
        }

        public async Task <CarCategory> GetByIdAsync(int id)
        {
            var getIdResponse = await client.GetAsync($"/api/CarCategory/{id}");
            getIdResponse.EnsureSuccessStatusCode();
            var result = await getIdResponse.Content.ReadFromJsonAsync<CarCategory>();
            return result;
        }

        public async Task UpdateAsync(CarCategory carCategory)
        {
            var updateResponse = await client.PutAsJsonAsync($"/api/CarCategory/{carCategory.Id}", carCategory);
            updateResponse.EnsureSuccessStatusCode();
        }
    }
}