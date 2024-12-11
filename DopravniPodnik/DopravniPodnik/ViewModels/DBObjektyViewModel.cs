using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class DBObjektyViewModel : ViewModelBase
{
    private DatabaseService _databaseService = new();
    [ObservableProperty] private ObservableCollection<DBObjektyDTO> _items;

    public DBObjektyViewModel()
    {
        Items = new();
        LoadDataAsync();
    }
    private async void LoadDataAsync()
    {
        try
        {
            var data = await Task.Run(() =>
                _databaseService.FetchData<DBObjektyDTO>($"SELECT * FROM DB_OBJEKTY_VIEW"));
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