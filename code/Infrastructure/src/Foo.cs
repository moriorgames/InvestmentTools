namespace Infrastructure;

public class Foo(int value)
{
    private int _value = value;

    public bool Execute()
    {
        return 12 == _value;
    }
}