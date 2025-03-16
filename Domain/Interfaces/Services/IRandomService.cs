namespace ProvaPub.Domain.Interfaces.Services
{
    public interface IRandomService
    {
        Task<int> GetRandom();
    }
}
