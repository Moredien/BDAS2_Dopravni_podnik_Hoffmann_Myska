namespace DopravniPodnik.Utils;

public class ListItemTemplate
{
    public ListItemTemplate(string label, Type modelType,Type viewModelType)
    {
        Label = label;
        ModelType = modelType;
        ViewModelType = viewModelType;
    }

    public string Label { get; }
    public Type ModelType { get; }
    public Type ViewModelType { get; }
}