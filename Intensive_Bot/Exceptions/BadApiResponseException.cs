namespace Exceptions;

public class BadApiResponseException : Exception
{
    public static void ThrowByPredicate(Func<bool> predicate, string msg)
    {
        if (predicate())
        {
            throw new BadApiResponseException(msg);
        }
    }

    public BadApiResponseException()
    {
    }

    public BadApiResponseException(string message) : base(message)
    {
    }

    public BadApiResponseException(string message, Exception innerException) : base(message, innerException)
    {
    }
}