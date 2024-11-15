using System.Windows;
using System.Windows.Controls;

namespace DopravniPodnik.Utils;

public class ViewLocator : DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item == null) return null;

        var viewModelType = item.GetType();

        // fix so it doesnt try to create views for custom datepicker template
        if (!viewModelType.FullName.Contains("ViewModel"))
            return null;
        
        var viewNamespace = viewModelType.Namespace;

        // Replace "ViewModels" with "Views" in the namespace
        var viewNamespaceAdjusted = viewNamespace?.Replace("ViewModels", "Views");

        // Replace "ViewModel" with "View" in the type name
        var viewTypeName = viewModelType.FullName?.Replace("ViewModel", "View");

        // Adjust namespace for the view
        if (viewNamespaceAdjusted != null && viewTypeName != null)
        {
            viewTypeName = viewTypeName.Replace(viewNamespace, viewNamespaceAdjusted);
        }

        // Try to get the view's type
        var viewType = Type.GetType(viewTypeName);
        if (viewType == null) return null;

        // Create the view
        var view = (FrameworkElement)Activator.CreateInstance(viewType);
        return new DataTemplate { VisualTree = new FrameworkElementFactory(view.GetType()) };
    }
}
