using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DopravniPodnik.ViewModels;

namespace DopravniPodnik.Views;

public partial class GenericGridView : UserControl
{
    public GenericGridView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (DataContext is GenericGridViewModel viewModel)
        {
            // Clear existing columns
            DynamicDataGrid.Columns.Clear();

            // Add columns dynamically
            foreach (var columnInfo in viewModel.Columns)
            {
                var column = new DataGridTextColumn
                {
                    Header = columnInfo.Header,
                    Binding = new Binding(columnInfo.BindingPath)
                };
                DynamicDataGrid.Columns.Add(column);
            }
        }
    }
    
}