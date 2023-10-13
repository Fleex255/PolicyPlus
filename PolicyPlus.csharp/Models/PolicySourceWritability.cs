namespace PolicyPlus.csharp.Models
{
    public enum PolicySourceWritability
    {
        Writable, // Full writability
        NoCommit, // Enable the OK button, but don't try to save
        NoWriting // Disable the OK button (there's no buffer)
    }
}