namespace NimbleFrame
{
    public interface IQMessage
    {
        bool ShowTag { get; }
        string AppName { get; }
        string GUID { get; }
        Priority Priority { get; }
        string Version { get; }

        string Process(string message);
    }
}