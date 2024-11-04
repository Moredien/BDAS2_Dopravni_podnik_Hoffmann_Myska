namespace DopravniPodnik.Utils;

public class ListItemTemplate
{
    public ListItemTemplate(string label, Type modelType, ViewType viewType)
    {
        Label = label;
        ModelType = modelType;
        ViewTypeEnum = viewType;
    }

    public string Label { get; }
    public Type ModelType { get; }
    public ViewType ViewTypeEnum{get;}
}