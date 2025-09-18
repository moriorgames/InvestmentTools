using Domain.Entity;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class IndicatorRepository(EntityFrameworkContext context) : IIndicatorRepository
{
    private readonly EntityFrameworkContext _context = context;

    public async Task<Indicator?> GetByIdAsync(string indicatorId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(indicatorId);

        return await _context.Indicators
            .AsNoTracking()
            .FirstOrDefaultAsync(indicator => indicator.IndicatorId == indicatorId, cancellationToken);
    }

    public async Task AddAsync(Indicator indicator, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(indicator);

        await _context.Indicators.AddAsync(indicator, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
