using Microsoft.EntityFrameworkCore;
using ProvaPub.Domain.Interfaces.Services;
using ProvaPub.Domain.Models;
using ProvaPub.Infrastructure.Data;

namespace ProvaPub.Application.Services
{
    public class RandomService : BaseService<RandomNumber>, IRandomService
    {
        private static readonly Random _random = new Random();

        public RandomService(TestDbContext ctx, ILogger<RandomService> logger) : base(ctx, logger) { }

        public async Task<int> GetRandom()
		{
            try
            {
                _logger.LogInformation("Starting random number");
             
                int number;
                bool numberExists;

                do
                {
                    number = _random.Next(100);
                    numberExists = await _ctx.Numbers.AnyAsync(n => n.Number == number);
                }
                while (numberExists);

                _ctx.Numbers.Add(new RandomNumber { Number = number });
                await _ctx.SaveChangesAsync();

                _logger.LogInformation("Random number {RandomNumber} saved successfully", number);

                return number;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error GetRandom");
                return new int();
            }
        }
	}
}
