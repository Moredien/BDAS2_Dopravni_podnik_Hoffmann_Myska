namespace DopravniPodnik.Utils;

public class Logger
{
    private static Logger? _instance;
    private static readonly object LockObject = new();

    private string _logLevel = string.Empty;
    private string _message = string.Empty;

    private Logger() { }
    public static Logger Instance
    {
        get
        {
            lock (LockObject)
            {
                return _instance ??= new Logger();
            }
        }
    }

    public Logger Message(string message)
    {
        _message = message;
        return this;
    }

    public Logger Info()
    {
        _logLevel = "INFO";
        return this;
    }

    public Logger Warning()
    {
        _logLevel = "WARNING";
        return this;
    }

    public Logger Error()
    {
        _logLevel = "ERROR";
        return this;
    }

    public Logger Exception(Exception ex)
    {
        _logLevel = "ERROR";
        _message = $"{ex.Message}\nStackTrace: {ex.StackTrace}";
        return this;
    }

    public void Log()
    {
        Console.WriteLine($"{_logLevel}: {_message}");
        Clear();
    }

    private void Clear()
    {
        _logLevel = string.Empty;
        _message = string.Empty;
    }
}