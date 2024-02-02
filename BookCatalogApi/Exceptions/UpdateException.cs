[Serializable]
internal class UpdateException : Exception
{
    public UpdateException(string? message) : base(message)
    {
    }

}