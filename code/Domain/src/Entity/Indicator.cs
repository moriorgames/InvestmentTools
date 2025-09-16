namespace Domain.Entity;

public sealed class Indicator
{
    public string IndicatorId { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public int Value { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Indicator(string indicatorId, string name, int value, DateTime createdAt)
    {
        IndicatorId = indicatorId;
        Name = name;
        Value = value;
        CreatedAt = DateTime.SpecifyKind(createdAt, DateTimeKind.Utc);
    }

    private Indicator()
    {
    }
}