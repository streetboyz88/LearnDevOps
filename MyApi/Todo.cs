namespace MyApi
{
    // A "record" is a compact C# type for holding data. This one line gives
    // you a class with Id, Title, and Done properties, a constructor,
    // and value-based equality. It's also what gets converted to and from
    // JSON in the endpoints above.
    public record Todo(int Id, string Title, bool Done);
}
