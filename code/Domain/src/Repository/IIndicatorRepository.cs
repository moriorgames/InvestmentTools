using Domain.Entity;

namespace Domain.Repository;

public interface IIndicatorRepository
{
    Task<Indicator?> GetByIdAsync(string indicatorId, CancellationToken cancellationToken);

    Task AddAsync(Indicator indicator, CancellationToken cancellationToken);
}
