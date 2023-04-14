using Biluthyrning.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net.Http;

namespace Biluthyrning.Data
{
    public class BookingRepository : IBooking
    {
        
        private readonly HttpClient client;

        public BookingRepository(IHttpClientFactory httpClientFactory)
        {
            client = httpClientFactory.CreateClient("RemoteApi");
        }
        public async Task AddAsync(Booking booking)
        {
            var postResponse = await client.PostAsJsonAsync("/api/Booking", booking);
            postResponse.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var deleteResponse = await client.DeleteAsync($"/api/Booking/{id}");
            deleteResponse.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            var getResponse = await client.GetAsync("/api/Booking");
            getResponse.EnsureSuccessStatusCode();
            var result = await getResponse.Content.ReadFromJsonAsync<IEnumerable<Booking>>();
            return result;

        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            var getResponse = await client.GetAsync($"/api/Booking/{id}");
            getResponse.EnsureSuccessStatusCode();
            var result = await getResponse.Content.ReadFromJsonAsync<Booking>();
            return result;
        }

        public async Task UpdateAsync(Booking booking)
        {
            var putResponse = await client.PutAsJsonAsync($"/api/Booking/{booking.Id}", booking);
            putResponse.EnsureSuccessStatusCode();
            
        }
    }
}
