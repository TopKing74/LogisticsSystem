using System.Threading.Tasks;

namespace LogisticsSystem.Domain.Contracts;

public interface IContextSeed
{
    Task SeedAsync();
}