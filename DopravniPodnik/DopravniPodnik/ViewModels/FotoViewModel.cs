using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class FotoViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    [ObservableProperty]
    private ObservableCollection<FotoDTO> _items;
    [ObservableProperty]
    private FotoDTO? _selectedItem;

    public FotoViewModel()
    {
        Items = new();
        LoadItemsAsync();
    }

    private async void LoadItemsAsync()
    {
        try
        {
            var data = await Task.Run(() =>
                _databaseService.FetchData<FotoDTO>("SELECT * FROM FOTO_VIEW"));

            foreach (var entry in data)
            {
                Items.Add(entry);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error while loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void Detail()
    {
        if(SelectedItem!=null)
            WindowManager.SetContentView(typeof(FotoDetailsViewModel), new object[] { SelectedItem.IdFoto });
    }
}