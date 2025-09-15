using Domain.Entity;
using Xunit;

namespace Domain.Test.Entity;

public class IndicatorTest
{
    [Fact]
    public void ShouldCreateIndicatorEntity()
    {
        const string id = "CPI_US_2025_08";
        const string name = "US CPI August 2025";
        const int value = 320;
        var nowUtc = DateTime.UtcNow;

        var actual = new Indicator(id, name, value, nowUtc);

        Assert.Equal(id, actual.IndicatorId);
        Assert.Equal(name, actual.Name);
        Assert.Equal(value, actual.Value);
        Assert.Equal(DateTimeKind.Utc, actual.CreatedAt.Kind);
    }
}