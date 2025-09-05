using Xunit;

namespace Infrastructure.Test;

public class FooTest
{
    [Fact]
    public void ShouldReturnTrue()
    {
        var sut = new Foo(12);

        var actual = sut.Execute();

        Assert.True(actual);
    }
}