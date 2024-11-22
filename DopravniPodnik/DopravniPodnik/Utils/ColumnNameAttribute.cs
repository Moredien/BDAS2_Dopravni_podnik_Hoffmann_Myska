namespace DopravniPodnik.Utils;

public class ColumnNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}