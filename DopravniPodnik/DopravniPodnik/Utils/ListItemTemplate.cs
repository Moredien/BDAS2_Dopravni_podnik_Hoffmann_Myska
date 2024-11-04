namespace DopravniPodnik.Utils;

public class ListItemTemplate
{
    public ListItemTemplate(string label, Type modelType)
    {
        Label = label;
        ModelType = modelType;
    }

    public string Label { get; }
    public Type ModelType { get; }
}