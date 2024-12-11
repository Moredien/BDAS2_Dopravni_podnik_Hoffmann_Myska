using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels;

public partial class LogyViewModel : ViewModelBase
{
    private DatabaseService _databaseService = new();
    [ObservableProperty] private ObservableCollection<Logy> _items;

    [ObservableProperty]
    private Logy? _selectedItem;

    private const int NumberOfFetchedRows = 500;

    public LogyViewModel()
    {
        Items = new();

        LoadDataAsync();
    }

    [RelayCommand]
    private void Detail()
    {
        if (SelectedItem == null)
        {
            MessageBox.Show("Nebyl vybrán žádný záznam", "Prazdny vyber", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        WindowManager.SetContentView(typeof(LogyFormViewModel), new object[] { SelectedItem });
    }
    private async Task LoadDataAsync()
    {
        try
        {
            var data = await Task.Run(() =>
                _databaseService.FetchData<Logy>($"SELECT * FROM LOGY ORDER BY CAS DESC FETCH FIRST {NumberOfFetchedRows} ROWS ONLY"));

            foreach (var entry in data)
            {
                Items.Add(entry);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}