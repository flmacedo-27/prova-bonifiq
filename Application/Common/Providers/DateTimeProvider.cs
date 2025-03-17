using ProvaPub.Application.Common.Interfaces;

namespace ProvaPub.Application.Common.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
