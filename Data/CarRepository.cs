using Biluthyrning.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace Biluthyrning.Data
{
    public class CarRepository : ICar
    {
        private readonly HttpClient client;

        public CarRepository( IHttpClientFactory httpClientFactory)
        {
            client = httpClientFactory.CreateClient("RemoteApi");
        }
       public async Task CreateAsync(Car car)
        {
            var postResponse = await client.PostAsJsonAsync($"/api/Car/", car);
            postResponse.EnsureSuccessStatusCode();
        }

       public async Task DeleteAsync(int id)
        {
            
            var deleteResponse = await client.DeleteAsync($"/api/Car/{id}");
            deleteResponse.EnsureSuccessStatusCode();
        }

        public async Task <IEnumerable<Car>> GetAllAsync()
        {
            var getResponse = await client.GetAsync("/api/Car/");
            getResponse.EnsureSuccessStatusCode();
            var result = await getResponse.Content.ReadFromJsonAsync<IEnumerable<Car>>();
            return result;
        }

        public async Task <Car> GetByIdAsync(int id)
        {
            var getResponse = await client.GetAsync($"/api/Car/{id}");
            getResponse.EnsureSuccessStatusCode();
            var result = await getResponse.Content.ReadFromJsonAsync<Car>();
            return result;
        }

       public async Task UpdateAsync(Car car)
        {
            var updateResponse = await client.PutAsJsonAsync($"/api/Car/{car.CarId}", car);
            updateResponse.EnsureSuccessStatusCode();
            
        }
        public async Task<IEnumerable<Car>> SearchCarAsync(string search)
        {
            var searchResponse = await client.GetAsync($"/api/Car/search?search={search}");
            searchResponse.EnsureSuccessStatusCode();
            var result = await searchResponse.Content.ReadFromJsonAsync<IEnumerable<Car>>();
            return result;
        }

    }
}
