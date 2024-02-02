[Serializable]
internal class ConcurrencyException : Exception
{
    public ConcurrencyException(string? message) : base(message)
    {
    }
}