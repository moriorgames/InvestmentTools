using Xunit;

namespace Infrastructure.Test;

public class FooTest
{
    [Fact]
    public void ShouldReturnTrue()
    {
        var sut = new Foo();

        var actual = sut.Execute();

        Assert.True(actual);
    }
}