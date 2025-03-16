using ProvaPub.Infrastructure.Data;

namespace ProvaPub.Application.Services
{
    public class BaseService<T> where T : class
    {
        protected readonly TestDbContext _ctx;
        protected readonly ILogger<BaseService<T>> _logger;

        public BaseService(TestDbContext ctx, ILogger<BaseService<T>> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }
    }
}
