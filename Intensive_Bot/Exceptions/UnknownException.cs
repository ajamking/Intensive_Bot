namespace Exceptions;

public class UnknownException : Exception
{
    public static void ThrowByPredicate(Func<bool> predicate, string msg)
    {
        if (predicate())
        {
            throw new UnknownException(msg);
        }
    }

    public UnknownException()
    {
    }

    public UnknownException(string message) : base(message)
    {
    }

    public UnknownException(string message, Exception innerException) : base(message, innerException)
    {
    }
}