namespace Exceptions;

public class BadMarkdownSyntaxException : Exception
{
    public static void ThrowByPredicate(Func<bool> predicate, string msg)
    {
        if (predicate())
        {
            throw new BadMarkdownSyntaxException(msg);
        }
    }

    public BadMarkdownSyntaxException()
    {
    }

    public BadMarkdownSyntaxException(string message) : base(message)
    {
    }

    public BadMarkdownSyntaxException(string message, Exception innerException) : base(message, innerException)
    {
    }
}