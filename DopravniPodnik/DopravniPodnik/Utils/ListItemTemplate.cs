namespace DopravniPodnik.Utils;

public class ListItemTemplate
{
    public ListItemTemplate(string label, Type modelType,string key)
    {
        Label = label;
        ModelType = modelType;
        Key = key;
    }

    public string Label { get; }
    public Type ModelType { get; }
    public string Key { get; }
}