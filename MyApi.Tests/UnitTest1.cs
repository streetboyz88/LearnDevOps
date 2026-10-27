namespace MyApi.Tests;

public class SmokeTests
{
    [Fact]
    public void Test1()
    {
        var todo = new Todo(1, "Learn DevOps", false);

        Assert.Equal("Wrong title", todo.Title);
        Assert.False(todo.Done);
    }
}
